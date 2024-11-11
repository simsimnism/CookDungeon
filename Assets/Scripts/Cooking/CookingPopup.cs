using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//요리UI의 팝업을 담당
public class CookingPopup : UI_Popup
{

    void Start()
    {
        base.Init();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)) 
        {
            Managers.Cooking.CloseCooking();
        }
    }
}
