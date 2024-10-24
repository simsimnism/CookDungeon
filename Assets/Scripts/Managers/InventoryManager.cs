using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public List<Item> items = new List<Item>();  // 아이템 리스트

    // 저장된 인벤토리 로드
    public void LoadInventory()
    {
        // 저장된 데이터를 불러와서 복구하는 로직 (JSON 등으로부터 로드 가능)
        Debug.Log("인벤토리를 불러옵니다.");
    }

    // 인벤토리 저장
    public void SaveInventory()
    {
        // 현재 인벤토리 데이터를 저장하는 로직 (JSON 등으로 저장 가능)
        Debug.Log("인벤토리를 저장합니다.");
    }

    // 아이템 추가
    public bool AddItem(Item newItem)
    {
        // 중복된 아이템이 있는지 확인
        Item existingItem = items.Find(item => item.ID == newItem.ID);

        if (existingItem != null)
        {
            // 중복된 아이템이 있으면 수량을 증가시킴
            existingItem.AddAmount(newItem.Amount);
            Debug.Log($"{newItem.Name}이(가) 추가되었습니다. 수량: {existingItem.Amount}");
        }
        else
        {
            // 중복된 아이템이 없으면 리스트에 새로 추가
            items.Add(newItem);
            Debug.Log($"{newItem.Name}이(가) 새로운 아이템으로 추가되었습니다.");
        }

        return true;
    }

    // 아이템 제거
    public bool RemoveItem(Item itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            items.Remove(itemToRemove);
            Debug.Log($"{itemToRemove.Name}이(가) 인벤토리에서 제거되었습니다.");
            return true;
        }
        else
        {
            Debug.LogError("제거할 아이템을 찾을 수 없습니다.");
            return false;
        }
    }
}
