using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemImage;
    public Text itemAmountText;
    public Item currentItem;
    public Inventory parentInventory;

    // 아이템을 설정하는 메서드
    public void SetItem(Item item)
    {
        currentItem = item;
        itemImage.sprite = item.ItemSprite;
        itemImage.enabled = true;
        itemAmountText.text = item.Amount.ToString();
        itemAmountText.enabled = item.Amount > 1;

        Debug.Log($"아이템 설정 완료: {item.Name}. currentItem 상태 업데이트됨.");
    }

    // 슬롯을 비우는 메서드
    public void ClearSlot()
    {
        currentItem = null; // currentItem을 null로 설정하여 슬롯 비우기
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemAmountText.text = "";
        itemAmountText.enabled = false;

        Debug.Log("슬롯 비우기 완료. currentItem은 null로 설정되었습니다.");
    }

    // 슬롯이 비어 있는지 확인하는 메서드
    public bool IsEmpty()
    {
        return currentItem == null; // currentItem이 null이면 빈 슬롯으로 간주
    }
}
