using UnityEngine;

public class FoodItem : Item
{
    public int ItemType { get; private set; }
    public int HealthRecovery { get; private set; }
    public int FullnessRecovery { get; private set; }

    public FoodItem(string name, int id, string description, int maxAmount, int itemType, int healthRecovery, int fullnessRecovery, Sprite itemSprite)
        : base(name, id, description, maxAmount, maxAmount, itemSprite)
    {
        ItemType = itemType;
        HealthRecovery = healthRecovery;
        FullnessRecovery = fullnessRecovery;
    }


    public override void Use()
    {
        base.Use();
        if (Amount > 0)
        {
            Debug.Log($"{Name}을(를) 사용하여 체력 {HealthRecovery}과 포만도 {FullnessRecovery}을 회복했습니다.");
        }
    }
}
