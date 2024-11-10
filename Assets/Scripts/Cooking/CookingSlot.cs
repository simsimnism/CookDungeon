using TMPro;
using UnityEngine;

public class CookingSlot : BaseItemSlot
{

    // CookingSlot.cs 내에 추가
    public Inventory parentInventory;

    public FoodItem currentFoodItem;
    public TMP_Text itemAmountText;



    public override void SetItem(Item item)
    {
        if (item is FoodItem foodItem)
        {
            currentFoodItem = foodItem;
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

    public override Item GetItem() => currentFoodItem;

    // CookingSlot.cs
    public override void ClearSlot()
    {
        currentFoodItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;
        SetImageAlpha(0f);
        cachedItem = null;
        if (itemAmountText != null)
            itemAmountText.text = "";
    }


    public override bool IsEmpty() => currentFoodItem == null;

    public void UpdateAmountText()
    {
        if (itemAmountText != null)
            itemAmountText.text = currentFoodItem?.Amount > 1 ? currentFoodItem.Amount.ToString() : "";
    }

    private void Start()
    {
        if (parentInventory == null)
        {
            Debug.LogWarning("parentInventory가 설정되지 않았습니다. 수동으로 할당했는지 확인하세요.");
        }

        if (currentFoodItem != null && currentFoodItem.ItemSprite != null)
        {
            string spriteName = itemImage.sprite?.name ?? "";
            if (!string.IsNullOrEmpty(spriteName))
            {
                LoadItemData(spriteName);
            }
        }
    }
}
