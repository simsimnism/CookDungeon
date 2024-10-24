using UnityEngine;

public class PickupItem : MonoBehaviour
{
    private bool isInRange = false;
    private Sprite itemSprite;

    public Item itemData; // 픽업할 아이템 데이터

    void Start()
    {
        // 프리팹의 SpriteRenderer에서 스프라이트 가져오기
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            itemSprite = spriteRenderer.sprite; // 프리팹의 스프라이트 저장
        }
        else
        {
            Debug.LogError("SpriteRenderer가 없습니다.");
        }
    }

    void Update()
    {
        // 플레이어가 범위 안에 있고 'f' 키를 눌렀을 때 아이템을 습득
        if (isInRange && Input.GetKeyDown(KeyCode.F))
        {
            if (Managers.Inventory.AddItem(itemData)) // 아이템 추가 성공 여부 반환
            {
                FindObjectOfType<InventoryUI>().UpdateInventoryUI(Managers.Inventory.items); // UI 업데이트
                Managers.Inventory.SaveInventory(); // 아이템 추가 후 인벤토리 저장
                Destroy(gameObject); // 아이템 오브젝트 삭제
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isInRange = false;
        }
    }
}
