using UnityEngine;
using DG.Tweening;

public class CandyAppleRoll : MonoBehaviour
{
    public GameObject candyApplePrefab;
    public float speed = 8f;
    private GameObject candyApple;
    private int level = 1;

    public void ActivateSkill()
    {
        if (candyApple == null)
        {
            candyApple = Instantiate(candyApplePrefab, transform.position, Quaternion.identity);
            candyApple.transform.SetParent(transform);
        }
    }

    void Update()
    {
        if (candyApple != null)
        {
            // 플레이어의 앞에 고정되도록 부드럽게 이동
            candyApple.transform.DOMove(transform.position + transform.right * 1.5f, 0.1f).SetEase(Ease.Linear);
        }
    }

    public void LevelUp()
    {
        level++;
        speed += 2f;
        candyApple.transform.DOScale(candyApple.transform.localScale + Vector3.one * 0.2f, 0.2f); // 크기 증가 애니메이션
    }
}
