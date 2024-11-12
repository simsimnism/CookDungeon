using TMPro;
using UnityEngine;

public class CookingSlot : BaseItemSlot
{
    public Inventory parentInventory;
    public TMP_Text itemAmountText;

    public override void SetItem(Item item)
    {
        Debug.Log($"SetItem 호출됨 - item: {item?.Name}");

        ClearSlot(); // 기존 데이터를 초기화하여 캐시 문제 방지

        if (item is FoodItem foodItem)
        {
            currentItem = foodItem;
            itemImage.sprite = foodItem.ItemSprite;
            itemImage.enabled = true;
            SetImageAlpha(1f);
            UpdateAmountText();
            Debug.Log($"currentItem 할당됨 - currentItem: {currentItem?.Name}");
        }
        else
        {
            ClearSlot();
            Debug.Log("FoodItem이 아니라서 ClearSlot 호출됨");
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

    public override void UpdateAmountTextInDerivedClasses()
    {
        UpdateAmountText();
    }
}
