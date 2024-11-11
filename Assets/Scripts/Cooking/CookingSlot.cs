using TMPro;
using UnityEngine;

public class CookingSlot : BaseItemSlot
{
    public Inventory parentInventory;
    public TMP_Text itemAmountText;

    public override void SetItem(Item item)
    {
        ClearSlot();  // 기존 데이터를 초기화하여 캐시 문제 방지
        if (item is FoodItem foodItem)
        {
            currentItem = foodItem;
            itemImage.sprite = foodItem.ItemSprite;
            itemImage.enabled = true;
            SetImageAlpha(1f);
            UpdateAmountText();

            if (parentInventory != null)
            {
                parentInventory.RemoveItemByName(foodItem.Name);
            }
        }
        else
        {
            ClearSlot();
        }
    }

    public override void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        SetImageAlpha(0f);
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
