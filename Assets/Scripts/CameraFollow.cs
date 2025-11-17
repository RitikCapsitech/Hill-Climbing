using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;        
    public float distance = 3f;     
    public float height = 4f;       


    private Vector3 currentRotation;


    void LateUpdate()
    {
        if (!target) return;
        Vector3 direction = Quaternion.Euler(currentRotation) * Vector3.back * distance + Vector3.up * height;
        transform.position = target.position + direction;
    }
}
