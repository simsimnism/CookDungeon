using UnityEngine;

public class InventorySlotGenerate : MonoBehaviour
{
    public GameObject slotPrefab; // 슬롯 프리팹
    public Transform slotParent; // 슬롯이 배치될 부모 객체
    public int slotCount = 10; // 고정된 슬롯 개수
    public ItemSlot[] slots;

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
            slotObj.name = "Slot_" + (i + 1);
            slots[i] = slotObj.GetComponent<ItemSlot>();
        }
    }

    // 빈 슬롯을 반환하는 메서드
    public ItemSlot GetEmptySlot()
    {
        foreach (ItemSlot slot in slots)
        {
            if (slot.currentItem == null) // 아이템이 없는 빈 슬롯
                return slot;
        }
        return null;
    }
}
