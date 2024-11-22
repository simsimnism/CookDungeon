using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillTotalSO", menuName = "ScriptableObject/SkillTotalSO")]
public class SkillTotalSO : ScriptableObject
{
    // 스킬 목록
    public SkillDataSO[] skills;

    // 스킬 이름으로 SkillDataSO 검색
    public SkillDataSO GetSkillDataSOByName(string name)
    {
        foreach (var skillData in skills)
        {
            if (skillData.skillName == name)
                return skillData;
        }
        return null;
    }
}
