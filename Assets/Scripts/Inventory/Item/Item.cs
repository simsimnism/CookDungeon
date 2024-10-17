using UnityEngine;

[System.Serializable]
public class Item
{
    public string Name { get; private set; }
    public int ID { get; private set; }
    public string Description { get; private set; }
    public int MaxAmount { get; private set; }
    public int Amount { get; protected set; }

    public Item(string name, int id, string description, int maxAmount, int initialAmount)
    {
        Name = name;
        ID = id;
        Description = description;
        MaxAmount = maxAmount;
        Amount = Mathf.Clamp(initialAmount, 0, MaxAmount); // 초기 개수 설정
    }

    public virtual void Use()
    {
        Debug.Log($"{Name}을(를) 사용했습니다.");
        Amount = Mathf.Max(Amount - 1, 0); // 아이템 사용 시 개수 감소
    }

    public void AddAmount(int amount)
    {
        Amount = Mathf.Clamp(Amount + amount, 0, MaxAmount); // 수량 추가
    }
}
