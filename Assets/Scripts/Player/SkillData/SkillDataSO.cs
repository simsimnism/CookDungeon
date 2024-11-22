using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObject/SkillDataSO")]
public class SkillDataSO : ScriptableObject
{
    // 공통 속성
    public string skillName;            // 스킬 이름
    public Sprite icon;                 // 스킬 아이콘
    public GameObject skillPrefab;      // 스킬에 사용할 이펙트 프리팹
    public float cooldown;              // 쿨다운 시간
    public float range;                 // 스킬 범위
    public string description;          // 스킬 설명

    // 동작을 연결할 수 있는 필드
    public MonoBehaviour skillBehavior; // 스킬 실행 시 동작

    // 스킬 실행
    public void ActivateSkill(GameObject user)
    {
        if (skillBehavior != null)
        {
            // 스킬 동작을 실행
            (skillBehavior as ISkillBehavior)?.Execute(user, this);
        }
        else
        {
            Debug.LogWarning($"{skillName} has no behavior assigned.");
        }
    }
}
