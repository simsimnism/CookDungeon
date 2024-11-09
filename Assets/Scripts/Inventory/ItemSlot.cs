using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image itemImage; // 아이템 이미지 (스프라이트)
    public TMP_Text itemAmountText; // 아이템 수량 텍스트
    public Item currentItem;
    public Inventory parentInventory;

    private void Awake()
    {
        ClearSlot();
    }

    // 아이템을 슬롯에 설정하는 메서드
    public void SetItem(Item item)
    {
        currentItem = item;

        if (itemImage != null)
        {
            // 스프라이트가 제대로 로드되었는지 확인
            if (item.ItemSprite != null)
            {
                itemImage.sprite = item.ItemSprite;
                itemImage.enabled = true; // 이미지 표시
                itemImage.color = new Color(1, 1, 1, 1); // 불투명하게 설정
                Debug.Log($"스프라이트 설정 완료: {item.Name}");
            }
            else
            {
                Debug.LogWarning($"스프라이트가 없습니다: {item.Name}");
                itemImage.enabled = false; // 스프라이트가 없으면 이미지 숨기기
            }
        }

        // 아이템의 현재 수량을 정확히 텍스트로 표시
        UpdateAmountText();
    }

    // 아이템 수량 텍스트 업데이트 메서드
    public void UpdateAmountText()
    {
        if (itemAmountText != null)
        {
            itemAmountText.text = currentItem.Amount > 1 ? currentItem.Amount.ToString() : "";
            itemAmountText.enabled = currentItem.Amount > 1;
        }
    }

    // 슬롯을 비우는 메서드
    public void ClearSlot()
    {
        currentItem = null;

        if (itemImage != null)
        {
            itemImage.sprite = null;
            itemImage.enabled = false;
        }

        if (itemAmountText != null)
        {
            itemAmountText.text = "";
            itemAmountText.enabled = false;
        }

        Debug.Log("슬롯 비우기 완료.");
    }

    // 슬롯이 비어 있는지 확인하는 메서드
    public bool IsEmpty()
    {
        return currentItem == null;
    }
}
