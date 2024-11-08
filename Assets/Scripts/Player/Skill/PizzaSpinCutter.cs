using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;

public class PizzaSpinCutter : MonoBehaviour
{
    public GameObject pizzaKnifePrefab;
    public float radius = 2f;
    public float rotationSpeed = 200f;
    public float duration = 5f;
    private int knifeCount = 2;
    private List<GameObject> knives = new List<GameObject>();

    public void ActivateSkill()
    {
        SpawnKnives();
        Invoke(nameof(DestroyKnives), duration);
    }

    private void SpawnKnives()
    {
        DestroyKnives();
        float angleStep = 360f / knifeCount;
        for (int i = 0; i < knifeCount; i++)
        {
            float angle = i * angleStep;
            GameObject knife = Instantiate(pizzaKnifePrefab, transform.position, Quaternion.identity);
            knife.transform.SetParent(transform);
            knife.transform.localPosition = new Vector3(radius, 0, 0); // 반경에 맞춰 위치 설정
            knife.transform.DOLocalRotate(new Vector3(0, 0, 360), 1f / rotationSpeed, RotateMode.FastBeyond360)
                .SetLoops(-1, LoopType.Restart); // 지속 회전
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
        if (knifeCount < 5)
        {
            knifeCount += 2; // 레벨업 시 칼 개수 증가
        }
    }
}
