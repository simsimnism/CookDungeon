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
        _inventoryPopup = Managers.UI.ShowPopupUI<InventoryPopup>("InventoryPopup");
        _isInventoryOpen = true;

        if (_inventoryPopup != null)
        {
            slotGenerate = _inventoryPopup.GetComponentInChildren<Inventory>();
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
        if (slotGenerate == null)
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
            slot.SetItem(item);
        }
        else
        {
            Debug.LogWarning("빈 슬롯을 찾을 수 없습니다.");
        }
    }
}
