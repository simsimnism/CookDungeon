using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

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

    // 상호작용 관련 변수
    private float interactionRange = 2f; // 상호작용 거리
    private InteractableObject currentInteractable; // 현재 상호작용 가능한 오브젝트

    private Vector2 moveDirection; // 이동 방향을 저장할 변수
    private InteractiveMasegge textManager; // 텍스트 관리 스크립트

    private GameObject hpBarCanvas; // HpBar를 담고 있는 캔버스 오브젝트
    private Vector3 hpBarOffset = new Vector3(0, 1.5f, 0); // 머리 위에 띄울 오프셋


    void Awake()
    {
        textManager = FindObjectOfType<InteractiveMasegge>();
        if (textManager == null)
        {
            Debug.LogWarning("InteractionTextManager를 찾을 수 없습니다. Scene에 추가해 주세요.");
        }
        InventoryPopup = GetComponent<InventoryPopup>();
        transparencyHoldTime = Managers.Player.transparencyHoldTime;
        transparencyFadeTime = Managers.Player.transparencyFadeTime;
        isInvincible = Managers.GM.IsInvincible;
        moveSpeed = Managers.Player.MoveSpeed;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        Managers.Player.Init();
        Managers.Input.KeyAction -= OnKeyMove; // 기존 이벤트 해제
        Managers.Input.KeyAction += OnKeyMove; // 새로운 이벤트 등록
    }

    private void OnDestroy()
    {
        Managers.Input.KeyAction -= OnKeyMove; // 삭제 시 이벤트 해제
    }

    private void Update()
    {
        // 상호작용 범위 내 오브젝트 확인
        DetectInteractableObjects();
        UpdateInteractionTextPosition();
    }

    // WASD로 상하좌우 이동 & F키로 아이템 습득
    private void OnKeyMove()
    {
        if (this == null) return; // Null 체크 추가

        // 이동 가능 여부 체크
        if (!Managers.Player.canMove || !Managers.GM.IsMoving)
            return;


        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.A))
        {
            moveHorizontal = -1f;
            spriteRenderer.flipX = false;
        }
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.D))
        {
            moveHorizontal = 1f;
            spriteRenderer.flipX = true;
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S)) moveVertical = 0;
        if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D)) moveHorizontal = 0;

        // 대각선 방향 조정
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        // 이동
        transform.Translate(direction * moveSpeed * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("키 입력을 감지했습니다");
            Managers.Inventory.ToggleInventory();
        }

        if (Input.GetKeyDown(KeyCode.Tab))
        {
            Managers.Popup.TogglePauseUI();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            Managers.Popup.ToggleRecipe();
        }

        // 이동 방향에 따라 상호작용 텍스트 위치 조정
        UpdateInteractionTextPosition();
        UpdateHpBarPosition();
    }
    
    //상호작용의 텍스트의 위치를 카메라 위치 즉 월드 좌표에 맞춰서 이동시키는 코드
    private void UpdateInteractionTextPosition()
    {
        if (textManager == null || textManager.interactionText == null) return;

        // 플레이어 위치를 화면 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // 텍스트 위치 조정 (플레이어 위쪽으로 오프셋 추가)
        Vector3 offset = new Vector3(0, 130, 0); // 필요에 따라 조정
        textManager.interactionText.transform.position = screenPosition + offset;
    }

    //상호작용 오브젝트와 접근했을때 상호작용하는 코드
    private void DetectInteractableObjects()
    {
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, interactionRange);

        InteractableObject closestInteractable = null;
        float closestDistance = interactionRange;

        foreach (Collider2D collider in interactables)
        {
            InteractableObject interactable = collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, interactable.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.Highlight(false);
                textManager?.HideInteractionMessage();
            }

            currentInteractable = closestInteractable;

            if (currentInteractable != null)
            {
                currentInteractable.Highlight(true);
                textManager?.ShowInteractionMessage(currentInteractable.GetInteractionMessage());
            }
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
        if (currentPickupItem == other.GetComponent<PickupItem>())
        {
            currentPickupItem = null;
            textManager?.HideInteractionMessage(); // 텍스트 숨기기
        }
    }

    private void UpdateHpBarPosition()
    {
        if (hpBarCanvas == null) return;

        // 플레이어 위치를 화면 좌표로 변환
        Vector3 screenPosition = Camera.main.WorldToScreenPoint(transform.position);

        // HP 바를 화면 좌표의 위쪽으로 오프셋을 적용하여 위치 설정
        Vector3 offset = new Vector3(0, 100, 0); // 필요에 따라 y값을 조정하여 머리 위로 띄웁니다.
        hpBarCanvas.transform.position = screenPosition + offset;
    }

}
