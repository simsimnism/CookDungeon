using UnityEngine;

public class ConsumableItem : Item
{
    public int HealthRecovery { get; private set; }
    public int FullnessRecovery { get; private set; }

    public ConsumableItem(string name, int id, string description, int maxAmount, int initialAmount, int healthRecovery, int fullnessRecovery, Sprite itemSprite)
        : base(name, id, description, maxAmount, initialAmount, itemSprite)
    {
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
