using System.Collections.Generic;
using UnityEngine;

public class FoodItemLoader
{
    private Dictionary<int, FoodItem> _foodItems = new Dictionary<int, FoodItem>();
    private Dictionary<string, FoodItem> _foodItemsByName = new Dictionary<string, FoodItem>();

    public void LoadFoodData(string jsonPath)
    {
        TextAsset jsonData = Managers.Resource.Load<TextAsset>(jsonPath);

        if (jsonData != null)
        {
            var foodData = JsonUtility.FromJson<FoodData>(jsonData.text);

            foreach (var item in foodData.items)
            {
                Sprite itemSprite = Managers.Resource.Load<Sprite>($"Sprites/Food/{item.name}");
                FoodItem foodItem = new FoodItem(
                    item.name,
                    item.id,
                    item.description,
                    item.MaxAmount,
                    item.itemType,
                    item.healthRecovery,
                    item.fullnessRecovery,
                    itemSprite
                );
                _foodItems.Add(item.id, foodItem);
                _foodItemsByName.Add(item.name, foodItem);
            }
        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {jsonPath}");
        }
    }

    public FoodItem GetFoodItemById(int id)
    {
        _foodItems.TryGetValue(id, out var item);
        return item;
    }

    public FoodItem GetFoodItemByPrefabName(string prefabName)
    {
        _foodItemsByName.TryGetValue(prefabName, out var item);
        return item;
    }
}

[System.Serializable]
public class FoodData
{
    public List<FoodItemData> items;
}

[System.Serializable]
public class FoodItemData
{
    public string name;
    public int id;
    public string description;
    public int MaxAmount;
    public int itemType;
    public int healthRecovery;
    public int fullnessRecovery;
}
