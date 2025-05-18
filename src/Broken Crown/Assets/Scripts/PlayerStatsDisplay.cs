using UnityEngine;
using TMPro;

public class PlayerStatsDisplay : MonoBehaviour
{
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI staminaText;
    public TextMeshProUGUI damageText;
    public TextMeshProUGUI experienceText;

    private PlayerHealth playerHealth;
    private PlayerStamina playerStamina;
    private HeroKnight heroKnight;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");

        if (player != null)
        {
            playerHealth = player.GetComponent<PlayerHealth>();
            playerStamina = player.GetComponent<PlayerStamina>();
            heroKnight = player.GetComponent<HeroKnight>();
        }

        UpdateStatsUI();
    }

    void Update()
    {
        UpdateStatsUI();
    }

    void UpdateStatsUI()
    {
        if (playerHealth != null)
            healthText.text = playerHealth.maxHealth.ToString();

        if (playerStamina != null)
            staminaText.text = playerStamina.maxStamina.ToString();

        if (heroKnight != null)
        {
            damageText.text = heroKnight.attackDamage.ToString();
            experienceText.text = heroKnight.currentExperience.ToString();
        }
    }
}
