using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public void Pickup()
    {
        string prefabName = gameObject.name;
        Managers.Inventory.AddItemToInventory(prefabName);
        Destroy(gameObject);
    }
}
