using UnityEngine;

public class NPC1 : MonoBehaviour
{
    public GameObject objectToToggle;
    public GameObject objectToToggle2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectToToggle.SetActive(true);
            objectToToggle2.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            objectToToggle.SetActive(false);
            objectToToggle2.SetActive(false);
        }
    }
}
