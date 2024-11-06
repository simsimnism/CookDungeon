using System.Collections.Generic;
using UnityEngine;

public class RecipeItem : ConsumableItem
{
    public List<int> RequiredIngredients { get; private set; }

    public RecipeItem(string name, int id, string description, int maxAmount, int initialAmount, int healthRecovery, int fullnessRecovery, List<int> requiredIngredients, Sprite itemSprite)
        : base(name, id, description, maxAmount, initialAmount, healthRecovery, fullnessRecovery, itemSprite) // 스프라이트 전달
    {
        RequiredIngredients = requiredIngredients;
    }

    public override void Use()
    {
        base.Use();
        // 레시피 조합 로직 추가 가능
        Debug.Log($"{Name} 레시피를 사용하여 아이템을 조합할 수 있습니다.");
    }
}
