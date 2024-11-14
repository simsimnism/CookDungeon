using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

public abstract class BaseItemSlot : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Image itemImage;
    public TMP_Text itemDescriptionText; // 아이템 설명 표시용 TMP 텍스트
    private Image draggedImage;
    protected Item currentItem;

    private float lastClickTime;
    private const float doubleClickThreshold = 0.3f;

    private Player Player;

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

    void Start()
    {
        Player = GetComponent<Player>() ?? FindObjectOfType<Player>();
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
            else
            {
                UpdateAmountTextInDerivedClasses();
            }
            if (recipeItem.Name == "Omelet")
            {
                Player.RecoverHealth();
            }
            else
            {
                Managers.Popup.ToggleSkillUI();
            }
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
        if (targetSlot.IsEmpty() || targetSlot.CurrentItem?.Name == currentItem.Name)
        {
            Item singleItemCopy = currentItem.Clone();
            singleItemCopy.SetAmount(1);

            if (targetSlot is CookingSlot cookingSlotTarget)
            {
                cookingSlotTarget.SetItem(singleItemCopy);
            }
            else if (targetSlot.IsEmpty())
            {
                targetSlot.SetItem(singleItemCopy);
            }
            else if (targetSlot.CurrentItem.Name == currentItem.Name)
            {
                targetSlot.CurrentItem.AddAmount(1);
            }

            currentItem.DecreaseAmount(1);

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
        return true;
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
        currentItem = null;
    }

    private void ShowConfirmationDialog(System.Action onConfirm)
    {
        bool userConfirmed = true;
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

    // 마우스 오버 시 설명 표시 및 숨기기
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (currentItem != null && itemDescriptionText != null)
        {
            itemDescriptionText.text = currentItem.Description;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (itemDescriptionText != null)
        {
            itemDescriptionText.text = "";
        }
    }
}
