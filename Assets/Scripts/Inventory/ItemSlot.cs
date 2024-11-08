using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemImage;
    public Text itemAmountText;
    public Item currentItem;
    public Inventory parentInventory;

    public void SetItem(Item item)
    {
        currentItem = item;
        itemImage.sprite = item.ItemSprite;
        itemImage.enabled = true;
        itemAmountText.text = item.Amount.ToString();
        itemAmountText.enabled = item.Amount > 1;
    }

    public void ClearSlot()
    {
        currentItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        itemAmountText.text = "";
        itemAmountText.enabled = false;

        if (parentInventory != null)
        {
            parentInventory.AddEmptySlotIndex(this);  // ºó ½½·Ô Å¥¿¡ ÇöÀç ½½·Ô Ãß°¡
        }
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }
}
