using System.Collections.Generic;
using UnityEngine;

public class FoodItemLoader
{
    private Dictionary<int, FoodItem> _foodItems = new Dictionary<int, FoodItem>();

    public void LoadFoodData(string jsonPath)
    {
        // Resources 폴더에서 JSON 파일을 불러옴
        TextAsset jsonData = Resources.Load<TextAsset>(jsonPath);

        if (jsonData != null)
        {
            // JSON 데이터를 FoodData로 변환
            var foodData = JsonUtility.FromJson<FoodData>(jsonData.text);

            // 변환된 데이터를 Dictionary에 저장
            foreach (var item in foodData.items)
            {
                // FoodItem 객체 생성
                FoodItem foodItem = new FoodItem(
                    item.name,
                    item.id,
                    item.description,
                    item.amount,
                    item.amount,  // maxAmount와 initialAmount 동일하게 설정
                    item.healthRecovery,
                    item.fullnessRecovery
                );
                _foodItems.Add(item.id, foodItem);
            }
        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {jsonPath}");
        }
    }

    // 프리팹 이름으로 FoodItem 반환
    public FoodItem GetFoodItemByPrefabName(string prefabName)
    {
        if (int.TryParse(prefabName, out int id) && _foodItems.ContainsKey(id))
        {
            return _foodItems[id];
        }
        return null;
    }
}

// JSON 데이터를 저장할 클래스
[System.Serializable]
public class FoodData
{
    public List<FoodItemData> items;
}

// FoodItemData 클래스도 정의 필요
[System.Serializable]
public class FoodItemData
{
    public string name;
    public int id;
    public string description;
    public int amount;
    public int healthRecovery;
    public int fullnessRecovery;
}
