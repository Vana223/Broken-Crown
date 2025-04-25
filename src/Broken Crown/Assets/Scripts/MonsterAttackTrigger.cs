using UnityEngine;
using System.Collections;

public class MonsterAttackTrigger : MonoBehaviour
{
    private Animator animator;

    public MonsterMovement monsterMovement;

    private void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            monsterMovement.isChasing = false;
            animator.SetTrigger("attack");
        }

        else
        {
            monsterMovement.isChasing = true;
        }
    }
}
