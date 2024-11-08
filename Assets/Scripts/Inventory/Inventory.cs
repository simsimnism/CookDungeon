using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    public int slotCount = 12;
    private ItemSlot[] slots;
    public bool isInitialized = false;

    private void OnEnable()
    {
        if (slotPrefab == null || slotParent == null)
        {
            Debug.LogError("슬롯 프리팹 또는 부모 객체가 설정되지 않았습니다.");
            return;
        }

        if (!isInitialized)
        {
            GenerateSlots();
            isInitialized = true;
            Debug.Log("슬롯 초기화 완료");
        }
    }

    private void GenerateSlots()
    {
        slots = new ItemSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            if (slotObj == null)
            {
                Debug.LogError($"슬롯 프리팹이 생성되지 않았습니다: 인덱스 {i}");
                continue;
            }

            // ItemSlot 컴포넌트가 없다면 추가
            ItemSlot itemSlot = slotObj.GetComponent<ItemSlot>();
            if (itemSlot == null)
            {
                itemSlot = slotObj.AddComponent<ItemSlot>();
                Debug.LogWarning($"ItemSlot 컴포넌트가 프리팹에 없어서 자동으로 추가되었습니다: 인덱스 {i}");
            }

            slots[i] = itemSlot;
            if (slots[i] == null)
            {
                Debug.LogError($"슬롯 오브젝트에 ItemSlot 컴포넌트를 할당할 수 없습니다: 인덱스 {i}");
            }
            else
            {
                slots[i].parentInventory = this;
                Debug.Log($"슬롯 {i} 초기화 완료, currentItem: {slots[i].currentItem?.Name ?? "null"}");
            }
        }
    }

    public ItemSlot GetEmptySlot()
    {
        if (slots == null)
        {
            Debug.LogError("슬롯 배열이 초기화되지 않았습니다.");
            return null;
        }

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null && slots[i].currentItem == null) // currentItem을 직접 검사
            {
                Debug.Log($"빈 슬롯 찾음: 인덱스 {i}");
                return slots[i];
            }
            else
            {
                Debug.Log($"슬롯 {i}는 비어있지 않습니다. currentItem: {slots[i].currentItem?.Name ?? "null"}");
            }
        }

        Debug.LogWarning("빈 슬롯을 찾을 수 없습니다. 모든 슬롯이 채워져 있습니다.");
        return null;
    }
}
