using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemImage;
    public TMP_Text itemAmountText;
    public Item currentItem;
    public Inventory parentInventory;

    private void Awake()
    {
        currentItem = null; // currentItem을 명시적으로 null로 초기화
        if (itemImage == null || itemAmountText == null)
        {
            Debug.LogError("ItemSlot UI 요소가 설정되지 않았습니다.");
        }
    }

    public void SetItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("유효하지 않은 아이템입니다.");
            return;
        }

        currentItem = item;
        itemImage.sprite = item.ItemSprite;
        itemImage.enabled = true;
        itemAmountText.text = item.Amount.ToString();
        itemAmountText.enabled = item.Amount > 1;

        Debug.Log($"아이템 설정 완료: {item.Name}. currentItem 상태 업데이트됨.");
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemAmountText.text = "";
        itemAmountText.enabled = false;

        Debug.Log("슬롯 비우기 완료. currentItem은 null로 설정되었습니다.");
    }

    public bool IsEmpty()
    {
        bool isEmpty = currentItem == null; // 빈 슬롯 여부를 확인
        Debug.Log($"슬롯이 비어 있는가? {isEmpty}");
        return isEmpty;
    }
}
