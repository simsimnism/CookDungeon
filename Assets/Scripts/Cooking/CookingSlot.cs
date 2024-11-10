using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CookingSlot : BaseItemSlot
{
    public TMP_Text itemAmountText;
    public FoodItem currentFoodItem;

    public override void SetItem(Item item)
    {
        if (item is FoodItem foodItem)
        {
            currentFoodItem = foodItem;

            // 스프라이트 설정
            if (currentFoodItem.ItemSprite != null)
            {
                itemImage.sprite = currentFoodItem.ItemSprite;
                itemImage.enabled = true;
                var color = itemImage.color;
                color.a = 1f;
                itemImage.color = color;
            }
            else
            {
                ClearSlot();
            }

            // 요리 UI 상태 업데이트
            FindObjectOfType<CookingUI>()?.UpdateCookingState();
        }
        else
        {
            ClearSlot();
        }
    }

    public override Item GetItem() => currentFoodItem;

    public override void ClearSlot()
    {
        currentFoodItem = null;
        itemImage.sprite = null;
        itemImage.enabled = false;

        // 투명하게 설정하여 이미지가 보이지 않도록 함
        var color = itemImage.color;
        color.a = 0f;
        itemImage.color = color;

        if (itemAmountText != null)
            itemAmountText.text = "";
    }

    public override bool IsEmpty() => currentFoodItem == null;

    public void UpdateAmountText()
    {
        if (itemAmountText != null)
            itemAmountText.text = currentFoodItem?.Amount > 1 ? currentFoodItem.Amount.ToString() : "";
    }
}
