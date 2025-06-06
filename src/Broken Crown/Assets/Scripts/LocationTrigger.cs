using UnityEngine;

public class LocationTrigger : MonoBehaviour
{
    public GameObject objectToDisable;

    public GameObject objectToEnable;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (objectToDisable != null)
                objectToDisable.SetActive(false);

            if (objectToEnable != null)
                objectToEnable.SetActive(true);
        }
    }
}
