using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int health;

    private Animator animator;

    public HealthBar healthBar;

    void Start()
    {
        animator = GetComponent<Animator>();
        health = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }
    
    public void TakeDamage(int damage)
    {
        health -= damage;
        animator.SetTrigger("Hurt");

        if(health <= 0)
        {
            animator.SetTrigger("Death");
        }

        healthBar.SetHealth(health);
    }
}
