using UnityEngine;

public class TriggerActivator : MonoBehaviour
{
    public GameObject targetObject;
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            targetObject.SetActive(true);
        }
    }
}
