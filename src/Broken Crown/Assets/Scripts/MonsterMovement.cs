using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;
    public int patrolDestination;

    public Transform playerTransform;
    public bool isChasing;
    public bool isPatrol;
    public float chaseDistance;
    public float stopChaseDistance;
    public float attackRange;

    public MonsterDamage monsterDamage;

    private Animator animator;
    private Rigidbody2D rb;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            rb.velocity = Vector2.zero;

            if (Time.time >= monsterDamage.GetNextAttackTime())
            {
                animator.SetTrigger("attack");
                monsterDamage.RegisterAttack();
            }
        }
        else
        {
            if (isChasing)
            {
                animator.SetBool("isWalking", true);

                if (distanceToPlayer > stopChaseDistance)
                {
                    isChasing = false;
                    animator.SetBool("isWalking", false);
                    return;
                }

                if (transform.position.x > playerTransform.position.x)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                    transform.position += Vector3.left * moveSpeed * Time.deltaTime;
                }

                if (transform.position.x < playerTransform.position.x)
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                    transform.position += Vector3.right * moveSpeed * Time.deltaTime;
                }

                if (distanceToPlayer <= attackRange)
                {
                    if (Time.time >= monsterDamage.GetNextAttackTime())
                    {
                        isChasing = false;
                        isPatrol = false;
                        animator.SetBool("isWalking", false);
                        animator.SetTrigger("attack");
                        monsterDamage.RegisterAttack();
                    }
                }
            }
            else
            {
                animator.SetBool("isWalking", true);

                if (distanceToPlayer < chaseDistance)
                {
                    isChasing = true;
                }
                else
                {
                    isPatrol = true;
                }

                if (isPatrol)
                {
                    if (patrolDestination == 0)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[0].position, moveSpeed * Time.deltaTime);
                        if (Vector2.Distance(transform.position, patrolPoints[0].position) < .2f)
                        {
                            transform.localScale = new Vector3(-1, 1, 1);
                            patrolDestination = 1;
                        }
                    }

                    if (patrolDestination == 1)
                    {
                        transform.position = Vector2.MoveTowards(transform.position, patrolPoints[1].position, moveSpeed * Time.deltaTime);
                        if (Vector2.Distance(transform.position, patrolPoints[1].position) < .2f)
                        {
                            transform.localScale = new Vector3(1, 1, 1);
                            patrolDestination = 0;
                        }
                    }
                }
            }
        }
    }

    void Destroy()
    {
        Destroy(gameObject);
    }
}
