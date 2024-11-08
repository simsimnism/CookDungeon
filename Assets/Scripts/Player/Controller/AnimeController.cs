using System.Collections;
using UnityEngine;

public class AnimeController : MonoBehaviour
{

    private Animator animator;
    private Rigidbody2D rbody;
    public Transform playerTransform;

    private Vector2 attackDirection;
    private bool isAttacking = false;  // 공격 상태 체크

    void Awake()
    {
        isAttacking = false;
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
        if (!isAttacking)
        {
            PlayerAnime(); // 플레이어 애니메이션 갱신
        }
    }

    // 플레이어 애니메이션 처리
    private void PlayerAnime()
    {
        HandleMovement();   // 이동 애니메이션 처리
    }

    // 이동 애니메이션 처리 함수
    private void HandleMovement()
    {
        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.D)) moveHorizontal = -1f;

        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        if (direction != Vector2.zero && !isAttacking) // 공격 중일 때는 이동 애니메이션 중단
        {
            PlayWalkAnimation(direction);
        }
        else
        {
            ResetAllTriggers();
            animator.SetTrigger("Stand");  // 이동하지 않으면 스탠드 애니메이션 실행
        }
    }

    // 걷기 애니메이션 실행 함수
    private void PlayWalkAnimation(Vector2 direction)
    {
        ResetAllTriggers();

        if (direction.x < 0) // 왼쪽 이동
        {
            animator.SetTrigger("WalkLeft");
        }
        else if (direction.x > 0) // 오른쪽 이동
        {
            animator.SetTrigger("WalkRight");
        }
        else if (direction.y > 0) // 위쪽 이동
        {
            animator.SetTrigger("WalkUp");
        }
        else if (direction.y < 0) // 아래쪽 이동
        {
            animator.SetTrigger("WalkDown");
        }
    }

    private void ResetAllTriggers()
    {
        // 모든 트리거를 초기화하여 상태 전환을 원활하게 함
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkRight");
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("Stand");
    }
}
