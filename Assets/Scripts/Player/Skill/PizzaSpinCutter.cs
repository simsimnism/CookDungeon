using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PizzaSpinCutter : MonoBehaviour
{
    public GameObject pizzaKnifePrefab;
    private float radius = 2.5f;
    private float rotationSpeed = 200f;
    private float duration = 5f;
    private int knifeCount = 0; // 시작 칼 개수
    public int Level = 1;
    private int damage = 5; // 1레벨에서의 기본 데미지
    private List<GameObject> knives = new List<GameObject>();

    public void ActivateSkill()
    {
        SpawnKnives();
        Invoke(nameof(DestroyKnives), duration);
    }

    private void SpawnKnives()
    {
        DestroyKnives(); // 기존 칼 제거
        float angleStep = 360f / knifeCount; // 칼 사이의 각도 차

        for (int i = 0; i < knifeCount; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad; // 라디안으로 변환
            Vector3 position = new Vector3(Mathf.Cos(angle), Mathf.Sin(angle), 0) * radius; // 원형 배열 위치 계산
            GameObject knife = Instantiate(pizzaKnifePrefab, transform.position + position, Quaternion.identity);
            knife.transform.SetParent(transform); // 플레이어 기준으로 배치

            // 피자칼을 플레이어 주위로 회전하도록 초기화
            knife.AddComponent<PizzaKnife>().Initialize(radius, rotationSpeed, transform);

            knives.Add(knife);
        }
    }

    private void DestroyKnives()
    {
        foreach (GameObject knife in knives)
        {
            Destroy(knife);
        }
        knives.Clear();
    }

    public void LevelUp()
    {
        Level++;
        if (knifeCount < 5)
        {
            knifeCount += 2; // 레벨업 시 칼 개수 증가
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Monsters 태그를 가진 객체와 충돌한 경우에만 데미지 적용
        if (other.CompareTag("Monsters"))
        {
            // 몬스터에게 데미지를 입힘
            MonsterAI monster = other.GetComponent<MonsterAI>();
            if (monster != null)
            {
                // 충돌한 방향에 따라 데미지를 적용
                Vector3 hitDirection = (other.transform.position - transform.position).normalized;
                monster.TakeDamage(damage, hitDirection);

                Debug.Log($"몬스터에게 {damage}의 데미지를 입혔습니다.");
            }
        }
    }
}
