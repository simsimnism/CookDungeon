using UnityEngine;

public class DataManager
{
    private MonsterDataLoader _monsterDataLoader = new MonsterDataLoader();
    private FoodItemLoader _foodItemLoader = new FoodItemLoader();
    private RecipeItemLoader _recipeItemLoader = new RecipeItemLoader();

    public void Init()
    {
        // 몬스터 데이터 로드
        _monsterDataLoader.LoadMonsterData("json/Monsters");

        // 날것의 음식 데이터를 Resources 폴더에서 로드
        _foodItemLoader.LoadFoodData("json/Food");

        // 제작(조합된) 음식 데이터를 Resources 폴더에서 로드
        _recipeItemLoader.LoadRecipeData("json/Recipe");
    }

    // 스크립터블 오브젝트의 이름으로 몬스터 데이터를 가져오는 메서드
    public MonsterDataSO GetMonsterDataByName(string name)
    {
        return _monsterDataLoader.GetMonsterDataByName(name);
    }

    // ID로 몬스터 데이터를 가져오는 메서드
    public MonsterDataSO GetMonsterDataById(int id)
    {
        return _monsterDataLoader.GetMonsterDataById(id);
    }

    // 프리팹 이름으로 음식 아이템을 가져오는 메서드
    public FoodItem GetFoodItemByPrefabName(string prefabName)
    {
        return _foodItemLoader.GetFoodItemByPrefabName(prefabName);
    }

    // 프리팹 이름으로 조합된 음식(Recipe) 아이템을 가져오는 메서드
    public RecipeItem GetRecipeItemByPrefabName(string prefabName)
    {
        return _recipeItemLoader.GetRecipeItemByPrefabName(prefabName);
    }
}
