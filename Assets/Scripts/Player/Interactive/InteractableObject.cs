using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    public Material outlineMaterial; // 새로 생성한 Outline 머티리얼

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originalMaterial = spriteRenderer.material;
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
