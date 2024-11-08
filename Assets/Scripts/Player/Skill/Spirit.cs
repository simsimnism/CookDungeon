using UnityEngine;

public class Spirit : MonoBehaviour
{
    public GameObject projectilePrefab;
    private float rotationSpeed;

    public void Initialize(GameObject projectilePrefab, float rotationSpeed)
    {
        this.projectilePrefab = projectilePrefab;
        this.rotationSpeed = rotationSpeed;
    }

    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
    }
}
