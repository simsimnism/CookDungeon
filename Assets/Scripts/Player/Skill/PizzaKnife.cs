using UnityEngine;

public class PizzaKnife : MonoBehaviour
{
    private float radius;
    private float rotationSpeed;
    private Transform player; // 플레이어 위치 참조
    private float currentAngle; // 현재 각도를 저장

    public void Initialize(float radius, float rotationSpeed, Transform playerTransform)
    {
        this.radius = radius;
        this.rotationSpeed = rotationSpeed;
        player = playerTransform; // 플레이어 참조를 저장
        Vector2 direction = ((Vector2)transform.position - (Vector2)player.position).normalized;
        currentAngle = Mathf.Atan2(direction.y, direction.x); // 초기 각도를 저장
    }

    void Update()
    {
        // 현재 각도 증가
        currentAngle += rotationSpeed * Mathf.Deg2Rad * Time.deltaTime;

        // 매 프레임 플레이어 위치를 중심으로 설정하고, 회전 위치 계산
        Vector2 centerPosition = player.position;
        Vector2 offset = new Vector2(Mathf.Cos(currentAngle), Mathf.Sin(currentAngle)) * radius;
        transform.position = (Vector3)(centerPosition + offset); // Vector3로 변환하여 설정
    }
}
