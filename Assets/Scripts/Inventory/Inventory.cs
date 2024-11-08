using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    public int slotCount = 12;
    private ItemSlot[] slots;
    private Queue<int> emptySlotIndices = new Queue<int>();

    private void Start()
    {
        GenerateSlots();
    }

    private void GenerateSlots()
    {
        slots = new ItemSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            slots[i] = slotObj.GetComponent<ItemSlot>();
            emptySlotIndices.Enqueue(i);
        }
    }

    public ItemSlot GetEmptySlot()
    {
        if (emptySlotIndices.Count > 0)
        {
            int index = emptySlotIndices.Dequeue();
            return slots[index];
        }
        Debug.LogWarning("빈 슬롯을 찾을 수 없습니다.");
        return null;
    }
    public void AddEmptySlotIndex(ItemSlot slot)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == slot)
            {
                emptySlotIndices.Enqueue(i);
                break;
            }
        }
    }
}
