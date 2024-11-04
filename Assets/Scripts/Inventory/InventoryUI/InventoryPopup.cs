using UnityEngine;

public class InventoryPopup : UI_Popup
{
    public override void Init()
    {
        base.Init();
        // 추가적인 초기화 작업이 필요한 경우 여기에 작성
    }

    public void OpenInventory()
    {
        // 인벤토리 팝업을 열기
        Managers.UI.ShowPopupUI<InventoryPopup>();
    }

    public void CloseInventory()
    {
        ClosePopupUI();
    }

    public void ToggleInventory()
    {
        if (Managers.UI.IsPopupOpen<InventoryPopup>())
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }
}
