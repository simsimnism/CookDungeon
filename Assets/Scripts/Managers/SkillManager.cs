using UnityEngine;

public class SkillManager
{
    public void ActivateSkill(SkillDataSO skill, GameObject user)
    {
        if (skill == null || user == null)
        {
            Debug.LogWarning("Skill or User is null.");
            return;
        }

        Debug.Log($"{user.name} activated skill {skill.skillName}");

        // 스킬 프리팹 생성 및 초기화
        if (skill.skillPrefab != null)
        {
            GameObject effect = Object.Instantiate(skill.skillPrefab, user.transform.position, Quaternion.identity);
            var behavior = effect.GetComponent<ISkillBehavior>();

            if (behavior != null)
            {
                behavior.Execute(user, skill); // 스킬 동작 실행
            }
        }
    }
}
