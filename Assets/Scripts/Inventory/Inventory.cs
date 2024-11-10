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

    public bool AddItemToInventoryByName(string prefabName)
    {
        if (_itemCache.TryGetValue(prefabName, out var cachedItem))
        {
            ItemSlot existingSlot = FindItemSlotByName(cachedItem.Name);
            if (existingSlot != null)
            {
                if (existingSlot.currentItem.Amount >= existingSlot.currentItem.MaxAmount)
                {
                    Debug.LogWarning($"{cachedItem.Name} 아이템의 수량이 최대치입니다.");
                    return false;
                }

                existingSlot.currentItem.AddAmount(1);
                existingSlot.UpdateAmountText();
                Debug.Log($"{cachedItem.Name}의 수량 증가: {existingSlot.currentItem.Amount}");
                return true;
            }
        }
        else
        {
            Item item = Managers.Data.GetFoodItemByPrefabName(prefabName) ?? (Item)Managers.Data.GetRecipeItemByPrefabName(prefabName);
            if (item == null)
            {
                Debug.LogWarning($"아이템 '{prefabName}'을(를) 찾을 수 없습니다.");
                return false;
            }

            // Load sprite and assign it to the item
            item.ItemSprite = LoadSpriteWithCaching($"Sprites/Food/{prefabName}");
            item.SetAmount(1);
            _itemCache[prefabName] = item;

            ItemSlot emptySlot = GetEmptySlot();
            if (emptySlot != null)
            {
                emptySlot.SetItem(item);
                Debug.Log($"{item.Name} 새 슬롯에 추가 - 현재 수량: {item.Amount}");
                return true;
            }
            else
            {
                Debug.LogWarning("빈 슬롯을 찾을 수 없거나 인벤토리가 가득 찼습니다.");
                return false;
            }
        }

        return false;
    }

    private Sprite LoadSpriteWithCaching(string spritePath)
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
            if (slot != null && slot.currentItem != null && slot.currentItem.Name == name)
                return slot;
        }
        return null;
    }

    public void SwapItems(ItemSlot slotA, ItemSlot slotB)
    {
        if (slotA == null || slotB == null) return;

        Item tempItem = slotA.currentItem;
        slotA.SetItem(slotB.currentItem);
        slotB.SetItem(tempItem);

        Debug.Log($"{slotA.currentItem?.Name ?? "빈 슬롯"}와 {slotB.currentItem?.Name ?? "빈 슬롯"} 교환 완료");
    }
}
