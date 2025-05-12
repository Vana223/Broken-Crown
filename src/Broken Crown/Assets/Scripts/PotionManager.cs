using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PotionManager : MonoBehaviour
{
    public int maxPotions = 5;
    public int healthPotionCount = 0;
    public int staminaPotionCount = 0;
    public int healAmount = 5;
    public int staminaAmount = 5;

    public GameObject healthFullImage;
    public GameObject healthEmptyImage;
    public TextMeshProUGUI healthPotionText;

    public GameObject staminaFullImage;
    public GameObject staminaEmptyImage;
    public TextMeshProUGUI staminaPotionText;

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
        if (Input.GetKeyDown(KeyCode.H))
        {
            TryUseHealthPotion();
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            TryUseStaminaPotion();
        }
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
        var current = playerStamina.GetCurrentStamina();
        if (staminaPotionCount > 0 && current < playerStamina.maxStamina)
        {
            playerStamina.RestoreStamina(staminaAmount);
            staminaPotionCount--;
            UpdateUI();
        }
    }

    void UpdateUI()
    {
        healthPotionText.text = healthPotionCount.ToString();
        healthFullImage.SetActive(healthPotionCount > 0);
        healthEmptyImage.SetActive(healthPotionCount == 0);

        staminaPotionText.text = staminaPotionCount.ToString();
        staminaFullImage.SetActive(staminaPotionCount > 0);
        staminaEmptyImage.SetActive(staminaPotionCount == 0);
    }
}
