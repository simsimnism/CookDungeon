using UnityEngine;

public class CandyAppleBall : MonoBehaviour
{
    public int damage = 4; // 데미지 값

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Monsters 태그가 붙은 객체와 충돌할 경우에만 데미지 적용
        if (other.CompareTag("Monsters"))
        {
            MonsterAI monster = other.GetComponent<MonsterAI>();
            if (monster != null)
            {
                // 데미지를 입히고 로그 출력
                monster.TakeDamage(damage, (other.transform.position - transform.position).normalized);
                Debug.Log($"몬스터에게 {damage}의 데미지를 입혔습니다.");

                // 사과 공은 한 번 충돌 후 제거
                Destroy(gameObject);
            }
        }
    }
}
