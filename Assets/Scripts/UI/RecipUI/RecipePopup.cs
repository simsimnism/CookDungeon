using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipePopup : UI_Popup
{
    public override void Init()
    {
        base.Init();
    }

    public void CloseRecipe()
    {
        Managers.Popup.CloseRecipe();
    }
}
