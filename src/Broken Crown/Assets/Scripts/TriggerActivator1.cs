using UnityEngine;

public class TriggerActivator1 : MonoBehaviour
{
    public GameObject targetObject;
    public GameObject targetObject2;
    public string playerTag = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            targetObject.SetActive(true);
            targetObject2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            targetObject.SetActive(false);
            targetObject2.SetActive(false);
        }
    }
}
