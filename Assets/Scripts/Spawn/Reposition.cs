using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//재배치 로직(각 방안의 몬스터를 재배치
public class Reposition : MonoBehaviour
{
    // 예시 값public Vector2 roomMinBounds = new Vector2(-10, -10); 
    //예시 값public Vector2 roomMaxBounds = new Vector2(10, 10);
    Collider2D coll;
    public Vector2 roomMinBounds; // 방의 최소 경계
    public Vector2 roomMaxBounds; // 방의 최대 경계

    void Awake()
    {
        // Collider2D 컴포넌트 가져오기
        coll = GetComponent<Collider2D>();
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        // 충돌한 오브젝트가 "Area" 태그를 가진 경우, 리턴하여 처리하지 않음
        if (collision.CompareTag("Area"))
            return;

        // 플레이어와 현재 오브젝트의 위치를 가져옴
        Vector3 playerPos = GameManager.Instance.Player.transform.position;
        Vector3 myPos = transform.position;

        // 플레이어의 이동 방향 벡터를 가져옴
        Vector3 playerDir = GameManager.Instance.Player.inputVec;

        // 태그에 따라 다른 재배치 로직 수행
        switch (transform.tag)
        {
            case "Ground":
                // "Ground" 태그의 오브젝트가 방 경계 내에서 재배치
                RepositionWithinRoom(playerDir, 40);
                break;
            case "Enemy":
                // "Enemy" 태그의 오브젝트가 Collider2D가 활성화되어 있는 경우, 방 경계 내에서 재배치
                if (coll.enabled)
                {
                    Vector3 randomOffset = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f), 0f);
                    RepositionWithinRoom(playerDir * 20 + randomOffset, 0);
                }
                break;
        }
    }

    // 방 경계 내에서 오브젝트 재배치
    void RepositionWithinRoom(Vector3 direction, float offset)
    {
        Vector3 newPos = transform.position + direction + new Vector3(offset, offset, 0);

        // 방 경계 내로 위치 제한
        newPos.x = Mathf.Clamp(newPos.x, roomMinBounds.x, roomMaxBounds.x);
        newPos.y = Mathf.Clamp(newPos.y, roomMinBounds.y, roomMaxBounds.y);

        transform.position = newPos;
    }
}