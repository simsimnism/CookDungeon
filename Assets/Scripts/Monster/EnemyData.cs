using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyData : MonoBehaviour
{
    public int id; // 몬스터의 고유 ID
    public new string name; // 몬스터 이름
    public float health; // 몬스터의 최대 체력
    public float attack; // 몬스터의 공격력
    public float range; // 몬스터의 탐지 범위
    public float attackRange; // 몬스터의 공격 범위
    public float speed; // 몬스터의 이동 속도
    public int spriteType; // 몬스터의 애니메이터 컨트롤러 인덱스

    // JSON 파일의 데이터를 저장할 클래스
    [System.Serializable]
    public class EnemyDataList
    {
        public List<EnemyData> monsters;
    }
}