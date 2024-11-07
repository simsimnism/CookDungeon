using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon;
    public TMP_Text itemName;  // TMP_Text로 변경

    public void SetItem(Item item)
    {
        icon.sprite = item.ItemSprite; // 필드명을 ItemSprite로 수정
        itemName.text = item.Name;      // 필드명을 Name으로 수정
        icon.enabled = true;
    }

    public void ClearItem()
    {
        
    }
}
