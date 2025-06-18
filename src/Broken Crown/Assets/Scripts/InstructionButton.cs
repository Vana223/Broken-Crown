using UnityEngine;

public class InstructionButton : MonoBehaviour
{
    [SerializeField] private GameObject instructionWindow;

    private bool isWindowOpen = false;

    public void ToggleInstructionWindow()
    {
        if (instructionWindow != null)
        {
            isWindowOpen = !isWindowOpen;
            instructionWindow.SetActive(isWindowOpen);
        }
    }

    private void Update()
    {
        if (isWindowOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            instructionWindow.SetActive(false);
            isWindowOpen = false;
        }
    }
}
