using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupController : MonoBehaviour
{
    private void Update()
    {
        Managers.Input.KeyAction -= OnEscKey;
        Managers.Input.KeyAction += OnEscKey;
    }

    private void OnDestroy()
    {
        // Managers.Input과 관련된 이벤트 해제 예시
        Managers.Input.KeyAction -= OnEscKey; ;
    }

    void OnEscKey()
    {
        // ESC 키 입력 확인
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Managers.UI != null)
            {
                UIPopupManager popupManager = Managers.Popup;

                // 특정 팝업은 ESC 키로 닫지 않음
                if (popupManager.IsGamePauseOpen || popupManager.IsGameClearOpen ||
                    popupManager.IsGameLoadingOpen || popupManager.IsGameStartOpen ||
                    popupManager.IsGameEndOpen || popupManager.IsIntroOpen ||
                    popupManager.IsSkillOpen || popupManager.IsRoundInfoOpen)
                {
                    // ESC 키로 닫지 않도록 동작 종료
                    return;
                }
                // 우선순위에 따라 팝업 닫기
                if (Managers.Inventory.IsInventoryOpen) // 인벤토리 팝업 확인
                {
                    Managers.Inventory.CloseInventory();
                }
                else if (popupManager.IsCookingOpen) // 요리 팝업 확인
                {
                    popupManager.CloseCooking();
                }
                else if (popupManager.IsRecipePopupOpen)
                {
                    popupManager.CloseRecipe();
                }
                else
                {
                    // 팝업 스택의 마지막 팝업 닫기
                    Managers.UI.ClosePopupUI();
                }
            }
        }
    }
}
