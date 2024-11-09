public class InventoryPopup : UI_Popup
{
    public override void Init()
    {
        base.Init();
    }

    public override void ClosePopupUI()
    {
        gameObject.SetActive(false); // 파괴하지 않고 비활성화
        Managers.Inventory.CloseInventory();
    }
}
