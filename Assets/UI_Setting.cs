using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Setting : UI_Popup
{
    enum Images
    {
        TestImage, NewButton
    }

    enum Sliders
    {
        MySlider
    }

    enum Buttons 
    { 
        NewButton,
        NewNewButton
    }


    public override void Init()
    {
        base.Init();

        Bind<Image>(typeof(Images));
        Bind<Slider>(typeof(Sliders));
        Bind<Button>(typeof(Buttons));

        Get<Image>((int)Images.TestImage);

        Get<Button>((int)Buttons.NewButton).gameObject.BindEvent(SetNewButton);
    }

    void s(PointerEventData data)
    {

    }
}
