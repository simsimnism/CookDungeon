using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture;
    public Texture2D clickedCursorTexture;
    public Vector2 hotSpot = Vector2.zero;

    private InteractableObject currentInteractable;
    private PickupItem currentPickupItem;

    public CursorMode cursorMode = CursorMode.Auto;
    private float interactionRange = 2f;

    private InteractiveMasegge textManager;

    private void Start()
    {
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        textManager = FindObjectOfType<InteractiveMasegge>();
        if (textManager == null)
        {
            Debug.LogWarning("InteractiveMasegge를 찾을 수 없습니다. Scene에 추가해주세요.");
        }

        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = interactionRange;
        collider.isTrigger = true;
    }

    private void Update()
    {
        DirectionMouseObject();

        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = mousePosition;

        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickedCursorTexture, hotSpot, cursorMode);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }

    private void DirectionMouseObject()
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


    private void OnTriggerEnter2D(Collider2D other)
    {
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
            textManager?.HideInteractionMessage();
        }
    }
}
