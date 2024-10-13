using UnityEngine;

[CreateAssetMenu(fileName = "NewMonsterDataSO", menuName = "Monster/MonsterData")]
public class MonsterDataSO : ScriptableObject
{
    public string monsterName;
    public GameObject monsterPrefab;
    public int health;
    public int attack;
    public float range;
    public float speed;
}
