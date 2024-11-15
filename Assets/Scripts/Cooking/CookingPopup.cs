using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//요리UI의 팝업을 담당
public class CookingPopup : UI_Popup
{
    void Start()
    {
        base.Init();
    }
    public void CloseCook()
    {
        
        Managers.Popup.CloseCooking();
        Debug.Log("Closing Cooking Popup");
    }
}
