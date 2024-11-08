using UnityEngine;

public class InventoryManager 
{
    private InventoryPopup _inventoryPopup;
    private bool _isInventoryOpen = false;
    public InventorySlotGenerate slotGenerate; // 슬롯 생성 스크립트 참조

    public void Update()
    {
        // E 키 입력을 감지하여 인벤토리 열고 닫기
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        if (_isInventoryOpen)
        {
            CloseInventory();
        }
        else
        {
            OpenInventory();
        }
    }

    private void OpenInventory()
    {
        _inventoryPopup = Managers.UI.ShowPopupUI<InventoryPopup>("InventoryPopup");
        _isInventoryOpen = true;
    }

    private void CloseInventory()
    {
        if (_inventoryPopup != null)
        {
            _inventoryPopup.ClosePopupUI();
            _isInventoryOpen = false;
        }
    }

    // 아이템을 인벤토리에 추가하는 메서드
    public void AddItemToInventory(string prefabName)
    {
        // 프리팹 이름을 통해 아이템 정보를 가져와 슬롯에 추가
        Item item = CreateItemFromPrefab(prefabName); // 임시 메서드로 아이템 생성
        if (item != null)
        {
            ItemSlot slot = slotGenerate.GetEmptySlot(); // 빈 슬롯 가져오기
            if (slot != null)
            {
                slot.SetItem(item); // 슬롯에 아이템 추가
            }
        }
    }

    // 임시로 프리팹 이름을 아이템으로 변환하는 메서드 (예시)
    private Item CreateItemFromPrefab(string prefabName)
    {
        // 실제로는 아이템 데이터를 로드하거나 아이템 DB에서 조회하여 생성할 수 있습니다.
        Sprite itemSprite = Resources.Load<Sprite>($"Sprites/{prefabName}");
        return new Item(prefabName, 1, "설명", 99, 1, itemSprite); // 기본 아이템 생성 (예시)
    }
}
