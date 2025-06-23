using UnityEngine;

public class MorokDamage : MonoBehaviour
{
    public int attackDamage;
    public Vector3 attackOffset;
    public float attackRange;
    public LayerMask attackMask;
    public float attackCooldown;

    private float lastAttackTime = -Mathf.Infinity;

    public void Attack()
    {
        Vector3 pos = transform.position;
        pos += transform.right * attackOffset.x;
        pos += transform.up * attackOffset.y;

        Collider2D colInfo = Physics2D.OverlapCircle(pos, attackRange, attackMask);
        if (colInfo != null)
        {
            PlayerHealth playerHealth = colInfo.GetComponent<PlayerHealth>();
            if (playerHealth != null)
                playerHealth.TakeDamage(attackDamage);

            HeroKnight hero = colInfo.GetComponent<HeroKnight>();
            if (hero != null)
            {
                hero.KnockFromRight = transform.position.x > hero.transform.position.x;
                hero.KBCounter = hero.KBTotalTime;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Vector3 pos = transform.position;

        float direction = transform.localScale.x > 0 ? 1f : -1f;

        pos += Vector3.right * attackOffset.x * direction;
        pos += Vector3.up * attackOffset.y;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(pos, attackRange);
    }


    public float GetNextAttackTime()
    {
        return lastAttackTime + attackCooldown;
    }

    public void RegisterAttack()
    {
        lastAttackTime = Time.time;
    }
}
