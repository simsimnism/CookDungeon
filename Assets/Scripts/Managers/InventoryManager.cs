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
        // 인벤토리 팝업이 이미 생성되었는지 확인하고 활성화
        if (_inventoryPopup == null)
        {
            _inventoryPopup = Managers.UI.ShowPopupUI<InventoryPopup>("InventoryPopup");
        }
        else
        {
            _inventoryPopup.gameObject.SetActive(true); // 기존 팝업 재활용
        }

        if (_inventoryPopup != null)
        {
            slotGenerate = _inventoryPopup.GetComponentInChildren<Inventory>();
            if (slotGenerate != null && !slotGenerate.isInitialized)
            {
                slotGenerate.InitializeSlots();
                LoadInventoryState(); // 이전 상태 불러오기
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
            SaveInventoryState(); // 인벤토리 상태 저장
            _inventoryPopup.gameObject.SetActive(false); // 파괴하지 않고 비활성화
            _isInventoryOpen = false;
        }
    }

    // 인벤토리 상태 저장
    private void SaveInventoryState()
    {
        Debug.Log("인벤토리 상태 저장");
        // 실제로 아이템 상태를 저장하는 코드 추가 (PlayerPrefs, JSON 등)
    }

    // 인벤토리 상태 불러오기
    private void LoadInventoryState()
    {
        Debug.Log("인벤토리 상태 불러오기");
        // 저장된 인벤토리 상태를 불러오는 코드 추가
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
