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
            _isInventoryOpen = true;
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
        // slotGenerate가 null이거나 초기화되지 않은 경우 인벤토리를 열어 초기화
        if (slotGenerate == null || !slotGenerate.isInitialized)
        {
            OpenInventory();
        }

        Sprite itemSprite = Resources.Load<Sprite>($"Sprites/Food/{prefabName}");
        if (itemSprite == null)
        {
            Debug.LogWarning($"아이템 스프라이트를 찾을 수 없습니다: {prefabName}");
            return;
        }

        Item item = new Item(prefabName, 101, "아이템 설명", 99, 1, itemSprite);
        ItemSlot slot = slotGenerate.GetEmptySlot();
        if (slot != null)
        {
            slot.SetItem(item); // 빈 슬롯에 아이템 설정
            Debug.Log($"아이템 {item.Name}을 슬롯에 추가하였습니다.");
        }
        else
        {
            Debug.LogWarning("빈 슬롯을 찾을 수 없습니다.");
        }
    }
}
