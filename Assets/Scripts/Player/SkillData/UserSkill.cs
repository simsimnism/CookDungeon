using UnityEngine;

public class UserSkill : MonoBehaviour
{
    public SkillTotalSO skillSet; // 연결된 스킬 세트
    private float[] cooldownTimers; // 쿨다운 관리

    private void Start()
    {
        cooldownTimers = new float[skillSet.skills.Length];
    }

    private void Update()
    {
        // 쿨다운 타이머 감소
        for (int i = 0; i < cooldownTimers.Length; i++)
        {
            if (cooldownTimers[i] > 0)
                cooldownTimers[i] -= Time.deltaTime;
        }
    }

    public void UseSkill(int index)
    {
        if (index < 0 || index >= skillSet.skills.Length)
        {
            Debug.LogWarning("Invalid skill index.");
            return;
        }

        if (cooldownTimers[index] <= 0)
        {
            SkillDataSO skill = skillSet.skills[index];
            Managers.Skill.ActivateSkill(skill, gameObject); // 매니저에 요청
            cooldownTimers[index] = skill.cooldown; // 쿨다운 설정
        }
        else
        {
            Debug.Log($"{skillSet.skills[index].skillName} is on cooldown.");
        }
    }
}
