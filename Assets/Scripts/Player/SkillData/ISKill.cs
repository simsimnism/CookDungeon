public interface ISkill
{
    void Initialize(SkillDataSO skillData); // SkillDataSO를 사용하여 스킬을 초기화
    void Activate(); // 스킬 발동 메서드
}
