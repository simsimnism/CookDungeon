using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PizzaSpinCutter : MonoBehaviour
{
    public GameObject pizzaKnifePrefab;
    public float radius = 2f;
    public float rotationSpeed = 200f;
    public float duration = 5f;
    public int knifeCount = 2; // 시작 칼 개수
    public int Level = 1;
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
}
