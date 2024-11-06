using UnityEngine;
using System.Collections.Generic;

public class InventorySlotHandler : MonoBehaviour
{
    public GameObject slotPrefab; // 슬롯 프리팹
    public Transform slotContainer; // 슬롯들이 추가될 부모 컨테이너
    private List<GameObject> slots = new List<GameObject>(); // 슬롯 리스트

    private void Start()
    {
        GenerateFixedSlots(10); // 고정된 10개의 슬롯 생성
    }

    public void GenerateFixedSlots(int slotCount)
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject newSlot = Instantiate(slotPrefab, slotContainer); // 슬롯 생성
            slots.Add(newSlot); // 리스트에 추가

            // 빈 슬롯으로 설정
            ItemSlot itemSlot = newSlot.GetComponent<ItemSlot>();
            if (itemSlot != null)
            {
                itemSlot.ClearItem(); // 아이템이 없으므로 빈 슬롯으로 초기화
            }
        }
    }

    public void UpdateSlot(GameObject slot, Item item)
    {
        ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
        if (itemSlot != null)
        {
            if (item != null)
            {
                itemSlot.SetItem(item); // 아이템이 있으면 아이템 설정
            }
            else
            {
                itemSlot.ClearItem(); // 아이템이 없으면 빈 슬롯으로 설정
            }
        }
    }

    public void UpdateInventory(List<Item> items)
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < items.Count)
            {
                UpdateSlot(slots[i], items[i]);
            }
            else
            {
                UpdateSlot(slots[i], null); // 남은 슬롯을 빈 상태로 유지
            }
        }
    }
}
