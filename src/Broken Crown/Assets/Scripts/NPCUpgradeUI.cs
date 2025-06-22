using UnityEngine;
using UnityEngine.UI;

public class NPCUpgradeUI : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject uiPanel;
    public GameObject interactHint;
    public GameObject dialogue;
    public Button healthButton;
    public Button staminaButton;
    public Button damageButton;
    public Button buyHealthPotionButton;
    public Button buyStaminaPotionButton;
    public Button buyRespawnPotionButton;

    [Header("Upgrade Costs and Values")]
    public int healthUpgradeCost;
    public int healthIncrease;

    public int staminaUpgradeCost;
    public int staminaIncrease;

    public int damageUpgradeCost;
    public int damageIncrease;

    [Header("Potion Costs")]
    public int healthPotionCost;
    public int staminaPotionCost;
    public int respawnPotionCost;

    private HeroKnight hero;
    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;
    private PotionManager potionManager;

    private bool playerInRange = false;

    private int x = 0;

    private void Start()
    {
        uiPanel.SetActive(false);
        interactHint.SetActive(false);

        healthButton.onClick.AddListener(() => UpgradeHealth());
        staminaButton.onClick.AddListener(() => UpgradeStamina());
        damageButton.onClick.AddListener(() => UpgradeDamage());
        buyHealthPotionButton.onClick.AddListener(() => BuyHealthPotion());
        buyStaminaPotionButton.onClick.AddListener(() => BuyStaminaPotion());
        buyRespawnPotionButton.onClick.AddListener(() => BuyRespawnPotion());
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            bool isActive = !uiPanel.activeSelf;
            uiPanel.SetActive(isActive);
            Time.timeScale = isActive ? 0f : 1f;
            interactHint.SetActive(!isActive);
            GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = !isActive;
        }

        if (uiPanel.activeSelf && Input.GetKeyDown(KeyCode.Escape))
        {
            uiPanel.SetActive(false);
            Time.timeScale = 0f;
            interactHint.SetActive(true);
            GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            hero = other.GetComponent<HeroKnight>();
            playerHealth = other.GetComponent<PlayerHealth>();
            playerStamina = other.GetComponent<PlayerStamina>();
            potionManager = other.GetComponent<PotionManager>();

            if (x == 0)
            {
                dialogue.SetActive(true);
            }

            if (hero != null && playerHealth != null && playerStamina != null && potionManager != null)
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
            dialogue.SetActive(false);
            Time.timeScale = 1f;
            x += 1;
        }
    }

    void UpgradeHealth()
    {
        if (hero.currentExperience >= healthUpgradeCost)
        {
            playerHealth.maxHealth += healthIncrease;
            playerHealth.health += healthIncrease;
            playerHealth.healthBar.SetMaxHealth(playerHealth.maxHealth);
            playerHealth.healthBar.SetHealth(playerHealth.health);
            hero.currentExperience -= healthUpgradeCost;
        }
    }

    void UpgradeStamina()
    {
        if (hero.currentExperience >= staminaUpgradeCost)
        {
            playerStamina.maxStamina += staminaIncrease;
            playerStamina.staminaBar.SetMaxStamina(playerStamina.maxStamina);
            hero.currentExperience -= staminaUpgradeCost;
        }
    }

    void UpgradeDamage()
    {
        if (hero.currentExperience >= damageUpgradeCost)
        {
            hero.attackDamage += damageIncrease;
            hero.currentExperience -= damageUpgradeCost;
        }
    }

    void BuyHealthPotion()
    {
        if (hero.currentExperience < healthPotionCost || potionManager.healthPotionCount >= potionManager.maxHealthPotions)
            return;

        potionManager.AddHealthPotion();
        hero.currentExperience -= healthPotionCost;
    }

    void BuyStaminaPotion()
    {
        if (hero.currentExperience < staminaPotionCost || potionManager.staminaPotionCount >= potionManager.maxStaminaPotions)
            return;

        potionManager.AddStaminaPotion();
        hero.currentExperience -= staminaPotionCost;
    }

    void BuyRespawnPotion()
    {
        if (hero.currentExperience < respawnPotionCost || potionManager.respawnPotionCount >= potionManager.maxRespawnPotions)
            return;

        potionManager.AddRespawnPotion();
        hero.currentExperience -= respawnPotionCost;
    }
}
