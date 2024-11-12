using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class BaseItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemImage;
    private Image draggedImage;
    protected Item currentItem;

    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f;

    public Item CurrentItem
    {
        get => currentItem;
        set => currentItem = value;
    }

    private void Awake()
    {
        draggedImage = new GameObject("DraggedImage").AddComponent<Image>();
        draggedImage.sprite = itemImage.sprite;
        draggedImage.raycastTarget = false;
        draggedImage.transform.SetParent(transform.root);
        draggedImage.transform.SetAsLastSibling();
        draggedImage.gameObject.SetActive(false);
    }

    public virtual void SetItem(Item item)
    {
        currentItem = item;
        if (currentItem != null)
        {
            itemImage.sprite = currentItem.ItemSprite;
            itemImage.enabled = true;
            SetImageAlpha(1f);
        }
        else
        {
            ClearSlot();
        }
    }

    public abstract void ClearSlot();
    public abstract bool IsEmpty();

    public void SetImageAlpha(float alpha)
    {
        var color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - lastClickTime <= doubleClickThreshold)
        {
            UseItem();
        }
        lastClickTime = Time.time;
    }

    private void UseItem()
    {
        if (currentItem is RecipeItem recipeItem)
        {
            recipeItem.Use();
            if (recipeItem.Amount <= 0)
            {
                ClearSlot();
            }
            Managers.Popup.ToggleSkillUI();
        }
        else if (currentItem is FoodItem)
        {
            Debug.LogWarning($"{currentItem.Name}은(는) 푸드 아이템이므로 사용할 수 없습니다.");
        }
        else if (currentItem != null)
        {
            currentItem.Use();
            if (currentItem.Amount <= 0)
            {
                ClearCache();
            }
        }
        else
        {
            Debug.LogWarning("사용할 아이템이 없습니다.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null || currentItem.Amount <= 0) return;

        draggedImage.sprite = itemImage.sprite;
        draggedImage.transform.position = eventData.position;
        draggedImage.gameObject.SetActive(true);

        SetImageAlpha(0f); // 드래그 중 원본 슬롯 이미지를 투명하게 설정
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedImage != null && draggedImage.gameObject.activeSelf)
        {
            draggedImage.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedImage != null)
        {
            draggedImage.gameObject.SetActive(false);
        }
        SetImageAlpha(1f); // 드래그 후 슬롯 이미지를 다시 보이게 설정

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<BaseItemSlot>(out BaseItemSlot targetSlot) && targetSlot != this)
            {
                if (CanSwap(targetSlot))
                {
                    MoveSingleItemToSlot(targetSlot);  // 타겟 슬롯에 아이템 이동
                }
                break;
            }
        }
        UpdateAmountTextInDerivedClasses();
    }

    private void MoveSingleItemToSlot(BaseItemSlot targetSlot)
    {
        Debug.Log("MoveSingleItemToSlot 호출됨");

        if (targetSlot.IsEmpty() || targetSlot.CurrentItem?.Name == currentItem.Name)
        {
            Item singleItemCopy = currentItem.Clone();
            singleItemCopy.SetAmount(1);
            Debug.Log($"복사된 아이템 - Name: {singleItemCopy.Name}");

            // targetSlot이 CookingSlot인지 확인하여 SetItem 호출
            if (targetSlot is CookingSlot cookingSlotTarget)
            {
                Debug.Log("CookingSlot으로 인식됨 - CookingSlot의 SetItem 호출");
                cookingSlotTarget.SetItem(singleItemCopy); // CookingSlot의 SetItem 호출
            }
            else if (targetSlot.IsEmpty())
            {
                Debug.Log("기본 BaseItemSlot의 SetItem 호출");
                targetSlot.SetItem(singleItemCopy); // 기본 BaseItemSlot의 SetItem 호출
            }
            else if (targetSlot.CurrentItem.Name == currentItem.Name)
            {
                targetSlot.CurrentItem.AddAmount(1);
            }

            currentItem.DecreaseAmount(1); // 인벤토리 슬롯에서 아이템 수량 차감

            UpdateAmountTextInDerivedClasses();
            targetSlot.UpdateAmountTextInDerivedClasses();

            if (currentItem.Amount <= 0)
            {
                ClearSlot();
            }
        }
    }


    protected virtual bool CanSwap(BaseItemSlot targetSlot)
    {
        return true;  // 슬롯 간의 교환이 가능할 때 true 반환
    }

    protected virtual void SwapItems(BaseItemSlot targetSlot)
    {
        Item tempItem = targetSlot.currentItem;
        targetSlot.SetItem(currentItem);
        SetItem(tempItem);
        ClearCache();
    }

    public void ClearCache()
    {
        Debug.Log("캐시가 초기화되었습니다.");
        currentItem = null;
    }

    private void ShowConfirmationDialog(System.Action onConfirm)
    {
        bool userConfirmed = true;  // 임시로 확인된 상태를 가정
        if (userConfirmed)
        {
            onConfirm?.Invoke();
        }
    }

    private void ConfirmSwap(BaseItemSlot targetSlot)
    {
        ShowConfirmationDialog(() => SwapItems(targetSlot));
    }

    public abstract void UpdateAmountTextInDerivedClasses();
}
