using UnityEngine;

public class Cauldron : MonoBehaviour
{
    public MonsterSpawner monsterSpawner; // 몬스터 스포너를 연결합니다.

    private bool isPlayerInRange = false; // 플레이어가 범위 안에 있는지 확인

    public float TimerDuration; // 타이머 시간
    public float RoundTimeLimit = 10f; // 라운드의 지속 시간
    public float FireTimeLimit = 5f; // 요리 가능한 시간
    private bool isStart = false; // 최초 상호작용을 했는지 판단
    private bool isFireON = false; // 라운드가 끝나고 요리를 할 수 있는지에 대한 확인
    private bool isFireOFF = false; // 최종적으로 모든 상호작용이 끝나고 불이 꺼짐

    private void Awake()
    {
        // MonsterSpawner 컴포넌트를 자동으로 찾기
        monsterSpawner = FindObjectOfType<MonsterSpawner>();
        if (monsterSpawner == null)
        {
            Debug.LogError("MonsterSpawner를 찾을 수 없습니다. 씬에 추가되어 있는지 확인하세요.");
        }
    }

    void Update()
    {
        if (!isFireOFF && isPlayerInRange && Input.GetKeyDown(KeyCode.E))
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

    // 상호작용
    void Interact()
    {
        // 만약 최초 상호작용을 하지 않았다면
        if (!isStart)
        {
            // isStart를 true로 바꾸고 타이머를 설정
            isStart = true;
            Timer(RoundTimeLimit);

            // 몬스터 생성 호출
            monsterSpawner.SpawnMonsters();
            Debug.Log("몬스터 생성 시작!");
        }
        else // 이미 한번 상호작용을 했다면
        {
            // 현재 불이 피워졌는지 확인하고 불이 켜져있다면
            if (isFireON) 
            {
                // 요리 UI 출력
            }
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

        // 불이 붙는 애니메이션 재생
        // PlayFireAnimation();

        // 불 지속시간 세팅
        Timer(FireTimeLimit);
    }

    // 불이 꺼질 때 기능
    void FireOFF()
    {
        isFireOFF = true;
        // 몬스터 삭제
    }

    void OnDisable()
    {
        // 객체 비활성화 시 리소스 정리
    }
}
