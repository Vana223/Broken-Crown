using UnityEngine;

public class MorokMovement : MonoBehaviour
{
    public Transform[] patrolPoints;
    public float moveSpeed;

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;
    public float stopChaseDistance;
    public float attackRange;

    public Vector2 attackOffset;
    public Vector2 chaseOffset;
    public Vector2 stopChaseOffset;

    public Vector2 attack2Offset;
    public float attack2Radius = 1.5f;

    public MorokDamage morokDamage;
    public Transform canvasRoot;

    private Animator animator;
    private Rigidbody2D rb;

    private float knockbackTimer;
    public float knockbackDuration;

    public int expAmount;

    public GameObject objectToD;
    public GameObject targetO;

    private bool playerInAttack2Zone;

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

        float direction = GetFacingDirection();

        Vector3 attackOrigin = transform.position + new Vector3(attackOffset.x * direction, attackOffset.y, 0f);
        Vector3 chaseOrigin = transform.position + new Vector3(chaseOffset.x * direction, chaseOffset.y, 0f);
        Vector3 stopChaseOrigin = transform.position + new Vector3(stopChaseOffset.x * direction, stopChaseOffset.y, 0f);
        Vector3 attack2Origin = transform.position + new Vector3(attack2Offset.x * direction, attack2Offset.y, 0f);

        float distanceToPlayer = Vector2.Distance(chaseOrigin, playerTransform.position);
        float stopChaseDistanceToPlayer = Vector2.Distance(stopChaseOrigin, playerTransform.position);
        float attackDistanceToPlayer = Vector2.Distance(attackOrigin, playerTransform.position);
        float distanceToAttack2 = Vector2.Distance(attack2Origin, playerTransform.position);

        playerInAttack2Zone = distanceToAttack2 <= attack2Radius;

        if (playerInAttack2Zone && isChasing)
        {
            animator.SetTrigger("attack2");
        }

        if (attackDistanceToPlayer <= attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);

            FacePlayer();

            if (Time.time >= morokDamage.GetNextAttackTime())
            {
                animator.SetTrigger("attack");
                morokDamage.RegisterAttack();
            }
        }
        else
        {
            if (isChasing)
            {
                animator.SetBool("isWalking", true);

                if (stopChaseDistanceToPlayer > stopChaseDistance)
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
            canvasRoot.localScale = new Vector3(-xScale, 1f, 1f);
        }
    }

    float GetFacingDirection()
    {
        return transform.localScale.x > 0 ? 1f : -1f;
    }

    public void StopMovement()
    {
        moveSpeed = 0f;
    }

    public void ResumeMovement()
    {
        moveSpeed = 3f;
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

    void OnDrawGizmosSelected()
    {
        float direction = Application.isPlaying ? GetFacingDirection() : 1f;

        Vector3 attackPos = transform.position + new Vector3(attackOffset.x * direction, attackOffset.y, 0f);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos, attackRange);
        Gizmos.DrawSphere(attackPos, 0.05f);

        Vector3 chasePos = transform.position + new Vector3(chaseOffset.x * direction, chaseOffset.y, 0f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(chasePos, chaseDistance);
        Gizmos.DrawSphere(chasePos, 0.05f);

        Vector3 stopChasePos = transform.position + new Vector3(stopChaseOffset.x * direction, stopChaseOffset.y, 0f);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(stopChasePos, stopChaseDistance);
        Gizmos.DrawSphere(stopChasePos, 0.05f);

        Vector3 attack2Pos = transform.position + new Vector3(attack2Offset.x * direction, attack2Offset.y, 0f);
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(attack2Pos, attack2Radius);
        Gizmos.DrawSphere(attack2Pos, 0.05f);
    }

    public void DestroyTargetObject()
    {
        if (objectToD != null)
        {
            Destroy(objectToD);
        }
    }

    public void ActivateTargetObject()
    {
        if (targetO != null)
        {
            targetO.SetActive(true);
        }
    }

    public void DealDamageFromAnimationEvent()
    {
        if (!isChasing)
            return;

        float direction = GetFacingDirection();
        Vector3 attack2Origin = transform.position + new Vector3(attack2Offset.x * direction, attack2Offset.y, 0f);

        Collider2D hit = Physics2D.OverlapCircle(attack2Origin, attack2Radius, LayerMask.GetMask("Player"));
        if (hit != null)
        {
            PlayerHealth playerHealth = hit.GetComponent<PlayerHealth>();
            if (playerHealth != null && Time.time >= morokDamage.GetNextAttackTime())
            {
                playerHealth.TakeDamage(morokDamage.attackDamage);
                morokDamage.RegisterAttack();
            }
        }
    }
}
