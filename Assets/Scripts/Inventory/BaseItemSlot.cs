using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public abstract class BaseItemSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Image itemImage;  // 슬롯에 표시되는 아이템 이미지
    public abstract Item GetItem();
    public abstract void SetItem(Item item);
    public abstract void ClearSlot();
    public abstract bool IsEmpty();

    private Image draggedImage;

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GetItem() == null) return;

        // 드래그 시 임시 이미지 생성
        draggedImage = new GameObject("DraggedImage").AddComponent<Image>();
        draggedImage.sprite = itemImage.sprite;
        draggedImage.raycastTarget = false;
        draggedImage.transform.SetParent(transform.root);  // 최상위에 추가하여 UI 이동
        draggedImage.transform.SetAsLastSibling();

        // 원래 슬롯 이미지를 숨김
        itemImage.enabled = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedImage != null)
        {
            draggedImage.transform.position = eventData.position;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (draggedImage != null)
        {
            Destroy(draggedImage.gameObject);
        }

        itemImage.enabled = true;

        // 마우스 위치에서 드롭 대상 슬롯을 정확히 감지
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        BaseItemSlot targetSlot = null;

        foreach (var result in results)
        {
            if (result.gameObject.TryGetComponent<BaseItemSlot>(out targetSlot) && targetSlot != this)
            {
                break;
            }
        }

        if (targetSlot != null)
        {
            // 아이템을 타겟 슬롯에 설정하고, 드래그 시작 슬롯 비우기
            targetSlot.SetItem(GetItem());
            ClearSlot();  // 원래 슬롯 비우기
        }
    }

}
