using UnityEngine;
using UnityEngine.UI;

public class CookingLimitBar : MonoBehaviour
{
    public Slider cookingTimerSlider;
    private Cauldron cauldron;

    private void Start()
    {
        // Cauldron 인스턴스를 찾습니다.
        cauldron = FindObjectOfType<Cauldron>();
        if (cauldron != null && cookingTimerSlider != null)
        {
            // 초기화 시점에 최대값을 설정합니다.
            UpdateSliderMaxValue();
            cookingTimerSlider.value = cauldron.GetRemainingTime();
        }
    }

    private void Update()
    {
        if (cauldron != null && cookingTimerSlider != null)
        {
            // 현재 요리 중인지에 따라 슬라이더의 최대값을 업데이트합니다.
            UpdateSliderMaxValue();

            // 타이머의 남은 시간을 슬라이더 값에 반영합니다.
            cookingTimerSlider.value = Mathf.Clamp(cauldron.GetRemainingTime(), 0, cookingTimerSlider.maxValue);
        }
    }

    // 슬라이더의 최대값을 요리 상태에 따라 업데이트하는 메서드
    private void UpdateSliderMaxValue()
    {
        if (cauldron.IsCookingTime())
        {
            // 요리 중일 때는 FireTimeLimit을 최대값으로 설정
            cookingTimerSlider.maxValue = cauldron.GetFireTimeLimit();
        }
        else
        {
        }
    }
}
