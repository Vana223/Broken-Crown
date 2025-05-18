using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    public int maxPotions = 3;

    public int healthPotionCount = 0;
    public int staminaPotionCount = 0;
    public int respawnPotionCount = 0;

    public int healAmount = 5;
    public int staminaAmount = 5;

    public GameObject healthFullImage, healthEmptyImage;
    public GameObject staminaFullImage, staminaEmptyImage;
    public GameObject respawnFullImage, respawnEmptyImage;

    public TextMeshProUGUI healthPotionText;
    public TextMeshProUGUI staminaPotionText;
    public TextMeshProUGUI respawnPotionText;

    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        playerStamina = GetComponent<PlayerStamina>();
        UpdateUI();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H)) TryUseHealthPotion();
        if (Input.GetKeyDown(KeyCode.G)) TryUseStaminaPotion();
    }

    public void AddHealthPotion()
    {
        if (healthPotionCount < maxPotions)
        {
            healthPotionCount++;
            UpdateUI();
        }
    }

    public void AddStaminaPotion()
    {
        if (staminaPotionCount < maxPotions)
        {
            staminaPotionCount++;
            UpdateUI();
        }
    }

    public void AddRespawnPotion()
    {
        if (respawnPotionCount < maxPotions)
        {
            respawnPotionCount++;
            UpdateUI();
        }
    }

    void TryUseHealthPotion()
    {
        if (healthPotionCount > 0 && playerHealth.health < playerHealth.maxHealth)
        {
            playerHealth.health = Mathf.Min(playerHealth.health + healAmount, playerHealth.maxHealth);
            playerHealth.healthBar.SetHealth(playerHealth.health);
            healthPotionCount--;
            UpdateUI();
        }
    }

    void TryUseStaminaPotion()
    {
        if (staminaPotionCount > 0 && playerStamina.GetCurrentStamina() < playerStamina.maxStamina)
        {
            playerStamina.RestoreStamina(staminaAmount);
            staminaPotionCount--;
            UpdateUI();
        }
    }

    public void UpdateUI()
    {
        healthPotionText.text = healthPotionCount.ToString();
        staminaPotionText.text = staminaPotionCount.ToString();
        respawnPotionText.text = respawnPotionCount.ToString();

        healthFullImage.SetActive(healthPotionCount > 0);
        healthEmptyImage.SetActive(healthPotionCount == 0);

        staminaFullImage.SetActive(staminaPotionCount > 0);
        staminaEmptyImage.SetActive(staminaPotionCount == 0);

        respawnFullImage.SetActive(respawnPotionCount > 0);
        respawnEmptyImage.SetActive(respawnPotionCount == 0);
    }
}
