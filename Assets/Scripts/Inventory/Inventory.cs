using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private List<Item> items; // 인벤토리에 저장된 아이템 리스트
    public FoodItemLoader foodItemLoader; // FoodItemLoader를 통해 아이템 데이터 로드 (상속 구조 활용)
    public GameObject inventorySlotPrefab; // 인벤토리 슬롯 UI 프리팹
    private object inventoryUIPanel;

    void Start()
    {
        // items 리스트 초기화
        items = new List<Item>();

        // JSON 데이터로부터 FoodItem 로드
        if (foodItemLoader != null)
        {
            foodItemLoader.LoadFoodData("Resorces/json/Food");  // JSON 파일 경로 설정
        }
        else
        {
            Debug.LogError("FoodItemLoader가 할당되지 않았습니다.");
        }
    }

    // 게임 중 아이템을 습득하는 함수 (프리팹 이름이 ID와 같을 때)
    public bool AddItemByPrefabID(string prefabName)
    {
        // 프리팹 이름을 ID로 변환하고 FoodItem을 로드
        FoodItem foodItem = foodItemLoader?.GetFoodItemByPrefabName(prefabName);

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
