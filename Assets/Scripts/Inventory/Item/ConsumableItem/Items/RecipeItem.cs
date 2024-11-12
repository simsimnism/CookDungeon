using System.Collections.Generic;
using UnityEngine;

public class RecipeItem : Item
{
    public int HealthRecovery { get; private set; }
    public int FullnessRecovery { get; private set; }
    public List<int> RequiredIngredients { get; private set; }

    public RecipeItem(string name, int id, string description, int maxAmount, int initialAmount, int healthRecovery, int fullnessRecovery, List<int> requiredIngredients, Sprite itemSprite)
        : base(name, id, description, maxAmount, initialAmount, itemSprite)
    {
        HealthRecovery = healthRecovery;
        FullnessRecovery = fullnessRecovery;
        RequiredIngredients = requiredIngredients;
        SetAmount(1);
    }

    public override void Use()
    {
        // 레시피 아이템의 고유 사용 기능 추가
        Debug.Log($"{Name}은(는) 요리 레시피입니다. 사용할 수 없습니다.");
    }
}
