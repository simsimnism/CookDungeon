using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookingUI : MonoBehaviour
{
    public CookingSlot slot1;  // 첫 번째 요리 슬롯
    public CookingSlot slot2;  // 두 번째 요리 슬롯
    public CookingSlot slot3;  // 세 번째 요리 슬롯
    public BaseItemSlot resultSlot;  // 결과 슬롯
    public Button cookButton;

    private CookingSlot[] slots;
    private RecipeItemLoader recipeLoader = new RecipeItemLoader();
    private Inventory inventory;
    private Cauldron Cauldron;

    private void Awake()
    {
        Cauldron = GetComponent<Cauldron>();
    }

    private void Start()
    {
        recipeLoader.LoadRecipeData("json/Recipe");
        inventory = Managers.Inventory.slotGenerate;

        slots = new CookingSlot[] { slot1, slot2, slot3 };

        cookButton.onClick.AddListener(StartCooking);

    }

    void Update()
    {
        cookButton.interactable = Managers.GM.CookAbleTime;
    }

    // 요리 시작 메서드
    public void StartCooking()
    {
        Debug.Log("StartCooking 메서드가 호출되었습니다.");

        var ingredientTypes = new HashSet<int>();
        foreach (var slot in slots)
        {
            AddIngredientTypeFromSlot(slot, ingredientTypes);
        }

        RecipeItem recipe = FindMatchingRecipe(ingredientTypes);
        if (recipe != null)
        {
            Debug.Log("조합 성공: " + recipe.Name);
            StartCoroutine(CookingProcess(recipe));
        }
        else
        {
            Debug.Log("조합할 수 없는 재료입니다.");
        }
    }

    private void AddIngredientTypeFromSlot(CookingSlot slot, HashSet<int> ingredientTypes)
    {
        if (slot == null || slot.CurrentItem == null)
        {
            Debug.LogWarning("요리 슬롯이 비어있거나 아이템이 설정되지 않았습니다.");
            return;
        }

        if (slot.CurrentItem is FoodItem foodItem)
        {
            Debug.Log($"아이템 Type: {foodItem.ItemType}, 아이템 이름: {foodItem.Name}");
            ingredientTypes.Add(foodItem.ItemType);
        }
    }

    private RecipeItem FindMatchingRecipe(HashSet<int> ingredientTypes)
    {
        foreach (var recipe in recipeLoader.GetAllRecipes())
        {
            HashSet<int> requiredIngredientsSet = new HashSet<int>(recipe.RequiredIngredients);
            if (requiredIngredientsSet.SetEquals(ingredientTypes))
            {
                return recipe;
            }
        }
        return null;
    }

    private System.Collections.IEnumerator CookingProcess(RecipeItem recipe)
    {
        yield return new WaitForSeconds(5f);

        if (Managers.Inventory != null)
        {
            Managers.Inventory.AddRecipeResultToSlot(recipe, resultSlot);  // 결과 아이템을 resultSlot에 추가
        }
        else
        {
            Debug.LogWarning("InventoryManager가 설정되지 않았습니다.");
        }

        ClearAllCookingSlots();
    }



    private void ClearAllCookingSlots()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
    }

    public void UpdateCookingState()
    {
        var ingredientTypes = new HashSet<int>();
        foreach (var slot in slots)
        {
            AddIngredientTypeFromSlot(slot, ingredientTypes);
        }

        RecipeItem recipe = FindMatchingRecipe(ingredientTypes);
        if (recipe != null)
        {
            Debug.Log("조합 가능한 레시피 발견: " + recipe.Name);
        }
        else
        {
            Debug.Log("현재 조합할 수 없는 재료 조합입니다.");
        }
    }
}
