using UnityEngine;

[CreateAssetMenu(fileName = "MonsterDataSO", menuName = "ScriptableObjects/MonsterDataSO", order = 1)]
public class MonsterDataSO : ScriptableObject
{
    public string monsterName;  // 몬스터 이름 (프리팹 이름과 비교)
    public int id;              // 몬스터 ID
    public int health;          // 체력
    public int attack;          // 공격력
    public float range;         // 공격 범위
    public float speed;         // 이동 속도


    public GameObject monsterPrefab;  // 몬스터 프리팹을 저장할 필드
}
