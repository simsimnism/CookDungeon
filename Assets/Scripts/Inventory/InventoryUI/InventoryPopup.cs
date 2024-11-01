using UnityEngine;

public class InventoryPopup : MonoBehaviour
{
    public KeyCode toggleKey = KeyCode.Q;  // 인벤토리 창을 열고 닫는 키
    private GameObject _inventoryCanvas;   // 인벤토리 캔버스 프리팹을 가리킬 변수

    private void Start()
    {
        Managers.Input.KeyAction += OnKeyPress;
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
        if (_inventoryCanvas == null)
        {
            // 처음 한 번만 프리팹을 로드
            LoadInventoryCanvas();
        }

        if (_inventoryCanvas != null)
        {
            // 활성 상태를 반전시켜 열거나 닫기
            bool isActive = _inventoryCanvas.activeInHierarchy;
            _inventoryCanvas.SetActive(!isActive);
        }
    }


    private bool LoadInventoryCanvas()
    {
        if (Managers.Resource == null)
        {
            Debug.LogError("Managers.Resource is not initialized.");
            return false;
        }

        if (Managers.UI == null || Managers.UI.Root == null)
        {
            Debug.LogError("Managers.UI or Managers.UI.Root is not initialized.");
            return false;
        }

        // 프리팹을 지정된 경로에서 로드
        GameObject originalPrefab = Managers.Resource.Load<GameObject>("Prefabs/UI/Popup/InventoryPopup");
        if (originalPrefab == null)
        {
            Debug.LogError("Failed to load inventory prefab at Prefabs/UI/Popup/InventoryPopup");
            return false;
        }

        _inventoryCanvas = Object.Instantiate(originalPrefab, Managers.UI.Root.transform);
        if (_inventoryCanvas == null)
        {
            Debug.LogError("Failed to instantiate inventory canvas prefab.");
            return false;
        }

        return true;
    }

}
