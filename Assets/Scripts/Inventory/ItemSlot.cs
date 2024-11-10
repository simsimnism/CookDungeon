using TMPro;
using UnityEngine;

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
            itemImage.sprite = item.ItemSprite;
            itemImage.enabled = true;
            SetImageAlpha(1f);
            UpdateAmountText();
        }
        else
        {
            ClearSlot();
        }
    }

    public override Item GetItem() => currentItem;

    public override void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        SetImageAlpha(0f);
        cachedItem = null;
        if (itemAmountText != null)
            itemAmountText.text = "";
    }

    public override bool IsEmpty() => currentItem == null;

    public void UpdateAmountText()
    {
        if (itemAmountText != null)
            itemAmountText.text = currentItem?.Amount > 1 ? currentItem.Amount.ToString() : "";
    }

    private void Start()
    {
        if (currentItem != null && currentItem.ItemSprite != null)
        {
            string spriteName = itemImage.sprite?.name ?? "";
            if (!string.IsNullOrEmpty(spriteName))
            {
                LoadItemData(spriteName);
            }
        }
    }
}
