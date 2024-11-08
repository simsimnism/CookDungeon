using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemImage;         // 슬롯의 아이템 이미지
    public Text itemAmountText;     // 슬롯의 아이템 개수 텍스트

    public Item currentItem;

    // 슬롯에 아이템을 설정하는 메서드
    public void SetItem(Item item)
    {
        currentItem = item;
        itemImage.sprite = item.ItemSprite;
        itemImage.enabled = true;
        itemAmountText.text = item.Amount.ToString();
        itemAmountText.enabled = item.Amount > 1;  // 아이템 개수가 1보다 클 때만 개수 표시
    }

    // 슬롯을 비우는 메서드
    public void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemAmountText.text = "";
        itemAmountText.enabled = false;
    }
}
