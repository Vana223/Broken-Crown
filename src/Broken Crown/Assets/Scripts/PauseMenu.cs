using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;

    public bool isPaused;

    void Start()
    {
        Debug.Log("PauseMenu started");
        pauseMenu.SetActive(false);
    }

    void Update()
    {
        if (Input.anyKeyDown)
            Debug.Log("Pressed some key");
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape pressed");
            if(isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Resume()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void Menu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Menu");
    }
}
