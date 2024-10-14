using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //밑에 두개는 다른데 구현하고 싶으면 되긴함
    public float speed; // 총알 속도
    public float damage; // 총알 데미지
    private Vector2 direction; // 발사 방향

    // 총알을 발사할 때 방향 설정
    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;
    }

    void Update()
    {
        // 총알을 설정된 방향으로 이동
        transform.Translate(direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // 플레이어와 충돌했을 때
        if (collision.CompareTag("Player"))
        {
            // 플레이어에게 데미지 주기 (플레이어가 데미지를 받는 로직은 따로 구현해야 함)
            Debug.Log("HIT");

            DestroyBullet();
        }

        // 장애물이나 다른 물체와 충돌했을 때도 총알을 파괴
        if (collision.CompareTag("여기에 벽과 관련된 테그 적어용 태그가 뭔지 몰라서"))
        {
            DestroyBullet();
        }
    }
    void DestroyBullet() // 총알 파괴
    {
        Destroy(gameObject);
    }
}
