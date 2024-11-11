using System.Collections.Generic;
using UnityEngine;

public class RecipeItem : FoodItem
{
    public List<int> RequiredIngredients { get; private set; }

    public RecipeItem(string name, int id, string description, int maxAmount, int itemType, int healthRecovery, int fullnessRecovery, List<int> requiredIngredients, Sprite itemSprite)
        : base(name, id, description, maxAmount, itemType, healthRecovery, fullnessRecovery, itemSprite)
    {
        RequiredIngredients = requiredIngredients;
    }

    public override void Use()
    {
        base.Use();
        if (Amount > 0)
        {
            Debug.Log($"{Name} 레시피 사용으로 조리 시작!");
            // 필요한 재료가 있는지 체크 로직 추가 가능
        }
    }
}
