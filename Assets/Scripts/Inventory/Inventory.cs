using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    private List<Item> items; // 인벤토리에 저장된 아이템 리스트
    private DataManager _dataManager; // DataManager를 통해 FoodItemLoader 가져오기
    public GameObject inventorySlotPrefab; // 인벤토리 슬롯 UI 프리팹
    private object inventoryUIPanel;
    private InventoryPopup popup;

    void Awake()
    {
        popup = GetComponent<InventoryPopup>();
    }

    void Start()
    {
        // items 리스트 초기화
        items = new List<Item>();

        // DataManager를 통해 FoodItemLoader 가져오기
        _dataManager = Managers.Data;  // DataManager 인스턴스 찾기

        if (_dataManager == null)
        {
            Debug.LogError("DataManager를 찾을 수 없습니다.");
            return;
        }

        // Init 호출 제거 - 이미 초기화되었을 것이므로 불필요한 호출을 방지
        // _dataManager.Init(); 
    }

    void Update()
    {
        Managers.Input.KeyAction -= OpenInventory;
        Managers.Input.KeyAction += OpenInventory;
    }

    void OpenInventory()
    {
        // I 키 입력을 감지
        if (Input.GetKeyDown(KeyCode.I))
        {
            // 인벤토리 팝업을 토글
            popup.ToggleInventoryPopup(Managers.Inventory.items);
        }
    }

    // 게임 중 아이템을 습득하는 함수 (프리팹 이름이 ID와 같을 때)
    public bool AddItemByPrefabID(string prefabName)
    {
        // DataManager를 통해 FoodItem을 가져옴
        FoodItem foodItem = _dataManager?.GetFoodItemByPrefabName(prefabName);

        if (foodItem != null)
        {
            return AddItem(foodItem); // 아이템 상속 구조에서 적절한 타입 사용
        }
        else
        {
            Debug.LogError($"프리팹 이름에 해당하는 아이템을 찾을 수 없습니다: {prefabName}");
            return false;
        }
    }

    // 아이템 추가 함수
    public bool AddItem(Item newItem)
    {
        // 같은 아이템이 이미 있는지 확인
        Item existingItem = items.Find(item => item.ID == newItem.ID);

        if (existingItem != null)
        {
            // 수량 추가
            if (existingItem.Amount < existingItem.MaxAmount)
            {
                existingItem.AddAmount(newItem.Amount);
                Debug.Log($"{newItem.Name}이(가) 추가되었습니다. 수량: {existingItem.Amount}");
                return true;
            }
            else
            {
                Debug.Log($"{newItem.Name}이(가) 최대 수량에 도달했습니다.");
                return false;
            }
        }
        else
        {
            // 새로운 아이템 추가
            if (items.Count < 20) // 인벤토리 최대 크기
            {
                items.Add(newItem);
                Debug.Log($"{newItem.Name}이(가) 새로운 아이템으로 추가되었습니다.");
                return true;
            }
            else
            {
                Debug.Log("인벤토리가 가득 찼습니다.");
                return false;
            }
        }
    }

    // 인벤토리에 있는 모든 아이템 출력
    public void DisplayInventory()
    {
        foreach (var item in items)
        {
            Debug.Log($"아이템: {item.Name}, 수량: {item.Amount}/{item.MaxAmount}");
        }
    }
}
