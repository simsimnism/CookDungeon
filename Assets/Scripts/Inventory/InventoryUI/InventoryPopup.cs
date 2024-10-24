using System.Collections.Generic;
using UnityEngine;

public class InventoryPopup : UI_Popup
{
    public void Initialize(List<Item> playerItems)
    {
        // 인벤토리 팝업을 열 때 호출 (아이템 슬롯 초기화, 필터링 등)
        Debug.Log("인벤토리 팝업 초기화 중...");
    }

    public void CloseInventory()
    {
        // 팝업 닫기 전 상태 저장 작업 (필요할 경우)
        SaveInventoryState(); 

        // 팝업 닫기
        Managers.UI.ClosePopupUI(this);
    }

    private void SaveInventoryState()
    {
        // 인벤토리 상태를 저장하는 작업 (필요한 경우에만 호출)
        Debug.Log("인벤토리 상태를 저장했습니다.");
    }
}
