using System.Collections.Generic;
using UnityEngine;

public class RecipeItemLoader
{
    private Dictionary<int, RecipeItem> _recipeItems = new Dictionary<int, RecipeItem>();
    private Dictionary<string, RecipeItem> _recipeItemsByName = new Dictionary<string, RecipeItem>();

    public void LoadRecipeData(string jsonPath)
    {
        TextAsset jsonData = Managers.Resource.Load<TextAsset>(jsonPath);

        if (jsonData != null)
        {
            var recipeData = JsonUtility.FromJson<RecipeData>(jsonData.text);

            foreach (var item in recipeData.items)
            {
                Sprite itemSprite = Managers.Resource.Load<Sprite>($"Sprites/{item.name}");
                RecipeItem recipeItem = new RecipeItem(
                    item.name,
                    item.id,
                    item.description,
                    item.MaxAmount,
                    item.itemType,
                    item.healthRecovery,
                    item.fullnessRecovery,
                    item.requiredIngredients,
                    itemSprite
                );
                _recipeItems.Add(item.id, recipeItem);
                _recipeItemsByName.Add(item.name, recipeItem);
            }
        }
        else
        {
            Debug.LogError($"파일을 찾을 수 없습니다: {jsonPath}");
        }
    }

    public RecipeItem GetRecipeItemById(int id)
    {
        _recipeItems.TryGetValue(id, out var item);
        return item;
    }

    public RecipeItem GetRecipeItemByPrefabName(string prefabName)
    {
        _recipeItemsByName.TryGetValue(prefabName, out var item);
        return item;
    }
}

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
    public int MaxAmount;
    public string itemType;
    public int healthRecovery;
    public int fullnessRecovery;
    public List<int> requiredIngredients;
}
