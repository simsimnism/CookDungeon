using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Image icon; // 아이템 스프라이트를 표시할 UI 이미지
    public Text amountText; // 아이템 수량을 표시할 텍스트

    // 슬롯에 아이템 설정
    public void SetItem(Item item)
    {
        icon.sprite = item.ItemSprite; // 아이템 스프라이트 설정
        icon.enabled = true; // 아이템 이미지 활성화
        amountText.text = item.Amount > 1 ? item.Amount.ToString() : ""; // 수량이 1 이상일 때만 표시
    }

    // 슬롯 비우기
    public void ClearSlot()
    {
        icon.sprite = null; // 이미지 초기화
        icon.enabled = false; // 이미지 비활성화
        amountText.text = ""; // 텍스트 초기화
    }
}
