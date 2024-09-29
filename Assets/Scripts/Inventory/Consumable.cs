using UnityEngine;

[CreateAssetMenu(fileName = "New Consumable", menuName = "Inventory/Consumable")]
public class Consumable : Item
{
    public int charges; // 사용 가능 횟수

    public override void Use()
    {
        if (charges > 0)
        {
            charges--;
            Debug.Log(itemName + " used. Remaining charges: " + charges);
        }
        else
        {
            Debug.Log(itemName + " is out of charges.");
        }
    }
}
