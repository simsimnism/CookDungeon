using UnityEngine;
using DG.Tweening;

public class EggCabbageSpirit : MonoBehaviour
{
    public GameObject projectilePrefab;
    public GameObject spiritPrefab;
    public float spawnInterval = 1f;
    public float rotationSpeed = 50f;
    public float duration = 10f;
    private GameObject spirit;

    public void ActivateSkill()
    {
        SpawnSpirit();
        InvokeRepeating(nameof(SpawnProjectile), 0f, spawnInterval);
        Invoke(nameof(EndSkill), duration);
    }

    private void SpawnSpirit()
    {
        if (spirit == null)
        {
            spirit = Instantiate(spiritPrefab, transform.position, Quaternion.identity);
            spirit.transform.SetParent(transform);
            spirit.transform.DOLocalRotate(new Vector3(0, 0, 360), rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart); // 지속 회전
        }
    }

    private void SpawnProjectile()
    {
        if (spirit != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, spirit.transform.position, Quaternion.identity);
            projectile.transform.DOScale(Vector3.one * 2, 0.5f); // 발사체 크기 증가 애니메이션
        }
    }

    private void EndSkill()
    {
        CancelInvoke(nameof(SpawnProjectile));
        if (spirit != null)
        {
            Destroy(spirit);
        }
    }

    public void LevelUp()
    {
        rotationSpeed += 20f;
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.1f);
    }
}

