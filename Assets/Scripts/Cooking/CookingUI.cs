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

    private CookingSlot[] slots;  // 수동 할당된 슬롯을 배열로 관리
    private RecipeItemLoader recipeLoader = new RecipeItemLoader();
    private Inventory inventory;

    private void Start()
    {
        recipeLoader.LoadRecipeData("json/Recipe");
        inventory = Managers.Inventory.slotGenerate;

        // 수동 할당된 슬롯을 배열에 추가
        slots = new CookingSlot[] { slot1, slot2, slot3 };

        cookButton.onClick.AddListener(StartCooking);
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

    // 특정 요리 슬롯의 itemType을 HashSet에 추가
    private void AddIngredientTypeFromSlot(CookingSlot slot, HashSet<int> ingredientTypes)
    {
        if (slot == null || slot.currentFoodItem == null)
        {
            Debug.LogWarning("요리 슬롯이 비어있거나 아이템이 설정되지 않았습니다.");
            return;
        }

        Debug.Log($"아이템 Type: {slot.currentFoodItem.ItemType}, 아이템 이름: {slot.currentFoodItem.Name}");
        ingredientTypes.Add(slot.currentFoodItem.ItemType);
    }

    // 조합 조건을 만족하는 RecipeItem을 찾는 메서드
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

    // 조리 과정을 실행하는 코루틴
    private System.Collections.IEnumerator CookingProcess(RecipeItem recipe)
    {
        yield return new WaitForSeconds(5f);

        string spriteName = recipe.Name;
        Sprite resultSprite = Managers.Resource.Load<Sprite>($"Sprites/Recipes/{spriteName}");

        if (resultSprite != null && resultSlot != null)
        {
            resultSlot.SetItem(recipe);
            resultSlot.itemImage.sprite = resultSprite;
            resultSlot.itemImage.enabled = true;
            Debug.Log("결과 아이템이 생성되었습니다: " + recipe.Name);
        }
        else
        {
            Debug.LogWarning($"스프라이트를 로드할 수 없습니다: Sprites/Recipes/{spriteName}");
        }

        ClearAllCookingSlots();
    }

    // 모든 요리 슬롯 초기화
    private void ClearAllCookingSlots()
    {
        foreach (var slot in slots)
        {
            slot.ClearSlot();
        }
    }

    // 요리 UI 상태를 업데이트하는 메서드
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
