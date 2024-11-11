using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//요리 관련 매니저, 팝업 등을 담당시킬것     
public class CookingManager 
{

    private CookingPopup _cookingPopup;
    private bool _isCookingOpen = false;
    public Inventory slotGenerate;

    // 인벤토리 열고 닫기
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

}
