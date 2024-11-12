using UnityEngine;
using DG.Tweening;

public class AppleJuiceSkill : MonoBehaviour
{
    public GameObject appleJuiceEffect;
    public SkillManager skillManager;

    public float initialRadius = 1f;
    public float finalRadius = 3f;
    public float duration = 5f;
    public float damagePerSecond = 10f;

    public int Level = 1;
    private float cooldownReductionPerLevel = 0.5f;
    private float radiusIncreasePerLevel = 0.5f;
    private float damageIncreasePerLevel = 2f;
    private float durationIncreasePerLevel = 1f;

    public void ActivateSkill()
    {
        GameObject effect = Instantiate(appleJuiceEffect, transform.position, Quaternion.identity);

        effect.transform.localScale = Vector3.one * initialRadius;
        effect.transform.DOScale(Vector3.one * finalRadius, duration);

        SpriteRenderer sprite = effect.GetComponent<SpriteRenderer>();
        if (sprite != null)
        {
            sprite.DOFade(0f, duration).OnComplete(() => Destroy(effect));
        }
        else
        {
            Debug.LogWarning("SpriteRenderer가 AppleJuiceEffect에 없습니다. DOPFade 애니메이션이 생략됩니다.");
            Destroy(effect, duration);
        }

        InvokeRepeating(nameof(DamageEnemiesInRadius), 0f, 1f);
        Invoke(nameof(StopDamage), duration);
    }

    private void DamageEnemiesInRadius()
    {
        // Monster 태그가 존재하지 않거나 태그를 가진 오브젝트가 없으면 실행하지 않음
        if (GameObject.FindGameObjectsWithTag("Monsters").Length == 0)
        {
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, finalRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            // "Monster" 태그를 가진 오브젝트만 데미지 처리
            if (enemy.CompareTag("Monsters"))
            {
                int damageAsInt = Mathf.RoundToInt(damagePerSecond);
                MonsterAI monsterAI = enemy.GetComponent<MonsterAI>();
                if (monsterAI != null)
                {
                    monsterAI.TakeDamage(damageAsInt, (enemy.transform.position - transform.position).normalized);
                    Debug.Log($"Monsters에게 {damageAsInt}의 데미지를 입혔습니다."); // 데미지 로그 출력
                }
            }
        }
    }


    private void StopDamage()
    {
        CancelInvoke(nameof(DamageEnemiesInRadius));
    }

    public void LevelUp()
    {
        Level++;

        finalRadius += radiusIncreasePerLevel;
        damagePerSecond += damageIncreasePerLevel;
        duration += durationIncreasePerLevel;

        skillManager.ReduceCooldown(1, cooldownReductionPerLevel);
    }
}
