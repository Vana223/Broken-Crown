using UnityEngine;
using System.Collections;

public class PotionPickup : MonoBehaviour
{
    public enum PotionType { Health, Stamina, Respawn }
    public PotionType potionType;

    public float pickupDelay;
    public int experienceOnFull;

    private bool playerInRange = false;
    private bool potionVisible = false;
    private bool potionCollected = false;
    private bool canBePickedUp = false;

    private PotionManager potionManager;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (spriteRenderer != null) spriteRenderer.enabled = false;
        if (animator != null) animator.enabled = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (!potionVisible)
            {
                spriteRenderer.enabled = true;
                animator.enabled = true;
                animator.SetTrigger("Appear");
                potionVisible = true;
                StartCoroutine(EnablePickupAfterDelay(pickupDelay));
            }
            else if (!potionCollected && canBePickedUp)
            {
                bool added = false;

                if (potionType == PotionType.Health && potionManager.healthPotionCount < potionManager.maxHealthPotions)
                {
                    potionManager.AddHealthPotion();
                    added = true;
                }
                else if (potionType == PotionType.Stamina && potionManager.staminaPotionCount < potionManager.maxStaminaPotions)
                {
                    potionManager.AddStaminaPotion();
                    added = true;
                }
                else if (potionType == PotionType.Respawn && potionManager.respawnPotionCount < potionManager.maxRespawnPotions)
                {
                    potionManager.AddRespawnPotion();
                    added = true;
                }

                if (!added)
                {
                    HeroKnight hero = potionManager.GetComponent<HeroKnight>();
                    if (hero != null) hero.currentExperience += experienceOnFull;
                }

                potionCollected = true;
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator EnablePickupAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        canBePickedUp = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            potionManager = other.GetComponent<PotionManager>();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
