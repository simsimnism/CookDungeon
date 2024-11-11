using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BaseItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemImage;
    private Image draggedImage;
    protected Item currentItem;  // 공통적으로 사용할 필드로 변경

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

    public abstract void SetItem(Item item);
    public abstract void ClearSlot();
    public abstract bool IsEmpty();

    protected void SetImageAlpha(float alpha)
    {
        var color = itemImage.color;
        color.a = alpha;
        itemImage.color = color;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentItem == null) return;

        draggedImage.sprite = itemImage.sprite;
        draggedImage.transform.position = eventData.position;
        draggedImage.gameObject.SetActive(true);
        SetImageAlpha(0f);
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
        SetImageAlpha(1f);

        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<BaseItemSlot>(out BaseItemSlot targetSlot) && targetSlot != this)
            {
                if (CanSwap(targetSlot))
                {
                    ConfirmSwap(targetSlot);
                }
                break;
            }
        }
        ClearCache();
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

    private void ConfirmSwap(BaseItemSlot targetSlot)
    {
        ShowConfirmationDialog(() => SwapItems(targetSlot));
    }

    private void ShowConfirmationDialog(System.Action onConfirm)
    {
        bool userConfirmed = true;
        if (userConfirmed)
        {
            onConfirm?.Invoke();
        }
    }

    public void ClearCache()
    {
        currentItem = null;  // 공통 필드를 초기화
        Debug.Log("캐시가 초기화되었습니다.");
    }
}
