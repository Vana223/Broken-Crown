using UnityEngine;
using System.Collections;

public class MorokHealth : MonoBehaviour
{
    public Animator animator;
    public MorokMovement morokMovement;

    public int maxHealth;
    int currentHealth;

    public HealthBar healthBar;

    private Rigidbody2D rb;
    public float knockbackForce;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        healthBar.SetMaxHealth(maxHealth);
        rb = GetComponent<Rigidbody2D>();
    }

    public void TakeDamage(int damage, Vector2 knockDirection)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        rb.linearVelocity = Vector2.zero;
        rb.AddForce(knockDirection * knockbackForce, ForceMode2D.Impulse);

        morokMovement.ApplyKnockback();

        if (currentHealth <= 0)
        {
            animator.SetBool("isDead", true);
        }

        healthBar.SetHealth(currentHealth);
    }
}
