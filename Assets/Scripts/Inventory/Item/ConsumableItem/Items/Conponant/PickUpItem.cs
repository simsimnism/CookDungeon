using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private Inventory inventory; // Inventory 참조
    private bool isInRange = false; // 플레이어가 범위 내에 있는지 여부

    private void Start()
    {
        // 인벤토리 매니저 참조 가져오기
        inventory = FindObjectOfType<Inventory>();
    }

    // PlayerController에서 호출할 메서드
    public void Pickup()
    {
        string prefabName = gameObject.name;

        if (inventory != null)
        {
            inventory.AcquireItem(prefabName); // 인벤토리에 아이템 추가
            Destroy(gameObject); // 아이템 습득 후 제거
        }
        else
        {
            Debug.LogWarning("Inventory를 찾을 수 없습니다.");
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어가 범위에 들어왔을 때
        if (other.CompareTag("Player"))
        {
            isInRange = true;
            ShowPickupMessage(true); // 아이템 습득 가능 메시지 표시
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 플레이어가 범위를 벗어났을 때
        if (other.CompareTag("Player"))
        {
            isInRange = false;
            ShowPickupMessage(false); // 아이템 습득 가능 메시지 숨김
        }
    }

    // 아이템 습득 메시지를 표시하거나 숨기는 메서드
    private void ShowPickupMessage(bool show)
    {
        if (show)
        {
            Debug.Log("Press F to pick up the item."); // UI로 전환 가능
        }
        else
        {
            Debug.Log("Out of range.");
        }
    }
}
