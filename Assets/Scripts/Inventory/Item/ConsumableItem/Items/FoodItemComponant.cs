using UnityEngine;

public class FoodItemComponent : MonoBehaviour
{
    public string itemName;
    public int itemId;
    public string description;
    public int maxAmount;
    public int initialAmount;
    public int healthRecovery;
    public int fullnessRecovery;

    private FoodItem foodItem;

    void Start()
    {
        // FoodItem 데이터를 초기화
        foodItem = new FoodItem(itemName, itemId, description, maxAmount, initialAmount, healthRecovery, fullnessRecovery);
    }

    // FoodItem 데이터를 사용할 수 있도록 메서드 제공
    public void UseItem()
    {
        if (foodItem != null)
        {
            foodItem.Use();
        }
    }

    // FoodItem 데이터를 반환하는 메서드
    public FoodItem GetFoodItem()
    {
        return foodItem;
    }
}
