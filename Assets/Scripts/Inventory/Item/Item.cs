using UnityEngine;

[System.Serializable]
public class Item
{
    public string Name { get; private set; }
    public int ID { get; private set; }
    public string Description { get; private set; }
    public int MaxAmount { get; private set; } = 99; // 기본 최대 수량을 99로 설정
    public int Amount { get; protected set; } = 1;
    public Sprite ItemSprite { get; set; }

    public Item(string name, int id, string description, int maxAmount, int initialAmount, Sprite itemSprite)
    {
        Name = name;
        ID = id;
        Description = description;
        MaxAmount = maxAmount > 0 ? maxAmount : 99; // MaxAmount가 0 이하이면 기본값 99로 설정
        Amount = Mathf.Clamp(initialAmount > 0 ? initialAmount : 1, 0, MaxAmount); // 기본 수량을 최소 1로 설정
        ItemSprite = itemSprite;
    }

    public virtual void Use()
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

    public virtual void DecreaseAmount(int amount)
    {
        Amount = Mathf.Max(Amount - amount, 0);
    }

    public virtual void AddAmount(int amount)
    {
        Amount = Mathf.Clamp(Amount + amount, 0, MaxAmount);
    }
    // 새로 추가된 메서드: SetAmount
    public void SetAmount(int amount)
    {
        Amount = Mathf.Clamp(amount, 0, MaxAmount);
    }
}
