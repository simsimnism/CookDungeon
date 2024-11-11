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
    private const float doubleClickThreshold = 0.3f;  // 더블 클릭 감지 시간 간격

    public Item CurrentItem
    {
        get => currentItem;
        set => currentItem = value;
    }

    private void Awake()
    {
        // 드래그 이미지 설정
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


    //더블클릭을 감지하는 코드
    public void OnPointerClick(PointerEventData eventData)
    {
        // 클릭 시간 간격을 확인하여 더블 클릭 감지
        if (Time.time - lastClickTime <= doubleClickThreshold)
        {
            UseItem();  // 더블 클릭이 감지되면 아이템 사용
        }
        lastClickTime = Time.time;
    }

    //아이템 사용 코드
    private void UseItem()
    {
        // 아이템이 RecipeItem인 경우에만 사용
        if (currentItem is RecipeItem recipeItem)
        {
            recipeItem.Use();  // 레시피 아이템 사용
            Debug.Log($"{recipeItem.Name} 레시피 아이템을 사용했습니다.");
            // 수량이 0이면 슬롯을 초기화
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
            currentItem.Use();  // 기타 일반 아이템 사용
            Debug.Log($"{currentItem.Name} 아이템을 사용했습니다.");
            // 수량이 0이면 슬롯을 초기화
            if (currentItem.Amount <= 0)
            {
                ClearSlot();
            }
        }
        else
        {
            Debug.LogWarning("사용할 아이템이 없습니다.");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        draggedImage.sprite = itemImage.sprite;
        draggedImage.transform.position = eventData.position;
        draggedImage.gameObject.SetActive(true);
        SetImageAlpha(1f);  // 드래그 중에는 슬롯의 이미지가 보이지 않도록 설정
    }

    //드래그에 사용되는 코드
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
        SetImageAlpha(1f);  // 드래그가 끝나면 원래 슬롯의 이미지가 다시 보이도록 설정

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<BaseItemSlot>(out BaseItemSlot targetSlot) && targetSlot != this)
            {
                if (CanSwap(targetSlot))
                {
                    MoveItemToSlot(targetSlot);  // 타겟 슬롯에 아이템 이동
                }
                break;
            }
        }
    }

    private void MoveItemToSlot(BaseItemSlot targetSlot)
    {
        targetSlot.SetItem(currentItem);  // 타겟 슬롯에 아이템 설정
        ClearSlot();  // 원래 슬롯을 초기화하여 아이템 제거
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

    private void ConfirmSwap(BaseItemSlot targetSlot)
    {
        ShowConfirmationDialog(() => SwapItems(targetSlot));
    }

    private void ShowConfirmationDialog(System.Action onConfirm)
    {
        bool userConfirmed = true;  // 임시로 확인된 상태를 가정
        if (userConfirmed)
        {
            onConfirm?.Invoke();
        }
    }

    public void ClearCache()
    {
        Debug.Log("캐시가 초기화되었습니다.");
        currentItem = null;
    }
}