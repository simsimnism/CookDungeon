using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//각 팝업의 함수를 관리하는 코드
public class UIPopupManager 
{
    //스킬 유아이 관련
    private SkillPopup _SkillPopup;
    private bool _isSkillOpen = false;

    //요리 유아이 관련
    private CookingPopup _cookingPopup;
    private bool _isCookingOpen = false;


    //============================스킬 UI_Popup==============================
    public void ToggleSkillUI()
    {
        if (_isSkillOpen)
        {
            CloseSkillUI();
        }
        else
        {
            OpenSkillUI();
        }
    }

    private void OpenSkillUI()
    {
        if (_SkillPopup == null)
        {
            _SkillPopup = Managers.UI.ShowPopupUI<SkillPopup>("SkillChoicePopup");
            Time.timeScale = 0;  // 게임 시간 멈춤
        }
        else
        {
            _SkillPopup.gameObject.SetActive(true);
        }

        if (_SkillPopup != null)
        {
            _isSkillOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    public void CloseSkillUI()
    {
        if (_SkillPopup != null)
        {
            _SkillPopup.gameObject.SetActive(false);
            _isSkillOpen = false;
            Time.timeScale = 1;  // 게임 시간 재개
        }
    }
    //========================================================================

    //============================요리 UI_Popup==============================
    public void ToggleCooking()
    {
        if (_isCookingOpen)
        {
            CloseCooking();
        }
        else
        {
            OpenCooking();
        }
    }

    //요리UI를 여는 로직
    private void OpenCooking()
    {
        if (_cookingPopup == null)
        {
            _cookingPopup = Managers.UI.ShowPopupUI<CookingPopup>("CookingPopup");
        }
        else
        {
            _cookingPopup.gameObject.SetActive(true);
        }

        if (_cookingPopup != null)
        {
            _isCookingOpen = true;
        }
        else
        {
            Debug.LogError("InventoryPopup을 생성하지 못했습니다.");
        }
    }

    //요리UI를 닫는 로직
    public void CloseCooking()
    {
        if (_cookingPopup != null)
        {
            _cookingPopup.gameObject.SetActive(false);
            _isCookingOpen = false;
        }
    }
    //========================================================================


    //==========================인벤토리 UI_Popup=============================

    //==========================================================================
}
