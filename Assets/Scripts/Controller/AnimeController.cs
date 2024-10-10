using UnityEngine;

public class AnimeController : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private float speed;
    private Animator animator;
    private SpriteRenderer rend;
    Rigidbody2D rbody;

    void Awake()
    {
        animator = GetComponent<Animator>();
        rbody = GetComponent<Rigidbody2D>();
        rend = GetComponent<SpriteRenderer>();
        speed = Managers.Player.moveSpeed;
        rend.flipX = true;
    }

    void Start()
    {
        PlayerAnime(); // 초기 애니메이션 설정
        ComboAttack();
    }

    void Update()
    {
        PlayerAnime(); // 매 프레임마다 애니메이션 갱신
        ComboAttack();
    }

    private void PlayerAnime()
    {
        if (!Managers.GM.IsMoving)
            return;

        // WASD 입력 값으로 이동 방향 계산
        float moveVertical = 0;
        float moveHorizontal = 0;

        if (Input.GetKey(KeyCode.W)) moveVertical = 1f;
        if (Input.GetKey(KeyCode.S)) moveVertical = -1f;
        if (Input.GetKey(KeyCode.A)) moveHorizontal = -1f;
        if (Input.GetKey(KeyCode.D)) moveHorizontal = 1f;

        // 이동 방향 계산
        Vector2 direction = new Vector2(moveHorizontal, moveVertical).normalized;

        // 대쉬 상태가 아닐 때 걷기 애니메이션 실행
        if (direction != Vector2.zero && !Managers.Player.isDashing)
        {
            PlayWalkAnimation(direction);
        }
        else if (direction == Vector2.zero)
        {
            // 정지 상태일 때 Stand 애니메이션
            ResetAllTriggers();
            animator.SetTrigger("Stand");
        }

        // 대쉬 시작
        if (Input.GetKeyDown(KeyCode.LeftShift) && direction != Vector2.zero)
        {
            // 대쉬 애니메이션 트리거
            PlayDashAnimation(direction);
        }
    }

    // 걷기 애니메이션 실행 함수
    private void PlayWalkAnimation(Vector2 direction)
    {
        ResetAllTriggers(); // 이전 트리거 초기화

        if (direction.x < 0) // 왼쪽
        {
            animator.SetTrigger("WalkLeft"); // 왼쪽으로 걷는 애니메이션 트리거
        }
        else if (direction.x > 0) // 오른쪽
        {
            animator.SetTrigger("WalkLeft"); // 오른쪽 애니메이션이 없으므로 왼쪽 애니메이션 재사용
            rend.flipX = false; // 오른쪽으로 반전
        }
        else if (direction.y > 0) // 위쪽
        {
            animator.SetTrigger("WalkUp");
        }
        else if (direction.y < 0) // 아래쪽
        {
            animator.SetTrigger("WalkDown");
        }
    }

    // 대쉬 애니메이션 실행 함수
    private void PlayDashAnimation(Vector2 direction)
    {
        ResetAllTriggers(); // 이전 트리거 초기화

        if (direction.x < 0) // 왼쪽 대쉬
        {
            animator.SetTrigger("LeftDash");
        }
        else if (direction.x > 0) // 오른쪽 대쉬
        {
            animator.SetTrigger("LeftDash"); // 오른쪽 애니메이션이 없으므로 왼쪽 애니메이션 재사용
            rend.flipX = false; // 오른쪽을 바라보도록 반전
        }
        else if (direction.y > 0) // 위쪽 대쉬
        {
            animator.SetTrigger("UpDash");
        }
        else if (direction.y < 0) // 아래쪽 대쉬
        {
            animator.SetTrigger("DownDash");
        }
    }

    // 트리거를 초기화하는 함수
    private void ResetAllTriggers()
    {
        animator.ResetTrigger("WalkLeft");
        animator.ResetTrigger("WalkUp");
        animator.ResetTrigger("WalkDown");
        animator.ResetTrigger("Stand");
        animator.ResetTrigger("LeftDash");
        animator.ResetTrigger("UpDash");
        animator.ResetTrigger("DownDash");
        animator.ResetTrigger("Attack1");
        animator.ResetTrigger("Attack2");
        // 필요에 따라 더 추가
    }

    //콤보 공격의 애니매이션
    void ComboAttack()
    {
        //스크린 월드 좌표에서 마우스 좌표를 가져옴
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //플레이어의 좌표를 가져옴
        Vector2 playerPos = transform.position;
        // 둘 사이의 간격을 계산
        Vector2 direction = (mousePos - playerPos).normalized;
        if (Managers.Player.comboStep == 1)
        {
            AttackTransform1(direction);
        }
        else if(Managers.Player.comboStep ==2)
        {
            AttackTransform2(direction);

        }
    }


    // 플레이어의 공격 상태 애니메이션
    private void AttackTransform1(Vector2 diretion)
    {
        //스크린 월드 좌표에서 마우스 좌표를 가져옴
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //플레이어의 좌표를 가져옴
        Vector2 playerPos = transform.position;
        // 둘 사이의 간격을 계산
        Vector2 direction = (mousePos - playerPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;



        //우측 공격모션
        if (direction.x > 0)
        {
            // 공격 시 다른 트리거 초기화
            rend.flipX = true;
            animator.SetTrigger("Attack1");

        }
        else if (direction.x < 0)
        {
            rend.flipX = false;

        }
        ResetAllTriggers();
    }
    private void AttackTransform2(Vector2 direction2)
    {
        //스크린 월드 좌표에서 마우스 좌표를 가져옴
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //플레이어의 좌표를 가져옴
        Vector2 playerPos = transform.position;
        // 둘 사이의 간격을 계산
        Vector2 direction = (mousePos - playerPos).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;


        if (direction.x > 0)
        {
            rend.flipX = true;
            animator.SetTrigger("Attack2");
        }
        else if (direction.x < 0)
        {
            rend.flipX = false;
        }
        ResetAllTriggers();

    }

}
