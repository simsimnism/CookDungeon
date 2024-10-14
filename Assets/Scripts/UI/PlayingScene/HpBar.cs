using UnityEngine;
using UnityEngine.UI;

public class HpBar : MonoBehaviour
{
    [SerializeField] private Image hpBarFill; // 체력바로 사용할 이미지
    private int maxHealth;

    private void Awake()
    {
        // fillMethod를 Horizontal로 설정하여 가로 방향 채우기
        hpBarFill.type = Image.Type.Filled;
        hpBarFill.fillMethod = Image.FillMethod.Horizontal;
        hpBarFill.fillOrigin = (int)Image.OriginHorizontal.Left; // 왼쪽부터 채워지도록 설정
    }

    // 체력바 초기화
    public void Initialize(int MaxHP)
    {
        this.maxHealth = MaxHP;
        UpdateHealth(MaxHP); // 처음엔 체력 가득 채움
    }

    // 체력 업데이트
    public void UpdateHealth(int currentHP)
    {
        // 체력 비율을 계산하고, hpBarFill의 fillAmount를 조정
        float healthRatio = (float)currentHP / maxHealth;
        hpBarFill.fillAmount = healthRatio; // 이미지 크기 조정
    }
}
