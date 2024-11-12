using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class CandyAppleRoll : MonoBehaviour
{
    public GameObject candyAppleBallPrefab;
    private Transform playerTransform;
    private List<GameObject> candyAppleBalls = new List<GameObject>(); // 여러 개의 candyAppleBall을 관리하는 리스트
    private int skillLevel = 1; // 스킬 레벨
    private float damage = 10f; // 기본 데미지
    private float distance = 30f; // 거리
    private float speed = 3f; // 속도
    private float cooldown = 5f; // 기본 쿨다운 시간

    private void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;
    }

    public void ActivateSkill()
    {
        Vector3 spawnPosition = playerTransform.position;

        // 랜덤으로 방향 설정
        Vector2 randomDirection = Random.insideUnitCircle.normalized;
        Vector3 targetPosition = spawnPosition + (Vector3)randomDirection * distance; // 날아갈 거리

        // 거리와 속도를 기반으로 지속 시간 계산
        float duration = distance / speed;

        GameObject candyAppleBall = Instantiate(candyAppleBallPrefab, spawnPosition, Quaternion.identity);
        candyAppleBalls.Add(candyAppleBall); // 리스트에 추가

        // DOTween으로 랜덤 방향으로 이동
        candyAppleBall.transform.DOMove(targetPosition, duration)
            .SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                candyAppleBalls.Remove(candyAppleBall); // 리스트에서 제거
                Destroy(candyAppleBall); // 이동 완료 후 삭제
            });
    }

    public void LevelUp()
    {
        skillLevel++;
        damage += 5f; // 레벨업 시 데미지 증가
        cooldown = Mathf.Max(1f, cooldown - 0.5f); // 쿨다운 감소, 최소 1초까지 감소
        Debug.Log($"CandyAppleRoll 스킬이 레벨 {skillLevel}로 상승했습니다. 데미지: {damage}, 쿨다운: {cooldown}");
    }
}
