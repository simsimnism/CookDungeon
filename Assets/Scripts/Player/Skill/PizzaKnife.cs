using UnityEngine;

public class PizzaKnife : MonoBehaviour
{
    private float radius;
    private float rotationSpeed;

    public void Initialize(float radius, float rotationSpeed)
    {
        this.radius = radius;
        this.rotationSpeed = rotationSpeed;
    }

    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        Vector3 offset = transform.right * radius;
        transform.position = transform.parent.position + offset;
    }
}
