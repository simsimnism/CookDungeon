using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BaseItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemImage;
    private Image draggedImage;
    protected Item cachedItem;

    private void Awake()
    {
        draggedImage = new GameObject("DraggedImage").AddComponent<Image>();
        draggedImage.sprite = itemImage.sprite;
        draggedImage.raycastTarget = false;
        draggedImage.transform.SetParent(transform.root);
        draggedImage.transform.SetAsLastSibling();
        draggedImage.gameObject.SetActive(false);
    }

    public abstract Item GetItem();
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
        if (GetItem() == null) return;

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
        Item tempItem = targetSlot.GetItem();
        targetSlot.SetItem(GetItem());
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

    // BaseItemSlot.cs 내에 추가
    public void LoadItemData(string spriteName)
    {
        Item item = Managers.Data.GetFoodItemByPrefabName(spriteName) ?? Managers.Data.GetRecipeItemByPrefabName(spriteName);
        if (item != null)
        {
            SetItem(item);
            cachedItem = item;
            Debug.Log($"{spriteName}의 데이터를 성공적으로 로드했습니다.");
        }
        else
        {
            Debug.LogWarning($"'{spriteName}'에 해당하는 JSON 데이터를 찾을 수 없습니다.");
        }
    }


    public void ClearCache()
    {
        cachedItem = null;
        Debug.Log("캐시가 초기화되었습니다.");
    }
}
