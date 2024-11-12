using UnityEngine;
using DG.Tweening;

public class AppleJuiceSkill : MonoBehaviour
{
    public GameObject appleJuiceEffect;
    public SkillManager skillManager;

    private float initialRadius = 0.5f;
    private float finalRadius = 1.5f;
    private float duration = 5f;
    private float damagePerSecond = 8f;

    public int Level = 1;
    private float cooldownReductionPerLevel = 0.5f;
    private float radiusIncreasePerLevel = 0.5f;
    private float damageIncreasePerLevel = 2f;
    private float durationIncreasePerLevel = 1f;

    public void ActivateSkill()
    {
        // 이 오브젝트를 부모로 하여 이펙트를 생성
        GameObject effect = Instantiate(appleJuiceEffect, transform.position, Quaternion.identity, this.transform);

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
        if (GameObject.FindGameObjectsWithTag("Monsters").Length == 0)
        {
            return;
        }

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, finalRadius);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Monsters"))
            {
                int damageAsInt = Mathf.RoundToInt(damagePerSecond);
                MonsterAI monsterAI = enemy.GetComponent<MonsterAI>();
                if (monsterAI != null)
                {
                    monsterAI.TakeDamage(damageAsInt, (enemy.transform.position - transform.position).normalized);
                    Debug.Log($"Monsters에게 {damageAsInt}의 데미지를 입혔습니다.");
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
