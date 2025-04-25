using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Игрок
    public float smoothTime = 0.3f; // Скорость сглаживания
    public Vector3 offset = new Vector3(0, 2, -10); // Смещение (подними Y выше)
    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 targetPosition = target.position + offset;

        targetPosition.z = transform.position.z; // Фиксируем Z

        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
    }
}
