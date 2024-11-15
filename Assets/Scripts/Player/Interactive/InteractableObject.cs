using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    public Material outlineMaterial; // 새로 생성한 Outline 머티리얼

    // 상호작용 메시지
    public string interactionMessage;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
    }

    public string GetInteractionMessage()
    {
        // 태그에 따라 다른 상호작용 메시지를 반환
        switch (tag)
        {
            case "PickupItem":
                return "재료 줍기";
            case "Interaction":
                return "불 지피기 / 요리하기";
            default:
                return "알 수 없는 상호작용";
        }
    }

    public void Highlight(bool highlight)
    {
        if (highlight)
        {
            spriteRenderer.material = outlineMaterial; // 테두리 강조
        }
        else
        {
            spriteRenderer.material = originalMaterial; // 원래 머티리얼로 복구
        }
    }


    public void Interact()
    {
        Debug.Log($"{gameObject.name}와 상호작용을 수행했습니다.");
    }
}
