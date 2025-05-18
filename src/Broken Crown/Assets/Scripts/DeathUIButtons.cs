using UnityEngine;
using UnityEngine.UI;

public class DeathUIButtons : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public Button poisonButton;
    public Button notPoisonButton;
    public PotionManager potionManager;

    void Start()
    {
        UpdateButtons();
    }

    public void UpdateButtons()
    {
        bool hasRespawnPotion = potionManager.respawnPotionCount > 0;

        poisonButton.interactable = hasRespawnPotion;
        notPoisonButton.interactable = !hasRespawnPotion;
    }

    public void OnRespawnButtonPressed()
    {
        if (potionManager.respawnPotionCount > 0)
        {
            potionManager.respawnPotionCount--;
            playerHealth.RespawnOnDeathSpot();
            potionManager.UpdateUI();
            UpdateButtons();
        }
    }

    public void OnCheckpointRespawnPressed()
    {
        playerHealth.Respawn();
    }
}
