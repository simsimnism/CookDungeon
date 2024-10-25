using System.Collections.Generic;
using UnityEngine;

public class InventoryPopup : UI_Popup
{
    public bool isOpen = false;  // 팝업이 열려 있는지 여부를 저장하는 변수

    // 인벤토리 팝업을 열거나 닫는 함수
    public void ToggleInventoryPopup(List<Item> playerItems)
    {
        if (!isOpen)  // 팝업이 닫혀 있을 때
        {
            ShowInventory(playerItems);  // 인벤토리를 초기화하면서 팝업을 염
            isOpen = true;
        }
        else  // 팝업이 열려 있을 때
        {
            CloseInventory();  // 팝업을 닫음
            isOpen = false;
        }
    }

    // 인벤토리를 표시하는 함수
    private void ShowInventory(List<Item> playerItems)
    {
        Managers.UI.ShowPopupUI<InventoryPopup>("InventoryPopup");  // 인벤토리 팝업을 UIManager를 통해 열음
        Initialize(playerItems);  // 인벤토리를 초기화
    }

    // 인벤토리를 초기화하는 함수
    public void Initialize(List<Item> playerItems)
    {
        Debug.Log("인벤토리 팝업 초기화 중...");
        // 아이템 목록을 슬롯에 반영하는 로직이 추가될 수 있음
    }

    // 인벤토리를 닫는 함수
    public void CloseInventory()
    {
        SaveInventoryState();  // 닫기 전 상태를 저장 (필요할 경우)
        Managers.UI.ClosePopupUI(this);  // UIManager를 통해 팝업을 닫음
    }

    // 인벤토리 상태를 저장하는 함수
    private void SaveInventoryState()
    {
        Debug.Log("인벤토리 상태를 저장했습니다.");
    }
}