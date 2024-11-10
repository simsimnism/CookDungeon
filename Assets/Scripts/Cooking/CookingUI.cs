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

    private RecipeItemLoader recipeLoader = new RecipeItemLoader();
    private Inventory inventory;

    private void Start()
    {
        recipeLoader.LoadRecipeData("json/Recipe");  // Recipe 데이터를 로드
        inventory = Managers.Inventory.slotGenerate;

        cookButton.onClick.AddListener(StartCooking);
    }

    public void StartCooking()
    {
        Debug.Log("StartCooking 메서드가 호출되었습니다.");  // StartCooking 호출 여부 확인

        var ingredientIds = new HashSet<int>();

        // 각 요리 슬롯에 있는 아이템의 ID를 HashSet에 추가
        AddIngredientIdFromSlot(slot1, ingredientIds);
        AddIngredientIdFromSlot(slot2, ingredientIds);
        AddIngredientIdFromSlot(slot3, ingredientIds);

        // 조건을 만족하는 RecipeItem을 찾음
        RecipeItem recipe = FindMatchingRecipe(ingredientIds);
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

    private void AddIngredientIdFromSlot(CookingSlot slot, HashSet<int> ingredientIds)
    {
        Debug.Log($"AddIngredientIdFromSlot 호출 - 슬롯: {slot}");  // 각 슬롯 호출 확인

        if (slot == null || slot.currentFoodItem == null)
        {
            Debug.LogWarning("요리 슬롯이 비어있거나 아이템이 설정되지 않았습니다.");
            return;  // 슬롯이 비어 있으면 종료
        }

        // 디버그 로그로 ID와 스프라이트 확인
        Debug.Log($"아이템 ID: {slot.currentFoodItem.ID}, 스프라이트: {slot.currentFoodItem.ItemSprite}");

        ingredientIds.Add(slot.currentFoodItem.ID);
        LoadAndSetSprite(slot);
    }

    private void LoadAndSetSprite(CookingSlot slot)
    {
        if (slot.currentFoodItem != null && slot.currentFoodItem.ItemSprite == null)
        {
            // 아이템 스프라이트 로드 및 캐싱
            string spritePath = $"Sprites/Food/{slot.currentFoodItem.Name}";
            slot.currentFoodItem.ItemSprite = Resources.Load<Sprite>(spritePath);

            if (slot.currentFoodItem.ItemSprite != null)
            {
                slot.itemImage.sprite = slot.currentFoodItem.ItemSprite;  // 슬롯에 스프라이트 설정
                slot.itemImage.enabled = true;
                var color = slot.itemImage.color;
                color.a = 1f;
                slot.itemImage.color = color;
            }
            else
            {
                Debug.LogWarning($"스프라이트를 찾을 수 없습니다: {spritePath}");
            }
        }
    }

    private RecipeItem FindMatchingRecipe(HashSet<int> ingredientIds)
    {
        foreach (var recipe in recipeLoader.GetAllRecipes())
        {
            if (new HashSet<int>(recipe.RequiredIngredients).SetEquals(ingredientIds))
            {
                return recipe;
            }
        }
        return null;
    }

    private System.Collections.IEnumerator CookingProcess(RecipeItem recipe)
    {
        yield return new WaitForSeconds(5f); // 요리 시간 5초

        if (resultSlot != null)
        {
            resultSlot.SetItem(recipe);  // 결과 슬롯에 조합된 아이템 설정
            Debug.Log("결과 아이템이 생성되었습니다: " + recipe.Name);
        }

        // 모든 요리 슬롯을 비우기
        slot1.ClearSlot();
        slot2.ClearSlot();
        slot3.ClearSlot();
    }

    public void UpdateCookingState()
    {
        var ingredientIds = new HashSet<int>();

        // 각 요리 슬롯에 있는 아이템의 ID를 HashSet에 추가
        AddIngredientIdFromSlot(slot1, ingredientIds);
        AddIngredientIdFromSlot(slot2, ingredientIds);
        AddIngredientIdFromSlot(slot3, ingredientIds);

        // 조건을 만족하는 RecipeItem을 찾음
        RecipeItem recipe = FindMatchingRecipe(ingredientIds);
        if (recipe != null)
        {
            Debug.Log("조합 가능한 레시피 발견: " + recipe.Name);
            // 조합 가능 상태를 UI에 표시하는 로직 추가 가능
        }
        else
        {
            Debug.Log("현재 조합할 수 없는 재료 조합입니다.");
        }
    }
}
