using UnityEngine;
using UnityEngine.UI;

public class NPCUpgradeUI : MonoBehaviour
{
    public GameObject uiPanel;
    public Button healthButton;
    public Button staminaButton;
    public Button damageButton;

    private HeroKnight hero;
    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;

    public int healthIncrease = 5;
    public int staminaIncrease = 5;
    public int damageIncrease = 1;
    public int expCost = 1;

    private void Start()
    {
        uiPanel.SetActive(false);

        healthButton.onClick.AddListener(() => UpgradeHealth());
        staminaButton.onClick.AddListener(() => UpgradeStamina());
        damageButton.onClick.AddListener(() => UpgradeDamage());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hero = other.GetComponent<HeroKnight>();
            playerHealth = other.GetComponent<PlayerHealth>();
            playerStamina = other.GetComponent<PlayerStamina>();

            if (hero != null && playerHealth != null && playerStamina != null)
                uiPanel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            uiPanel.SetActive(false);
        }
    }

    void UpgradeHealth()
    {
        if (hero.currentExperience >= expCost)
        {
            playerHealth.maxHealth += healthIncrease;
            playerHealth.health += healthIncrease;
            playerHealth.healthBar.SetMaxHealth(playerHealth.maxHealth);
            playerHealth.healthBar.SetHealth(playerHealth.health);
            hero.currentExperience -= expCost;
        }
    }

    void UpgradeStamina()
    {
        if (hero.currentExperience >= expCost)
        {
            playerStamina.maxStamina += staminaIncrease;
            hero.currentExperience -= expCost;
            playerStamina.staminaBar.SetMaxStamina(playerStamina.maxStamina);
        }
    }

    void UpgradeDamage()
    {
        if (hero.currentExperience >= expCost)
        {
            hero.attackDamage += damageIncrease;
            hero.currentExperience -= expCost;
        }
    }
}
