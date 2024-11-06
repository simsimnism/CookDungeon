using UnityEngine;

public class InventoryManager 
{
    private InventoryPopup _inventoryPopup;
    private bool _isInventoryOpen = false;

    public void Update()
    {
        // E 키 입력을 감지하여 인벤토리 열고 닫기
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
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
        // 인벤토리 팝업 생성 및 표시
        _inventoryPopup = Managers.UI.ShowPopupUI<InventoryPopup>("InventoryPopup");
        _isInventoryOpen = true;
    }

    private void CloseInventory()
    {
        // 인벤토리 팝업 닫기
        if (_inventoryPopup != null)
        {
            _inventoryPopup.ClosePopupUI();
            _isInventoryOpen = false;
        }
    }
}
