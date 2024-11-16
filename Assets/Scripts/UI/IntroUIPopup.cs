using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroUIPopup : UI_Popup
{
    public override void Init()
    {
        base.Init();
    }

    public void CloseIntroUI()
    {
        Managers.Popup.CloseIntro();
    }
}