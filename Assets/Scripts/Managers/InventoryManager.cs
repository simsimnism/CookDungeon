using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager
{
    public void Update()
    {
        // 'E' 키를 눌러 인벤토리를 토글
        if (Input.GetKeyDown(KeyCode.E))
        {
            Managers.UI.ShowPopupUI<InventoryPopup>().ToggleInventory();
        }
    }
}