using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items = new List<Item>(); // 인벤토리에 저장될 아이템 리스트

    // 아이템을 인벤토리에 추가
    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        Debug.Log(newItem.itemName + " added to inventory.");
    }

    // 아이템을 인벤토리에서 제거
    public void RemoveItem(Item item)
    {
        if (items.Contains(item))
        {
            items.Remove(item);
            Debug.Log(item.itemName + " removed from inventory.");
        }
    }

    // 아이템 사용
    public void UseItem(string itemName)
    {
        Item item = items.Find(i => i.itemName == itemName);
        if (item != null)
        {
            item.Use(); // 각 아이템의 고유한 Use 메서드를 호출
        }
        else
        {
            Debug.Log("Item not found in inventory.");
        }
    }
}

