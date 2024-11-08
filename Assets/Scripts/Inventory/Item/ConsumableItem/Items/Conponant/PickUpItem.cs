using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private InventoryManager inventoryManager;

    private void Start()
    {
        // InventoryManager 참조 가져오기
        inventoryManager = Managers.Inventory;
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager를 찾을 수 없습니다.");
        }
    }

    // PlayerController에서 호출할 메서드
    public void Pickup()
    {
        if (inventoryManager == null)
        {
            Debug.LogWarning("InventoryManager를 찾을 수 없습니다.");
            return;
        }

        string prefabName = gameObject.name;
        inventoryManager.AddItemToInventory(prefabName); // InventoryManager에서 아이템 추가 처리
        Destroy(gameObject); // 아이템 습득 후 제거
    }
}
