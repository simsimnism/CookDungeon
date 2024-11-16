using UnityEngine;

public class InventoryManager
{
    private InventoryPopup _inventoryPopup;
    private bool _isInventoryOpen = false;
    public Inventory slotGenerate;

    public bool IsInventoryOpen => _isInventoryOpen;

    // 인벤토리 열고 닫기
    public void ToggleInventory()
    {
        if (_isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    public void OpenInventory()
    {
        if (_inventoryPopup == null)
        {
            _inventoryPopup = Managers.UI.ShowPopupUI<InventoryPopup>();
            _inventoryPopup.gameObject.SetActive(false);
        }
        else
        {
            _inventoryPopup.gameObject.SetActive(true);
        }

        if (_inventoryPopup != null)
        {
            slotGenerate = _inventoryPopup.GetComponentInChildren<Inventory>();
            if (slotGenerate != null && !slotGenerate.isInitialized)
            {
                slotGenerate.InitializeSlots();
                LoadInventoryState();
            }

            _isInventoryOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    public void CloseInventory()
    {
        if (_inventoryPopup != null)
        {
            SaveInventoryState();
            _inventoryPopup.gameObject.SetActive(false);
            _isInventoryOpen = false;
        }
    }

    private void SaveInventoryState()
    {
        Debug.Log("인벤토리 상태 저장");
    }

    private void LoadInventoryState()
    {
        Debug.Log("인벤토리 상태 불러오기");
    }

    // 드롭된 아이템 추가 기능
    public void AddItemToInventory(string prefabName)
    {
        if (slotGenerate == null || !slotGenerate.isInitialized)
        {
            OpenInventory();
        }

        Sprite itemSprite = slotGenerate.LoadSpriteWithCaching($"Sprites/Food/{prefabName}");
        if (itemSprite == null)
        {
            Debug.LogWarning($"아이템 스프라이트를 찾을 수 없습니다: {prefabName}");
            return;
        }

        Item item = Managers.Data.GetFoodItemByPrefabName(prefabName) ?? (Item)Managers.Data.GetRecipeItemByPrefabName(prefabName);
        if (item == null)
        {
            Debug.LogWarning($"아이템 '{prefabName}'을(를) 찾을 수 없습니다.");
            return;
        }

        item.ItemSprite = itemSprite;

        if (slotGenerate.AddItemToInventory(item))
        {
            Debug.Log($"아이템 {item.Name}을 인벤토리에 추가하였습니다.");
        }
        else
        {
            Debug.LogWarning("빈 슬롯을 찾을 수 없거나 인벤토리가 가득 찼습니다.");
        }
    }

    // 새로 추가된 메서드: AddRecipeResultToInventory
    public void AddRecipeResultToSlot(RecipeItem recipe, BaseItemSlot targetSlot)
    {
        if (targetSlot == null)
        {
            Debug.LogWarning("타겟 슬롯이 설정되지 않았습니다.");
            return;
        }

        // 스프라이트 로드 및 설정
        string spriteName = recipe.Name;
        Sprite resultSprite = Managers.Resource.Load<Sprite>($"Sprites/Recipes/{spriteName}");
        if (resultSprite == null)
        {
            Debug.LogWarning($"결과 아이템 스프라이트를 로드할 수 없습니다: Sprites/Recipes/{spriteName}");
            return;
        }

        recipe.ItemSprite = resultSprite;

        // 타겟 슬롯에 아이템 설정
        targetSlot.SetItem(recipe);
        targetSlot.itemImage.sprite = resultSprite;
        targetSlot.itemImage.enabled = true;
        targetSlot.SetImageAlpha(1f);  // 스프라이트가 보이도록 알파값 설정

        Debug.Log($"요리 결과 아이템 {recipe.Name}이 {targetSlot.name}에 생성되었습니다.");
    }

}
