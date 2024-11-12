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
        if (Amount > 0)
        {
            Amount--;
            Debug.Log($"{Name}을(를) 사용했습니다. 남은 수량: {Amount}");
        }
        else
        {
            Debug.Log($"{Name}의 수량이 모두 소진되었습니다.");
        }
    }
}
