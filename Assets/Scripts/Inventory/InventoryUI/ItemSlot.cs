using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public int Index { get; set; }

    public void UpdateSlot(Item itemData)
    {
        if (itemData != null)
        {
            // 슬롯에 아이템 데이터 반영 (예: 이미지, 이름)
        }
        else
        {
            // 빈 슬롯 처리
        }
    }
}
