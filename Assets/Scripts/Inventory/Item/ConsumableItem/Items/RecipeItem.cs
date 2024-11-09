using System.Collections.Generic;
using UnityEngine;

public class RecipeItem : FoodItem
{
    public List<int> RequiredIngredients { get; private set; }

    public RecipeItem(string name, int id, string description, int maxAmount, string itemType, int healthRecovery, int fullnessRecovery, List<int> requiredIngredients, Sprite itemSprite)
        : base(name, id, description, maxAmount, itemType, healthRecovery, fullnessRecovery, itemSprite)
    {
        RequiredIngredients = requiredIngredients;
    }
}
