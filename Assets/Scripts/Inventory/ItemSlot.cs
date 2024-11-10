using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : BaseItemSlot
{
    public Inventory parentInventory;
    public Item currentItem;
    public TMP_Text itemAmountText;

    public override void SetItem(Item item)
    {
        currentItem = item;
        if (item != null && item.ItemSprite != null)
        {
            itemImage.sprite = item.ItemSprite;  // 아이템 이미지 설정
            itemImage.enabled = true;  // 이미지 표시

            // 불투명하게 설정하여 보이게 하기
            var color = itemImage.color;
            color.a = 1f;  // 알파 값을 1로 설정하여 완전히 보이도록 함
            itemImage.color = color;
        }
        else
        {
            ClearSlot();  // 아이템이 없거나 이미지가 없는 경우 슬롯 비우기
        }
        UpdateAmountText();
    }

    public override Item GetItem() => currentItem;

    public override void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;

        // 투명하게 설정하여 이미지가 보이지 않도록 함
        var color = itemImage.color;
        color.a = 0f;  // 알파 값을 0으로 설정하여 투명하게 만듦
        itemImage.color = color;

        if (itemAmountText != null)
            itemAmountText.text = "";
    }

    public override bool IsEmpty() => currentItem == null;

    public void UpdateAmountText()
    {
        if (itemAmountText != null)
            itemAmountText.text = currentItem?.Amount > 1 ? currentItem.Amount.ToString() : "";
    }
}
