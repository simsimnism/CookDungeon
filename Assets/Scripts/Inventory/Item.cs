using UnityEngine;

// Item 클래스는 ScriptableObject를 사용해 에셋으로 관리 가능
[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName; // 아이템 이름
    public Sprite icon; // UI에 표시될 아이템 아이콘
    public bool isStackable; // 중복해서 쌓을 수 있는지 여부

    // 아이템을 사용할 때의 기본 동작
    public virtual void Use()
    {
        Debug.Log("Using " + itemName);
    }
}
