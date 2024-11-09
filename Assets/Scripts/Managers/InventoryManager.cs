using UnityEngine;

public class InventoryManager
{
    private InventoryPopup _inventoryPopup;
    private bool _isInventoryOpen = false;
    public Inventory slotGenerate;

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

    private void OpenInventory()
    {
        if (_inventoryPopup == null)
        {
            _inventoryPopup = Managers.UI.ShowPopupUI<InventoryPopup>("InventoryPopup");
        }

        if (_inventoryPopup != null)
        {
            slotGenerate = _inventoryPopup.GetComponentInChildren<Inventory>();
            if (slotGenerate != null && !slotGenerate.isInitialized)
            {
                slotGenerate.InitializeSlots();
            }

            _isInventoryOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    private void CloseInventory()
    {
        if (_inventoryPopup != null)
        {
            _inventoryPopup.ClosePopupUI();
            _isInventoryOpen = false;
            slotGenerate = null;
        }
    }

    public void AddItemToInventory(string prefabName)
    {
        if (slotGenerate == null || !slotGenerate.isInitialized)
        {
            OpenInventory();
        }

        Sprite itemSprite = Managers.Resource.Load<Sprite>($"Sprites/Food/{prefabName}");
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

        if (slotGenerate.AddItemToInventoryByName(prefabName))
        {
            Debug.Log($"아이템 {item.Name}을 인벤토리에 추가하였습니다.");
        }
        else
        {
            Debug.LogWarning("빈 슬롯을 찾을 수 없거나 인벤토리가 가득 찼습니다.");
        }
    }
}
