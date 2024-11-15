using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIPopup : UI_Popup
{
    public override void Init()
    {
        Managers.Popup.CloseIntro();
        base.Init();
    }

    public override void ClosePopupUI()
    {
        base.ClosePopupUI();
    }

}