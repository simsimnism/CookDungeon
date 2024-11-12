using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SkillChoice : MonoBehaviour
{
    private SkillManager skillManager;

    public Button skillSlot1Button;
    public Button skillSlot2Button;
    public Button skillSlot3Button;
    public Button skillSlot4Button;

    public TMP_Text skillSlot1LevelText;  // 슬롯 1의 레벨 표시 텍스트
    public TMP_Text skillSlot2LevelText;  // 슬롯 2의 레벨 표시 텍스트
    public TMP_Text skillSlot3LevelText;  // 슬롯 3의 레벨 표시 텍스트
    public TMP_Text skillSlot4LevelText;  // 슬롯 4의 레벨 표시 텍스트

    void Start()
    {
        // SkillManager를 자동으로 찾아서 연결
        skillManager = FindObjectOfType<SkillManager>();

        if (skillManager == null)
        {
            Debug.LogError("SkillManager가 씬에 존재하지 않습니다!");
            return;
        }

        // 각 스킬 슬롯에 대한 버튼 리스너를 추가합니다.
        skillSlot1Button.onClick.AddListener(() => OnSkillSlotClicked(1));
        skillSlot2Button.onClick.AddListener(() => OnSkillSlotClicked(2));
        skillSlot3Button.onClick.AddListener(() => OnSkillSlotClicked(3));
        skillSlot4Button.onClick.AddListener(() => OnSkillSlotClicked(4));

        // 초기 레벨 표시 업데이트
        UpdateSkillLevels();
    }

    void OnSkillSlotClicked(int skillIndex)
    {
        if (skillManager != null)
        {
            // 스킬을 활성화하고, 레벨업 처리
            skillManager.ActivateSkill(skillIndex);
            skillManager.LevelUpSkill(skillIndex);

            // 레벨 표시 업데이트
            UpdateSkillLevels();
        }
        Managers.Popup.CloseSkillUI();
    }

    public void UpdateSkillLevels()
    {
        // SkillManager에서 각 스킬의 레벨을 가져와 텍스트로 표시
        skillSlot1LevelText.text = "Lv. " + skillManager.GetSkillLevel(1).ToString();
        skillSlot2LevelText.text = "Lv. " + skillManager.GetSkillLevel(2).ToString();
        skillSlot3LevelText.text = "Lv. " + skillManager.GetSkillLevel(3).ToString();
        skillSlot4LevelText.text = "Lv. " + skillManager.GetSkillLevel(4).ToString();
    }
}
