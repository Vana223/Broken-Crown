using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public Animator animator;

    public MonsterMovement monsterMovement;

    public int maxHealth = 10;
    int currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        animator.SetTrigger("hurt");

        if(currentHealth <= 0)
        {
            animator.SetBool("isDead", true);

            this.enabled = false;
            monsterMovement.enabled = false;
            monsterMovement.moveSpeed = 0;
        }
    }
}
