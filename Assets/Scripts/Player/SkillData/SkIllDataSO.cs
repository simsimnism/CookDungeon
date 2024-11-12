using UnityEngine;
[CreateAssetMenu(fileName = "SkillDataSO", menuName = "ScriptableObjects/SkillDataSO", order = 1)]
public class SkillDataSO : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    public int skillIndex;
    public float baseCooldown;
    public GameObject skillPrefab; // 스킬 프리팹 참조
    public int level = 1; // 스킬 레벨

    public void AssignToPlayer(GameObject player)
    {
        GameObject skillInstance = Instantiate(skillPrefab, player.transform);
        ISkill skillScript = skillInstance.GetComponent<ISkill>();

        if (skillScript != null)
        {
            skillScript.Initialize(this);
        }
    }

    public void LevelUp()
    {
        level++; // 레벨 증가
        baseCooldown -= 0.5f; // 예시: 쿨다운 감소 등 레벨업에 따른 효과 부여
        Debug.Log($"{skillName}의 레벨이 {level}로 증가했습니다.");
    }
}
