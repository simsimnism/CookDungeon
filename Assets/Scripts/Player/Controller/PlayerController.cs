using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //아직 사용 안함
    public string gameState;

    //Pm에서 사용할 변수
    public float moveSpeed;  // 움직임 속도

    private float transparencyFadeTime; // 투명화가 진행되는 시간
    private float transparencyHoldTime; // 투명화가 완료된 후 유지되는 시간

    //GM에서 관리할 변수
    private bool isInvincible; // 무적인지 여부

    //각 스크립트에서 직접 관리?
    public float invincibleDuration = 1f; // 무적 지속 시간

    private SpriteRenderer spriteRenderer; // 반투명 상태를 위한 SpriteRenderer

    //인벤토리 팝업 관련 변수
    public InventoryPopup InventoryPopup;

    // 아이템 습득 관련 변수
    private PickupItem currentPickupItem; // 현재 범위 내 아이템 참조

    void Awake()
    {
        InventoryPopup = GetComponent<InventoryPopup>();
        transparencyHoldTime = Managers.Player.transparencyHoldTime;
        transparencyFadeTime = Managers.Player.transparencyFadeTime;
        isInvincible = Managers.GM.IsInvincible;
        moveSpeed = Managers.Player.moveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Managers.Player.Init();
        Managers.Input.KeyAction -= OnKeyMove;
        Managers.Input.KeyAction += OnKeyMove;
    }

    // WASD로 상하좌우 이동 & F키로 아이템 습득
    private void OnKeyMove()
    {
        // 이동 가능 여부 체크
        if (!Managers.Player.canMove)
            return;

        if (!Managers.GM.IsMoving)
            return;

        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        }
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = -1f;
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveVertical = 0;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveHorizontal = 0;

        // 대각선 방향 조정
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        // 이동
        transform.Translate(direction * moveSpeed * Time.deltaTime);

        // F 키로 아이템 습득
        if (Input.GetKeyDown(KeyCode.F) && currentPickupItem != null)
        {
            currentPickupItem.Pickup(); // 아이템 습득
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("키 입력을 감지했습니다");
            Managers.Inventory.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            Managers.Popup.TogglePauseUI();
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // "PickupItem" 태그가 있는 오브젝트와 충돌 시
        if (other.CompareTag("PickupItem"))
        {
            currentPickupItem = other.GetComponent<PickupItem>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // "PickupItem" 태그가 있는 오브젝트와 충돌 해제 시
        if (other.CompareTag("PickupItem"))
        {
            currentPickupItem = null;
        }
    }
}
