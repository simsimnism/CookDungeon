using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager
{
    public InventoryPopup inventoryPopup;  // 인벤토리 팝업을 참조
    public List<Item> items = new List<Item>();  // 플레이어의 아이템 목록 관리

    // 초기화: InputManager의 KeyAction에 콜백 등록
    // 초기화 메서드
    public void Init()
    {

        // inventoryPopup이 null 상태라면 프리팹을 동적으로 로드하여 할당
        if (inventoryPopup == null)
        {
            GameObject popupPrefab = Managers.Resource.Instantiate("UI/Popup/InventoryPopup");
            inventoryPopup = popupPrefab.GetComponent<InventoryPopup>();

            if (inventoryPopup == null)
                Debug.LogError("InventoryPopup 프리팹이 올바르게 할당되지 않았습니다.");
            else
                Debug.Log("InventoryPopup 프리팹이 동적으로 할당되었습니다.");
        }

        Managers.Input.KeyAction += OnKeyPress;  // 키 입력 이벤트에 콜백 등록
        LoadInventory();  // 인벤토리 로드
    }


    // 키 입력 처리: 'I' 키를 눌렀을 때 인벤토리 팝업 토글
    private void OnKeyPress()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
    }

    // 인벤토리 팝업 상태를 토글하는 함수
    public void ToggleInventory()
    {
        if (inventoryPopup == null)
        {
            Debug.LogWarning("InventoryPopup is not assigned.");
            return;
        }

        inventoryPopup.ToggleInventoryPopup(items);  // 인벤토리 팝업 열기/닫기
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

        SaveInventory();  // 아이템 추가 시마다 저장
        return true;
    }

    // 아이템 제거
    public bool RemoveItem(Item itemToRemove)
    {
        if (items.Contains(itemToRemove))
        {
            items.Remove(itemToRemove);
            Debug.Log($"{itemToRemove.Name}이(가) 인벤토리에서 제거되었습니다.");
            SaveInventory();  // 아이템 제거 후 인벤토리 저장
            return true;
        }
        else
        {
            Debug.LogError("제거할 아이템을 찾을 수 없습니다.");
            return false;
        }
    }

    // 저장된 인벤토리 로드
    public void LoadInventory()
    {
        // 여기에 저장된 데이터를 불러오는 로직 추가 (예: JSON 파일에서 불러오기)
        Debug.Log("인벤토리를 불러옵니다.");
        // items 리스트를 불러온 데이터로 초기화
    }

    // 인벤토리 저장
    public void SaveInventory()
    {
        // 여기에 데이터를 저장하는 로직 추가 (예: JSON 파일로 저장하기)
        Debug.Log("인벤토리를 저장합니다.");
        // items 리스트를 저장할 데이터로 변환하여 저장
    }

    // 아이템 목록 반환 (기타 시스템에서 접근 가능)
    public List<Item> GetPlayerItems()
    {
        return items;
    }
}