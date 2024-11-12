using UnityEngine;
using System.Collections;
using DG.Tweening;

public class Spirit : MonoBehaviour
{
    public GameObject projectilePrefab;
    private Transform player;
    private float radius;
    private Vector2 moveDirection;
    private int skillLevel = 1; // 스킬 레벨
    private float detectRange = 5f; // 탐지 범위
    private float shootInterval = 4f; // 발사 간격
    private float timer = 0f;

    public void Initialize(Transform playerTransform, float maxRadius)
    {
        player = playerTransform;
        radius = maxRadius;
        StartCoroutine(ChangeDirectionRoutine());
    }

    private IEnumerator ChangeDirectionRoutine()
    {
        while (true)
        {
            moveDirection = Random.insideUnitCircle.normalized;
            yield return new WaitForSeconds(1f);
        }
    }

    private void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + (Vector3)moveDirection * radius;
            transform.DOMove(targetPosition, 0.5f);

            if (Vector3.Distance(transform.position, player.position) > radius)
            {
                transform.position = player.position + (transform.position - player.position).normalized * radius;
            }

            timer += Time.deltaTime;
            if (timer >= shootInterval)
            {
                DetectAndShoot();
                timer = 0f;
            }
        }
    }

    private void DetectAndShoot()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, detectRange);

        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy.CompareTag("Monsters"))
            {
                ShootProjectile(enemy.transform);
                break;
            }
        }
    }

    private void ShootProjectile(Transform target)
    {
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<Projectile>().Initialize(target);
    }

    public void LevelUp()
    {
        skillLevel++;
        detectRange += 1f; // 탐지 범위 증가
        shootInterval = Mathf.Max(0.5f, shootInterval - 0.2f); // 발사 간격 감소, 최소 0.5초
        Debug.Log($"Spirit 스킬이 레벨 {skillLevel}로 상승했습니다. 탐지 범위: {detectRange}, 발사 간격: {shootInterval}");
    }
}


