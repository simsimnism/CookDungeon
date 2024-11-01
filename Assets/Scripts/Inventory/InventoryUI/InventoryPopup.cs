using UnityEngine;

public class InventoryPopup : UI_Popup
{
    public KeyCode toggleKey = KeyCode.Q; // 인벤토리 창을 열고 닫는 키
    private GameObject _inventoryCanvas;   // 인벤토리 캔버스 프리팹을 가리킬 변수

    public override void Init()
    {
        base.Init(); // 부모 클래스(UI_Popup)의 Init() 호출하여 기본 설정

        if (_inventoryCanvas == null)
        {
            LoadInventoryCanvas();
            if (_inventoryCanvas != null)
                _inventoryCanvas.SetActive(false); // 시작 시 비활성화
        }
    }

    private void Start()
    {
        Managers.Input.KeyAction += OnKeyPress;
        Init(); // Start에서 Init을 호출하여 초기화 시작
    }

    private void OnDestroy()
    {
        Managers.Input.KeyAction -= OnKeyPress;
    }

    private void OnKeyPress()
    {
        if (Input.GetKeyDown(toggleKey))
        {
            TogglePopup();
        }
    }

    private void TogglePopup()
    {
        if (_inventoryCanvas == null) return;

        if (_inventoryCanvas.activeInHierarchy)
        {
            CloseInventoryPopup();
        }
        else
        {
            Managers.UI.ShowPopupUI<InventoryPopup>(); // 팝업 스택에 추가
            _inventoryCanvas.SetActive(true); // 인벤토리 창을 활성화
        }
    }

    private void CloseInventoryPopup()
    {
        if (_inventoryCanvas != null)
        {
            _inventoryCanvas.SetActive(false); // 인벤토리 캔버스를 비활성화
            Managers.UI.ClosePopupUI(); // 팝업 스택에서 제거
        }
    }

    private bool LoadInventoryCanvas()
    {
        if (Managers.Resource == null || Managers.UI == null || Managers.UI.Root == null)
        {
            Debug.LogError("Managers.Resource, Managers.UI, 또는 UI Root가 초기화되지 않았습니다.");
            return false;
        }

        GameObject originalPrefab = Managers.Resource.Load<GameObject>("Prefabs/UI/Popup/InventoryPopup");
        if (originalPrefab == null)
        {
            Debug.LogError("Failed to load inventory prefab at Prefabs/UI/Popup/InventoryPopup");
            return false;
        }

        _inventoryCanvas = Object.Instantiate(originalPrefab, Managers.UI.Root.transform);
        return _inventoryCanvas != null;
    }
}
