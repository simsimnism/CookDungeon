using UnityEngine;

public class EggCabbageSpirit : MonoBehaviour
{
    public GameObject spiritPrefab;
    public float spawnInterval = 1f;
    public float rotationSpeed = 50f;
    public float radius = 3f; // 플레이어와의 최대 거리
    private GameObject spirit;

    public void ActivateSkill()
    {
        if (spirit == null)
        {
            spirit = Instantiate(spiritPrefab, transform.position, Quaternion.identity);
            spirit.GetComponent<Spirit>().Initialize(transform, radius); // 플레이어와의 거리 설정
        }
    }

    public void LevelUp()
    {
        rotationSpeed += 20f;
        spawnInterval = Mathf.Max(0.5f, spawnInterval - 0.1f);
    }
}
