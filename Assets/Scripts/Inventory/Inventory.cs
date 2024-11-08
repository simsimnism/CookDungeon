using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, Item> inventoryItems = new Dictionary<string, Item>();
    private ItemSlot[] slots;
    private DataManager dataManager;  // DataManager 참조 추가

    [SerializeField] private GameObject itemInfoPanel;
    [SerializeField] private Text itemNameText;
    [SerializeField] private Text itemDescriptionText;
    [SerializeField] private Image itemSpriteImage;
    [SerializeField] private Transform slotParent;

    private void Start()
    {
        slots = slotParent.GetComponentsInChildren<ItemSlot>();
        dataManager = new DataManager();
        dataManager.Init(); // 데이터 초기화
    }

    // 아이템 습득 요청을 받아 처리하는 메서드
    public void AcquireItem(string prefabName)
    {
        Item item = dataManager.GetFoodItemByPrefabName(prefabName);
        if (item != null)
        {
            AddItem(item);
        }
        else
        {
            Debug.LogWarning($"{prefabName}에 해당하는 아이템을 찾을 수 없습니다.");
        }
    }

    // 아이템을 인벤토리에 추가하는 메서드 (기존 메서드)
    public void AddItem(Item item)
    {
        if (inventoryItems.ContainsKey(item.Name))
        {
            inventoryItems[item.Name].AddAmount(1);
            UpdateSlot(inventoryItems[item.Name]);
        }
        else
        {
            inventoryItems.Add(item.Name, item);
            AddItemToSlot(item);
            DisplayItemInfo(item);
        }
    }

    private void AddItemToSlot(Item item)
    {
        foreach (ItemSlot slot in slots)
        {
            if (!slot.GetComponent<Image>().enabled)
            {
                slot.SetItem(item);
                break;
            }
        }
    }

    private void UpdateSlot(Item item)
    {
        foreach (ItemSlot slot in slots)
        {
            if (slot.GetComponent<Image>().sprite == item.ItemSprite)
            {
                slot.SetItem(item);
                break;
            }
        }
    }

    private void DisplayItemInfo(Item item)
    {
        itemInfoPanel.SetActive(true);
        itemNameText.text = item.Name;
        itemDescriptionText.text = item.Description;
        itemSpriteImage.sprite = item.ItemSprite;
    }
}
