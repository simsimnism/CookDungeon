using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorTexture; // 커서 이미지
    public Texture2D clickedCursorTexture; // 클릭 시 커서 이미지
    public Vector2 hotSpot = Vector2.zero; // 핫스팟 위치
    public CursorMode cursorMode = CursorMode.Auto; // 커서 모드
    public float interactionRange = 1f; // 탐지 반경
    private InteractiveMasegge textManager; // 메시지 관리

    private void Start()
    {
        // 커서를 커스텀 이미지로 설정
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);

        // 메시지 매니저 찾기
        textManager = FindObjectOfType<InteractiveMasegge>();
        if (textManager == null)
        {
            Debug.LogWarning("InteractiveMasegge를 찾을 수 없습니다. Scene에 추가해주세요.");
        }

        // Collider 설정
        CircleCollider2D collider = gameObject.AddComponent<CircleCollider2D>();
        collider.radius = interactionRange; // 탐지 반경 설정
        collider.isTrigger = true; // Trigger 활성화
    }

    private void Update()
    {
        // 마우스 왼쪽 버튼을 누를 때
        if (Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(clickedCursorTexture, hotSpot, cursorMode);
        }
        // 마우스 왼쪽 버튼에서 손을 뗄 때
        else if (Input.GetMouseButtonUp(0))
        {
            Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
        }
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        // 상호작용 가능한 오브젝트 탐지
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if (interactable != null)
        {
            interactable.Highlight(true); // 하이라이트 활성화
            textManager?.ShowInteractionMessage(interactable.GetInteractionMessage()); // 메시지 표시
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // 탐지 범위를 벗어났을 때
        InteractableObject interactable = other.GetComponent<InteractableObject>();
        if (interactable != null)
        {
            interactable.Highlight(false); // 하이라이트 비활성화
            textManager?.HideInteractionMessage(); // 메시지 숨기기
        }
    }
}
