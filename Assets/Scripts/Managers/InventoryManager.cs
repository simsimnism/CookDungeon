using System.Collections.Generic;
using System;

public class InventoryManager
{
    List<Item> _items = new List<Item>();

    public int GetInventorySize()
    {
        return _items.Count; // 인벤토리에 있는 아이템 개수를 반환
    }

    public Item GetItem(int index)
    {
        if (index >= 0 && index < _items.Count)
            return _items[index];
        return null;
    }

    // 아이템 추가 로직
    public void AddItem(Item item)
    {
        _items.Add(item);
        OnInventoryChanged?.Invoke(); // UI 업데이트를 위한 이벤트 호출
    }

    public event Action OnInventoryChanged;
}
