using UnityEngine;

public class FoodItem : ConsumableItem
{
    public string ItemType { get; private set; }  // 아이템 타입을 저장하는 필드 추가

    public FoodItem(string name, int id, string description, int maxAmount, string itemType, int initialAmount, int healthRecovery, int fullnessRecovery)
        : base(name, id, description, maxAmount, initialAmount, healthRecovery, fullnessRecovery)
    {
        ItemType = itemType;  // itemType 초기화
    }

    public override void Use()
    {
        base.Use();
        Debug.Log($"{Name}을(를) 사용하여 체력을 회복했습니다. (타입: {ItemType})");
    }
}
