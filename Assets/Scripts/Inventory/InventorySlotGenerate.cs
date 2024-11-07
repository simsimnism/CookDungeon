using UnityEngine;

public class InventorySlotGenerate : MonoBehaviour
{
    public GameObject slotPrefab;  // 슬롯 프리팹
    public Transform slotParent;   // 슬롯이 배치될 부모 객체
    public int slotCount = 10;     // 고정된 슬롯 개수

    void Start()
    {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            Debug.Log("Creating Slot " + (i + 1));  // 디버그 메시지 추가
            GameObject slot = Instantiate(slotPrefab, slotParent);
            slot.name = "Slot_" + (i + 1);
        }
    }

}
