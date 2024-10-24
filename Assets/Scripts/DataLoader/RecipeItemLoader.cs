using UnityEngine;
using System.Collections.Generic;

public class RecipeItemLoader
{
    private Dictionary<int, RecipeItem> _recipeItems = new Dictionary<int, RecipeItem>();

    public void LoadRecipeData(string jsonPath)
    {
        // Resources 폴더에서 JSON 파일을 불러옴 (확장자 제외)
        TextAsset jsonData = Resources.Load<TextAsset>(jsonPath);

        if (jsonData != null)
        {
            // JSON 데이터를 RecipeData로 변환
            var recipeData = JsonUtility.FromJson<RecipeData>(jsonData.text);

            // 변환된 데이터를 Dictionary에 저장
            foreach (var item in recipeData.items)
            {
                // 아이템 스프라이트 불러오기
                Sprite itemSprite = Resources.Load<Sprite>($"Sprites/{item.name}");

                RecipeItem recipeItem = new RecipeItem(
                    item.name,
                    item.id,
                    item.description,
                    item.amount,
                    item.amount,  // maxAmount와 initialAmount 동일하게 설정
                    item.healthRecovery,
                    item.fullnessRecovery,
                    new List<int>(item.requiredIngredients),
                    itemSprite // 스프라이트 전달
                );
                _recipeItems.Add(item.id, recipeItem);
            }
        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {jsonPath}");
        }
    }

    public RecipeItem GetRecipeItemByPrefabName(string prefabName)
    {
        if (int.TryParse(prefabName, out int id) && _recipeItems.ContainsKey(id))
        {
            return _recipeItems[id];
        }
        return null;
    }
}

// JSON 데이터를 저장할 클래스
[System.Serializable]
public class RecipeData
{
    public List<RecipeItemData> items;
}

[System.Serializable]
public class RecipeItemData
{
    public string name;
    public int id;
    public string description;
    public int amount;
    public int healthRecovery;
    public int fullnessRecovery;
    public List<int> requiredIngredients;
}
