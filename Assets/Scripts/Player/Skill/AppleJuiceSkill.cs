using UnityEngine;
using DG.Tweening;

public class AppleJuiceSkill : MonoBehaviour
{
    public GameObject appleJuiceEffect;
    public SkillManager skillManager; // SkillManager에 대한 참조 추가

    public float initialRadius = 1f;
    public float finalRadius = 3f;
    public float duration = 5f;
    public float damagePerSecond = 10f;

    // 추가: 스킬 레벨 관리 변수
    private int level = 1;
    private float cooldownReductionPerLevel = 0.5f;
    private float radiusIncreasePerLevel = 0.5f;
    private float damageIncreasePerLevel = 2f;
    private float durationIncreasePerLevel = 1f;

    public void ActivateSkill()
    {
        GameObject effect = Instantiate(appleJuiceEffect, transform.position, Quaternion.identity);

        // 반경 증가 애니메이션
        effect.transform.localScale = Vector3.one * initialRadius;
        effect.transform.DOScale(Vector3.one * finalRadius, duration);

        // 투명도 애니메이션
        SpriteRenderer sprite = effect.GetComponent<SpriteRenderer>();
        sprite.DOFade(0f, duration).OnComplete(() => Destroy(effect));

        // 범위 내 적들에게 데미지 주기
        InvokeRepeating(nameof(DamageEnemiesInRadius), 0f, 1f);
        Invoke(nameof(StopDamage), duration);
    }

    private void DamageEnemiesInRadius()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, finalRadius);
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Monster"))
            {
                int damageAsInt = Mathf.RoundToInt(damagePerSecond);
                enemy.GetComponent<MonsterAI>()?.TakeDamage(damageAsInt, (enemy.transform.position - transform.position).normalized);
            }
        }
    }

    private void StopDamage()
    {
        CancelInvoke(nameof(DamageEnemiesInRadius));
    }

    // 추가: 레벨업 메서드
    public void LevelUp()
    {
        level++;

        // 레벨업 시 강화
        finalRadius += radiusIncreasePerLevel;         // 반경 증가
        damagePerSecond += damageIncreasePerLevel;     // 초당 데미지 증가
        duration += durationIncreasePerLevel;          // 지속시간 증가

        // 쿨타임 감소는 SkillManager에서 적용
        skillManager.ReduceCooldown(1, cooldownReductionPerLevel);
    }
}
