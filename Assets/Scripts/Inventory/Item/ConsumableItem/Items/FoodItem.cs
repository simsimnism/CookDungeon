using UnityEngine;

public class FoodItem : ConsumableItem
{
    public FoodItem(string name, int id, string description, int maxAmount, int initialAmount, int healthRecovery, int fullnessRecovery)
        : base(name, id, description, maxAmount, initialAmount, healthRecovery, fullnessRecovery)
    {
    }

    public override void Use()
    {
        base.Use();
        // 추가적인 음식 관련 처리 가능
        Debug.Log($"{Name}을(를) 사용하여 체력을 회복했습니다.");
    }
}
