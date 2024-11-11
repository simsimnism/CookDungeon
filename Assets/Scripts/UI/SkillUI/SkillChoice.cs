using UnityEngine;
using UnityEngine.UI;

public class SkillSelectUI : MonoBehaviour
{
    public GameObject skillSelectionPanel;  // UI 전체 패널
    public Button skillButton1;  // 첫 번째 스킬 버튼
    public Button skillButton2;  // 두 번째 스킬 버튼
    public Button skillButton3;  // 세 번째 스킬 버튼
    public Button skillButton4;  // 네 번째 스킬 버튼

    private void Start()
    {
        // 각 버튼에 클릭 이벤트 추가
        skillButton1.onClick.AddListener(() => OnSkillSelected(1));
        skillButton2.onClick.AddListener(() => OnSkillSelected(2));
        skillButton3.onClick.AddListener(() => OnSkillSelected(3));
        skillButton4.onClick.AddListener(() => OnSkillSelected(4));
    }

    private void OnSkillSelected(int skillIndex)
    {
        Debug.Log($"Skill {skillIndex} 선택됨");

        // 여기서 선택된 스킬에 따른 로직 추가 가능 (예: 스킬 활성화 등)

        // 선택 후 UI 창을 닫음
        Managers.Popup.CloseSkillUI();
    }


}
