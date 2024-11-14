using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractiveMasegge : MonoBehaviour
{
    public TextMeshProUGUI interactionText; // TextMeshPro를 사용하는 UI 텍스트

    private void Awake()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false); // 기본적으로 비활성화
    }

    public void ShowInteractionMessage(string message)
    {
        if (interactionText != null)
        {
            interactionText.text = $"F {message}";
            interactionText.gameObject.SetActive(true);
        }
    }

    // 상호작용 메시지 숨기기
    public void HideInteractionMessage()
    {
        if (interactionText != null)
            interactionText.gameObject.SetActive(false);
    }
}
