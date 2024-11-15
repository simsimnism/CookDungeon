using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening; // DOTween import

public class RoundInfo : MonoBehaviour
{
    [SerializeField] private GameObject numObject; // 자식 Num 오브젝트
    [SerializeField] private Sprite[] roundSprites; // 라운드 숫자 스프라이트 배열 (1~5)

    private Image numImage; // Num 오브젝트의 Image 컴포넌트
    private RectTransform rectTransform; // 이 오브젝트의 RectTransform

    private void Awake()
    {
        // Num 오브젝트의 Image 컴포넌트를 가져옵니다.
        numImage = numObject.GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
    }

    void OnEnable()
    {
        // 팝업이 활성화될 때 라운드 이미지를 변경
        UpdateRoundImage();

        // 왼쪽에서부터 나타나게 애니메이션 시작
        //AnimateIn();

        // 팝업이 활성화되면 5초 뒤에 자동으로 꺼지게 설정
        StartCoroutine(Round_Info(5f));
    }

    private IEnumerator Round_Info(float delay)
    {
        // 지정된 시간(5초) 동안 대기
        yield return new WaitForSeconds(delay);

        // 전체 UI를 오른쪽으로 사라지게 애니메이션 시작
        AnimateOut();

        // 팝업을 비활성화
        yield return new WaitForSeconds(1f); // 애니메이션이 끝날 시간을 기다리기
        Managers.Popup.CloseRoundInfo();
    }

    private void UpdateRoundImage()
    {
        // 현재 라운드에 맞는 스프라이트로 변경 (0~4 범위 체크)
        int currentRound = Managers.GM.currentDungeonLevelListIndex;

        if (currentRound >= 0 && currentRound < roundSprites.Length)
        {
            numImage.sprite = roundSprites[currentRound]; // 배열은 0부터 시작하므로
        }
        else
        {
            Debug.LogWarning("현재 라운드가 유효한 범위가 아닙니다.");
        }
    }

    private void AnimateIn()
    {
        // 왼쪽에서부터 나타나게 애니메이션
        numObject.transform.localPosition = new Vector3(-Screen.width, 0, 0); // 시작 위치를 화면 왼쪽으로 설정
        numObject.transform.DOLocalMoveX(5, 0.5f).SetEase(Ease.OutBounce); // 0 위치로 이동
    }

    private void AnimateOut()
    {
        // 전체 UI를 오른쪽으로 사라지게 애니메이션
        rectTransform.DOLocalMoveX(Screen.width, 0.5f).SetEase(Ease.InExpo); // 화면 오른쪽으로 이동
    }

    // 라운드 변경 함수 (다른 스크립트에서 호출할 수 있도록 public)
    public void ChangeRound(int newRound)
    {
        UpdateRoundImage();
    }
}
