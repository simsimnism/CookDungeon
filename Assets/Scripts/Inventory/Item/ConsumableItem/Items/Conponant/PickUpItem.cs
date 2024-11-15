using UnityEngine;

public class PickupItem : MonoBehaviour
{

    private void Start()
    {
        // 오브젝트가 생성된 지 10초 후에 자동으로 삭제
        Destroy(gameObject, 10f);
    }

    public void Pickup()
    {
        string prefabName = gameObject.name;
        Managers.Inventory.AddItemToInventory(prefabName);
        Destroy(gameObject);
    }
}
