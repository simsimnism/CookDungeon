using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SkillCollection", menuName = "ScriptableObjects/SkillCollectionSO")]
public class SkillCollectionSO : ScriptableObject
{
    public List<SkillDataSO> skills;  // 모든 스킬들을 담은 리스트
}
