using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D clickedCursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    private InteractableObject currentInteractable;
    private PickupItem currentPickupItem;

    public CursorMode cursorMode = CursorMode.Auto;
    private CircleCollider2D interactionCollider;

    private InteractiveMasegge textManager;

    private void Start()
    {
        // 커서 텍스처의 중심을 중단점으로 설정
        hotSpot = new Vector2(cursorTexture.width / 2, cursorTexture.height / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        textManager = FindObjectOfType<InteractiveMasegge>();
        if (textManager == null)
        {
            Debug.LogWarning("InteractiveMasegge를 찾을 수 없습니다. Scene에 추가해주세요.");
        }

        // CircleCollider2D 추가 및 설정
        interactionCollider = gameObject.AddComponent<CircleCollider2D>();
        interactionCollider.radius = 2f; // 초기 반경
        interactionCollider.isTrigger = true;
    }

    private void Update()
    {
        DirectionMouseObject();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        UpdateInteractionTextPosition();
        if (Input.GetMouseButtonDown(0) && currentPickupItem != null)
        {
            currentPickupItem.Pickup(); // 아이템 습득
            Debug.Log("아이템을 습득했습니다.");
 
            Cursor.SetCursor(clickedCursorTexture, hotSpot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    private void DirectionMouseObject()
    {
        // CircleCollider2D의 반경을 기준으로 선택 범위 확인
        Collider2D[] interactables = Physics2D.OverlapCircleAll(transform.position, interactionCollider.radius);

        InteractableObject closestInteractable = null;
        float closestDistance = interactionCollider.radius;

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

        // 상호작용 오브젝트가 변경되었는지 확인
        if (closestInteractable != currentInteractable)
        {
            if (currentInteractable != null)
            {
                currentInteractable.Highlight(false);
                textManager?.HideInteractionMessage(); // 이전 메시지 숨기기
            }

            currentInteractable = closestInteractable;

            if (currentInteractable != null)
            {
                currentInteractable.Highlight(true);
                string message = currentInteractable.GetInteractionMessage();
                textManager?.ShowInteractionMessage(message); // 새로운 메시지 출력
            }
        }
    }

    // 상호작용 텍스트를 마우스 위치에 따라 이동시키는 코드 (UI 기준)
    private void UpdateInteractionTextPosition()
    {
        if (textManager == null || textManager.interactionText == null) return;

        // 마우스 위치를 가져옴 (화면 좌표)
        Vector3 mouseScreenPosition = Input.mousePosition;

        // UI 캔버스 위치를 기준으로 텍스트 위치 설정 (화면 좌표 그대로 사용)
        Vector3 offset = new Vector3(0, 20, 0); // 마우스 위쪽으로 약간 띄우기 위한 y축 오프셋
        textManager.interactionText.transform.position = mouseScreenPosition + offset;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PickupItem"))
        {
            PickupItem pickupItem = other.GetComponent<PickupItem>();
            if (pickupItem != null)
            {
                if (currentPickupItem == null ||
                    Vector2.Distance(transform.position, pickupItem.transform.position) < Vector2.Distance(transform.position, currentPickupItem.transform.position))
                {
                    currentPickupItem = pickupItem; // 가장 가까운 아이템으로 업데이트
                }
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (currentPickupItem == other.GetComponent<PickupItem>() && currentPickupItem != null)
        {
            currentPickupItem = null;

            // 근처에 다른 아이템이 있다면 가장 가까운 아이템을 다시 설정
            Collider2D[] pickups = Physics2D.OverlapCircleAll(transform.position, interactionCollider.radius);
            float closestDistance = interactionCollider.radius;

            foreach (Collider2D collider in pickups)
            {
                PickupItem pickupItem = collider.GetComponent<PickupItem>();
                if (pickupItem != null)
                {
                    float distance = Vector2.Distance(transform.position, pickupItem.transform.position);
                    if (distance < closestDistance)
                    {
                        closestDistance = distance;
                        currentPickupItem = pickupItem;
                    }
                }
            }
        }

        textManager?.HideInteractionMessage();
    }


    // Editor에서 반경을 동적으로 확인할 수 있도록 Gizmos에 표시
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionCollider != null ? interactionCollider.radius : 2f);
    }
}
