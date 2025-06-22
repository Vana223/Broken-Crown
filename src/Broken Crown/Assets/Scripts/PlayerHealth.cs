using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;
    private Animator animator;

    public HealthBar healthBar;
    public GameObject deathPanelUI;
    public GameObject deathPanelUI2;

    private Vector3 lastDeathPosition;
    private PlayerStamina stamina;

    void Start()
    {
        animator = GetComponent<Animator>();
        stamina = GetComponent<PlayerStamina>();
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);

        if (deathPanelUI != null)
            deathPanelUI.SetActive(false);
    }

    public void TakeDamage(int damage)
    {
        HeroKnight hero = GetComponent<HeroKnight>();

        if (hero != null && hero.IsBlocking && stamina != null)
        {
            if (stamina.UseStamina(2))
                return;
        }

        health -= damage;
        animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            animator.SetTrigger("Death");
            lastDeathPosition = transform.position;
        }

        healthBar.SetHealth(health);
    }

    private void Die()
    {
        if (deathPanelUI != null)
            deathPanelUI.SetActive(true);

        Time.timeScale = 0f;
        GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = false;
    }

    public void Respawn()
    {
        HeroKnight hero = GetComponent<HeroKnight>();

        if (hero != null)
        {
            hero.currentExperience = Mathf.Max(hero.currentExperience - 100, 0);
        }

        transform.position = CheckpointManager.Instance.GetCheckpointPosition();

        health = maxHealth;
        healthBar.SetHealth(health);

        if (stamina != null)
            stamina.RestoreStamina(stamina.maxStamina);

        if (deathPanelUI != null)
            deathPanelUI.SetActive(false);

        if (deathPanelUI2 != null)
            deathPanelUI2.SetActive(false);

        Time.timeScale = 1f;
        GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = true;
    }

    public void RespawnOnDeathSpot()
    {
        transform.position = lastDeathPosition;

        health = maxHealth;
        healthBar.SetHealth(health);

        if (stamina != null)
            stamina.RestoreStamina(stamina.maxStamina);

        if (deathPanelUI != null)
            deathPanelUI.SetActive(false);

        Time.timeScale = 1f;
        GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = true;
    }
    public void DisableHK()
    {
        GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = false;
    }
}
