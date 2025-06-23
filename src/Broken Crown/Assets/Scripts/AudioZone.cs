using UnityEngine;

public class AudioZone : MonoBehaviour
{
    public string musicName;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.PlayMusic(musicName);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.FadeOutMusic(1f);
        }
    }
}
