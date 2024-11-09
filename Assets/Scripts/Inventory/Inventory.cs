using System;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    public int slotCount = 12;
    private ItemSlot[] slots;
    public bool isInitialized = false;

    private void Start() // Start에서 초기화를 진행하여 모든 게임 오브젝트가 준비된 후 슬롯을 설정
    {
        if (!isInitialized)
        {
            InitializeSlots();
        }
    }

    public void InitializeSlots()
    {
        if (slotPrefab == null || slotParent == null)
        {
            Debug.LogError("슬롯 프리팹 또는 부모 객체가 설정되지 않았습니다.");
            return;
        }

        slots = new ItemSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            if (slotObj == null)
            {
                Debug.LogError($"슬롯 프리팹이 생성되지 않았습니다: 인덱스 {i}");
                continue;
            }

            ItemSlot itemSlot = slotObj.GetComponent<ItemSlot>();
            if (itemSlot == null)
            {
                itemSlot = slotObj.AddComponent<ItemSlot>();
                Debug.LogWarning($"ItemSlot 컴포넌트가 프리팹에 없어서 자동으로 추가되었습니다: 인덱스 {i}");
            }

            itemSlot.currentItem = null; // 명확하게 null로 초기화
            slots[i] = itemSlot;
            slots[i].parentInventory = this;

            Debug.Log($"슬롯 {i} 초기화 완료, currentItem: {slots[i].currentItem?.Name ?? "null"}");
        }

        isInitialized = true;
        Debug.Log("모든 슬롯 초기화 완료.");
    }

    public ItemSlot GetEmptySlot()
    {
        if (slots == null)
        {
            Debug.LogError("슬롯 배열이 초기화되지 않았습니다.");
            return null;
        }

        // 빈 슬롯 탐색 시 디버그 로그 추가
        for (int i = 0; i < slots.Length; i++)
        {
            Debug.Log($"슬롯 {i} 상태 확인, currentItem: {slots[i].currentItem?.Name ?? "null"}");

            if (slots[i] != null && slots[i].IsEmpty())
            {
                Debug.Log($"빈 슬롯 찾음: 인덱스 {i}");
                return slots[i];
            }
        }

        Debug.LogWarning("빈 슬롯을 찾을 수 없습니다. 모든 슬롯이 채워져 있습니다.");
        return null;
    }
}
