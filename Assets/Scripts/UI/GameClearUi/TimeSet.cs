using UnityEngine;

public class TimeSet : MonoBehaviour
{
    private GameClearUI gameClearUI;

    private void Update()
    {
        // GameClearUI가 null일 때만 계속 찾아봄
        if (gameClearUI == null)
        {
            gameClearUI = FindObjectOfType<GameClearUI>();
            if (gameClearUI != null)
            {
                Debug.Log("GameClearUI를 성공적으로 찾았습니다.");
            }
        }

        // GameClearUI가 비활성화 상태일 때 시간 누적
        if (gameClearUI != null && !gameClearUI.gameObject.activeInHierarchy)
        {
            gameClearUI.AddInactiveTime(Time.deltaTime);
            Debug.Log("비활성화 시간 누적 중...");
        }
    }
}
