using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // 몬스터 스포너를 연결합니다.
    private Animator animator;

    private bool isPlayerInRange = false; // 플레이어가 범위 안에 있는지 확인

    private float TimerDuration; // 타이머 시간
    private float RoundTimeLimit = 20f; // 라운드의 지속 시간
    private float FireTimeLimit = 30f; // 요리 가능한 시간
    private bool isStart = false; // 최초 상호작용을 했는지 판단
    private bool isFireON = false; // 라운드가 끝나고 요리를 할 수 있는지에 대한 확인
    private bool isFireOFF = false; // 최종적으로 모든 상호작용이 끝나고 불이 꺼짐
    private bool hasInteractedOnce = false; // 최초 상호작용 여부 플래그 추가

    private void Awake()
    {
        // MonsterSpawner 컴포넌트를 자동으로 찾기
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner를 찾을 수 없습니다. 씬에 추가되어 있는지 확인하세요.");
        }

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (!isFireOFF && isPlayerInRange && Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("상호작용 키 누름");
            Interact();
        }

        if (isFireOFF)
        {
            return;
        }

        // 최초 상호작용이 되었으면 타이머 작동
        if (isStart && !isFireON) // isFireON이 false일 때만 타이머 작동
        {
            TimerDuration -= Time.deltaTime;
            //Debug.Log($"라운드의 시간 : {TimerDuration}");

            // 타이머가 0보다 작으면 불을 켭니다.
            if (TimerDuration <= 0)
            {
                Debug.Log("불이 켜짐");
                FireON();
                return; // FireON이 실행되면 이후 코드는 실행되지 않음
            }
        }

        // 불이 켜져있으면 요리 가능 타이머 작동
        if (isFireON) // isFireON이 true일 때만 타이머 작동
        {
            TimerDuration -= Time.deltaTime;
            //Debug.Log($"불이 꺼지는 시간 : {TimerDuration}");

            // 타이머가 0보다 작으면 불을 끕니다.
            if (TimerDuration <= 0)
            {
                Debug.Log("불이 꺼짐");
                FireOFF();
                return; // FireOFF가 실행되면 이후 코드는 실행되지 않음
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(Settings.playerTag))
        {
            isPlayerInRange = true; // 플레이어가 범위에 들어옴
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Settings.playerTag))
        {
            isPlayerInRange = false; // 플레이어가 범위에서 나감
        }
    }

    void Interact()
    {
        
        // 불이 꺼져 있고, 최초 상호작용도 하지 않았으며, 한 번도 상호작용하지 않았다면 실행
        if (!isStart && !isFireOFF && !hasInteractedOnce)
        {
            Managers.Popup.OpenRound();
            // isStart를 true로 바꾸고 타이머를 설정
            isStart = true;
            hasInteractedOnce = true; // 상호작용 기록
            Timer(RoundTimeLimit);
            
            // 몬스터 생성 호출
            monsterSpawner.SpawnMonsters();
            Debug.Log("몬스터 생성 시작!");
        }
        else if (isFireON) // 이미 불이 켜져 있다면
        {
            // 요리 UI 출력
            Managers.Popup.ToggleCooking();

            // 인벤토리 UI 출력
            Managers.Inventory.OpenInventory();
        }
        else
        {
            Debug.Log("이미 상호작용했거나 조건이 맞지 않습니다.");
        }
    }


    public void clearCauldron()
    {
        if (Managers.GM.gameState == GameState.levelCompleted)
        {
            hasInteractedOnce = true;

        }
    }

    // 타이머 함수
    void Timer(float TimerSetting)
    {
        // 타이머 시작 로직
        TimerDuration = TimerSetting;
    }

    // 불이 켜질 때 기능
    void FireON()
    {
        // 불이 켜짐
        isFireON = true;
        Managers.Sound.PlaySFX(Define.SFX.Fire1);

        //매니저 연동
        Managers.GM.CookAbleTime = true;

        // 몬스터 삭제
        monsterSpawner.RemoveAllMonsters();
        Debug.Log("모든 몬스터가 삭제되었습니다.");

        // 불이 붙는 애니메이션 재생
        animator.SetBool("isFire", true);

        // 불 지속시간 세팅
        Timer(FireTimeLimit);
    }

    // 불이 꺼질 때 기능
    void FireOFF()
    {
        isFireOFF = true;
        Managers.Sound.StopSfx(Define.SFX.Fire1);

        //불이 켜져있을때 요리 가능하게 하는 함수(변수 연동GM) 
        Managers.GM.CookAbleTime = false;

        animator.SetBool("isFire", false);
    }

    // 요리 제한 시간을 반환하는 Get 메서드
    public float GetFireTimeLimit()
    {
        return FireTimeLimit;
    }

    // 남은 시간을 반환하는 Get 메서드
    public float GetRemainingTime()
    {
        return TimerDuration;
    }

    public bool IsCookingTime()
    {
        return isFireON; // 불이 켜져있으면 요리 제한 시간 상태
    }

    void OnDisable()
    {
        // 객체 비활성화 시 리소스 정리
    }
}
