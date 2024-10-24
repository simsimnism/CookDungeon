using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    public List<ItemSlot> inventorySlots = new(); // 고정된 슬롯 리스트

    // 인벤토리 UI를 업데이트하는 함수 (아이템 추가 및 슬롯 업데이트)
    public void UpdateInventoryUI(List<Item> items)
    {
        for (int i = 0; i < inventorySlots.Count; i++)
        {
            if (i < items.Count)
            {
                inventorySlots[i].SetItem(items[i]); // 아이템 데이터를 슬롯에 설정
            }
            else
            {
                inventorySlots[i].ClearSlot(); // 아이템이 없을 경우 빈 슬롯으로 남겨둠
            }
        }
    }
}
