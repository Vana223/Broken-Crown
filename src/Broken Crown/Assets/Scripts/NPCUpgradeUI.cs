using UnityEngine;
using UnityEngine.UI;

public class NPCUpgradeUI : MonoBehaviour
{
    public GameObject uiPanel;
    public GameObject interactHint;
    public Button healthButton;
    public Button staminaButton;
    public Button damageButton;

    private HeroKnight hero;
    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;

    public int healthIncrease;
    public int staminaIncrease;
    public int damageIncrease;
    public int expCost;

    private bool playerInRange = false;

    private void Start()
    {
        uiPanel.SetActive(false);
        interactHint.SetActive(false);

        healthButton.onClick.AddListener(() => UpgradeHealth());
        staminaButton.onClick.AddListener(() => UpgradeStamina());
        damageButton.onClick.AddListener(() => UpgradeDamage());
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            bool isActive = !uiPanel.activeSelf;
            uiPanel.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;
            interactHint.SetActive(!isActive);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hero = other.GetComponent<HeroKnight>();
            playerHealth = other.GetComponent<PlayerHealth>();
            playerStamina = other.GetComponent<PlayerStamina>();

            if (hero != null && playerHealth != null && playerStamina != null)
            {
                playerInRange = true;
                interactHint.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            uiPanel.SetActive(false);
            interactHint.SetActive(false);
            Time.timeScale = 1f;
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
