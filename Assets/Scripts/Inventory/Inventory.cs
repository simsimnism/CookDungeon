using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotParent;
    public int slotCount = 11;
    private ItemSlot[] slots;
    public bool isInitialized = false;

    private Dictionary<string, Item> _itemCache = new Dictionary<string, Item>();
    private Dictionary<string, Sprite> _spriteCache = new Dictionary<string, Sprite>();

    private void Start()
    {
        if (!isInitialized)
        {
            InitializeSlots();
        }
    }

    public void InitializeSlots()
    {
        if (slotPrefab == null || slotParent == null)
        {
            Debug.LogError("슬롯 프리팹 또는 부모 객체가 설정되지 않았습니다.");
            return;
        }

        slots = new ItemSlot[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slotObj = Instantiate(slotPrefab, slotParent);
            ItemSlot itemSlot = slotObj.GetComponent<ItemSlot>() ?? slotObj.AddComponent<ItemSlot>();
            itemSlot.ClearSlot();
            itemSlot.parentInventory = this;
            slots[i] = itemSlot;
        }
        isInitialized = true;
    }

    public bool AddItemToInventory(Item item)
    {
        if (item == null) return false;

        ItemSlot existingSlot = FindItemSlotByName(item.Name);
        if (existingSlot != null)
        {
            if (existingSlot.CurrentItem.Amount >= existingSlot.CurrentItem.MaxAmount)
            {
                Debug.LogWarning($"{item.Name} 아이템의 수량이 최대치입니다.");
                return false;
            }

            existingSlot.CurrentItem.AddAmount(1);
            existingSlot.UpdateAmountText();
            return true;
        }

        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null)
        {
            item.SetAmount(1);
            emptySlot.SetItem(item);
            emptySlot.UpdateAmountText();
            Debug.Log($"{item.Name} 새 슬롯에 추가 - 현재 수량: {item.Amount}");
            return true;
        }
        else
        {
            Debug.LogWarning("빈 슬롯을 찾을 수 없거나 인벤토리가 가득 찼습니다.");
            return false;
        }
    }

    public Sprite LoadSpriteWithCaching(string spritePath)
    {
        if (_spriteCache.TryGetValue(spritePath, out var cachedSprite))
            return cachedSprite;

        Sprite newSprite = Resources.Load<Sprite>(spritePath);
        if (newSprite != null)
            _spriteCache[spritePath] = newSprite;
        else
            Debug.LogWarning($"스프라이트를 찾을 수 없습니다: {spritePath}");

        return newSprite;
    }

    public ItemSlot GetEmptySlot()
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.IsEmpty())
                return slot;
        }
        return null;
    }

    public ItemSlot FindItemSlotByName(string name)
    {
        foreach (var slot in slots)
        {
            if (slot != null && slot.CurrentItem != null && slot.CurrentItem.Name == name)
                return slot;
        }
        // 이름에 해당하는 슬롯이 없으면 null 반환
        return null;
    }

    public bool RemoveItemByName(string itemName)
    {
        if (!isInitialized)
        {
            Debug.LogWarning("인벤토리가 초기화되지 않았습니다.");
            return false;
        }

        ItemSlot itemSlot = FindItemSlotByName(itemName);
        if (itemSlot != null && itemSlot.CurrentItem != null)
        {
            itemSlot.CurrentItem.DecreaseAmount(1);

            if (itemSlot.CurrentItem.Amount <= 0)
            {
                itemSlot.ClearSlot();
            }

            itemSlot.UpdateAmountText();
            return true;
        }

        Debug.LogWarning($"'{itemName}' 아이템을 인벤토리에서 찾을 수 없습니다.");
        return false;
    }

}
