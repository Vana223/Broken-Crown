using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenePortal1 : MonoBehaviour
{
    public string sceneName = "Menu";

    public void ToMenu()
    {
        SceneManager.LoadScene(sceneName);
    }

}
