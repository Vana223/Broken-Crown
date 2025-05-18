using UnityEngine;

public class MonsterMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    public float stopChaseDistance;
    public float attackRange;

    public MonsterDamage monsterDamage;

    public Transform canvasRoot;

    private Animator animator;
    private Rigidbody2D rb;

    private float knockbackTimer = 0f;
    public float knockbackDuration;

    public int expAmount = 10;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (knockbackTimer > 0f)
        {
            knockbackTimer -= Time.deltaTime;
            return;
        }

        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);

        if (distanceToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);

            FacePlayer();

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
                    return;
                }

                MoveTowards(playerTransform.position);
            }
            else
            {
                if (distanceToPlayer < chaseDistance)
                {
                    isChasing = true;
                }
                else
                {
                    float distanceToStart = Vector2.Distance(transform.position, patrolPoints[0].position);
                    if (distanceToStart > 0.05f)
                    {
                        animator.SetBool("isWalking", true);
                        MoveTowards(patrolPoints[0].position);
                    }
                    else
                    {
                        transform.position = patrolPoints[0].position;
                        animator.SetBool("isWalking", false);
                    }
                }
            }
        }
    }

    void MoveTowards(Vector3 target)
    {
        Vector3 direction = (target - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (direction.x > 0)
            SetFacingDirection(-1f);
        else if (direction.x < 0)
            SetFacingDirection(1f);
    }

    void FacePlayer()
    {
        if (transform.position.x > playerTransform.position.x)
            SetFacingDirection(1f);
        else
            SetFacingDirection(-1f);
    }

    void SetFacingDirection(float xScale)
    {
        transform.localScale = new Vector3(xScale, 1f, 1f);

        if (canvasRoot != null)
        {
            canvasRoot.localScale = new Vector3(
                -xScale,
                1f,
                1f
            );
        }
    }

    public void ApplyKnockback()
    {
        knockbackTimer = knockbackDuration;
    }

    public void Die()
    {
        HeroKnight hero = playerTransform.GetComponent<HeroKnight>();
        if (hero != null)
        {
            hero.currentExperience += expAmount;
        }

        Destroy(gameObject);
    }
}
