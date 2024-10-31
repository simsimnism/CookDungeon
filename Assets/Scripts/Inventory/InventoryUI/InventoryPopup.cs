using System.Collections.Generic;
using UnityEngine;

public class InventoryPopup : UI_Popup
{
    public bool isOpen = false;  // 팝업이 열려 있는지 여부를 저장하는 변수

    public void ToggleInventoryPopup(List<Item> playerItems)
    {
        if (!isOpen)
        {
            InventoryPopup existingPopup = Managers.UI.GetPopup<InventoryPopup>();
            if (existingPopup == null)
            {
                ShowInventory(playerItems);
                isOpen = true;
            }
            else
            {
                Debug.Log("이미 열려 있는 인벤토리 팝업을 감지했습니다.");
            }
        }
        else
        {
            CloseInventory();
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

    public void CloseInventory()
    {
        if (!isOpen)
        {
            Debug.LogWarning("인벤토리 팝업이 이미 닫혀 있습니다.");
            return;
        }

        SaveInventoryState();  // 닫기 전 상태를 저장 (필요할 경우)

        // UIManager 스택에서 팝업이 존재할 경우 닫기
        if (Managers.UI.GetPopup<InventoryPopup>() != null)
        {
            Managers.UI.ClosePopupUI(this);
        }
        else
        {
            Debug.LogWarning("UIManager 스택에 해당 팝업이 없습니다.");
        }

        // 인벤토리 팝업을 비활성화하여 UI에서 보이지 않도록 설정
        gameObject.SetActive(false);
        isOpen = false;  // 팝업 상태를 닫힌 상태로 설정
        Debug.Log("인벤토리 팝업이 닫혔습니다.");
    }




    // 인벤토리 상태를 저장하는 함수
    private void SaveInventoryState()
    {
        Debug.Log("인벤토리 상태를 저장했습니다.");
    }
}