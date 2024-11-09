using UnityEngine;
using UnityEngine.EventSystems;

public class MovableHeaderUI : MonoBehaviour, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private RectTransform _parentRectTransform;
    private Canvas _parentCanvas;
    private bool _isDragging = false;
    private Vector2 _dragOffset;

    private void Awake()
    {
        // 부모의 RectTransform을 참조 (창 전체 이동을 위해)
        _parentRectTransform = transform.parent.GetComponent<RectTransform>();
        _parentCanvas = GetComponentInParent<Canvas>();

        if (_parentRectTransform == null || _parentCanvas == null)
        {
            Debug.LogError("DraggableUI: 부모 RectTransform 또는 Canvas가 필요합니다.");
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 드래그 시작 위치와 부모 위치 간의 오프셋 계산
        Vector2 localMousePosition;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _parentCanvas.transform as RectTransform,
            eventData.position,
            _parentCanvas.worldCamera,
            out localMousePosition
        );

        // 오프셋 = 부모 오브젝트의 중심 - 마우스 클릭 위치
        _dragOffset = _parentRectTransform.anchoredPosition - localMousePosition;
        _isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (_isDragging)
        {
            Vector2 localMousePosition;

            // Canvas 기준으로 마우스 위치 계산
            if (_parentCanvas != null && RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                eventData.position,
                _parentCanvas.worldCamera,
                out localMousePosition))
            {
                // 부모 오브젝트 위치 = 마우스 위치 + 드래그 시작 오프셋
                _parentRectTransform.anchoredPosition = localMousePosition + _dragOffset;
            }
            else
            {
                // Canvas가 없는 경우, 스크린 좌표를 직접 사용
                Vector3 screenPoint = Input.mousePosition;
                screenPoint.z = 0;
                _parentRectTransform.position = screenPoint;
            }
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isDragging = false;
    }
}
