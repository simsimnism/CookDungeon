using UnityEngine;

public class PizzaKnife : MonoBehaviour
{
    private float radius;
    private float rotationSpeed;
    private Transform player;

    public void Initialize(float radius, float rotationSpeed, Transform playerTransform)
    {
        this.radius = radius;
        this.rotationSpeed = rotationSpeed;
        player = playerTransform;
    }

    void Update()
    {
        if (player != null)
        {
            // 플레이어를 중심으로 회전
            transform.RotateAround(player.position, Vector3.forward, rotationSpeed * Time.deltaTime);

            // 반경을 유지하며 이동
            Vector3 offset = (transform.position - player.position).normalized * radius;
            transform.position = player.position + offset;
        }
    }
}
