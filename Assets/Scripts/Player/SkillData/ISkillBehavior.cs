using UnityEngine;

//게임 오브젝트와 스킬의 내용을 분리
public interface ISkillBehavior
{
    void Execute(GameObject user, SkillDataSO skillData);
}
