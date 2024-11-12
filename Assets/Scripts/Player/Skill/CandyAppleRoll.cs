using UnityEngine;
using DG.Tweening;

public class CandyAppleRoll : MonoBehaviour
{
    public GameObject candyAppleBallPrefab;
    private Transform playerTransform;
    private GameObject candyAppleBall;
    public int Level = 1; // 스킬 레벨
    private float damage = 10f; // 기본 데미지
    private float duration = 5f; // 지속 시간
    private float cooldown = 5f; // 기본 쿨다운 시간

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void ActivateSkill()
    {
        if (candyAppleBall == null)
        {
            Vector3 spawnPosition = playerTransform.position + (Vector3)Managers.Player.inputVec.normalized;
            candyAppleBall = Instantiate(candyAppleBallPrefab, spawnPosition, Quaternion.identity);

            // DOTween으로 플레이어 앞에 위치하게 설정
            candyAppleBall.transform.DOMove(playerTransform.position + (Vector3)Managers.Player.inputVec.normalized, duration)
                .SetEase(Ease.Linear)
                .SetLoops(-1, LoopType.Yoyo);

            Destroy(candyAppleBall, duration);
        }
    }

    public void LevelUp()
    {
        Level++;
        damage += 5f; // 레벨업 시 데미지 증가
        cooldown = Mathf.Max(1f, cooldown - 0.5f); // 쿨다운 감소, 최소 1초까지 감소
        Debug.Log($"CandyAppleRoll 스킬이 레벨 {Level}로 상승했습니다. 데미지: {damage}, 쿨다운: {cooldown}");
    }
}
