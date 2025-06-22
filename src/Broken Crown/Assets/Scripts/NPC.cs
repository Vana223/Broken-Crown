using UnityEngine;

public class NPC : MonoBehaviour
{
    public GameObject objectToToggle;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectToToggle.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectToToggle.SetActive(false);
        }
    }
}
