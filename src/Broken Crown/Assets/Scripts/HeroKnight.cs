using UnityEngine;
using System.Collections;
using TMPro;

public class HeroKnight : MonoBehaviour
{
    [SerializeField] float m_speed;
    [SerializeField] float m_jumpForce;
    [SerializeField] private float sprintSpeedBonus;
    [SerializeField] private int staminaCostPerSecond;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] private GameObject spikeWindowUI;

    private Animator m_animator;
    private Rigidbody2D m_body2d;

    private bool m_grounded = false;
    private int m_facingDirection;
    private int m_currentAttack;
    private float m_timeSinceAttack;
    private float m_delayToIdle;
    private bool isAttacking = false;

    public Transform attackPoint;
    public float attackRange;
    public LayerMask enemyLayers;
    public int attackDamage;

    public float KBForce;
    public float KBCounter;
    public float KBTotalTime;
    public bool KnockFromRight;

    public bool IsBlocking { get; private set; }

    private PlayerStamina stamina;

    public int currentExperience;

    [SerializeField] private TextMeshProUGUI experienceText;

    private bool isSprinting = false;
    private float sprintStaminaTimer = 0f;

    private Vector3 attackPointInitialLocalPos;

    void Start()
    {
        m_animator = GetComponent<Animator>();
        m_body2d = GetComponent<Rigidbody2D>();
        stamina = GetComponent<PlayerStamina>();
        UpdateExperienceText();

        if (attackPoint != null)
            attackPointInitialLocalPos = attackPoint.localPosition;
    }

    void Update()
    {
        m_timeSinceAttack += Time.deltaTime;

        UpdateExperienceText();

        m_grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        m_animator.SetBool("Grounded", m_grounded);

        float inputX = Input.GetAxis("Horizontal");

        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            m_facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            m_facingDirection = -1;
        }

        if (Input.GetKey(KeyCode.LeftShift) && inputX != 0 && stamina.currentStamina >= staminaCostPerSecond)
        {
            isSprinting = true;
            sprintStaminaTimer += Time.deltaTime;

            if (sprintStaminaTimer >= 1f)
            {
                bool used = stamina.UseStamina(staminaCostPerSecond);
                if (!used)
                {
                    isSprinting = false;
                }
                sprintStaminaTimer = 0f;
            }
        }
        else
        {
            isSprinting = false;
            sprintStaminaTimer = 0f;
        }

        if (isAttacking)
        {
            m_body2d.linearVelocity = new Vector2(0, m_body2d.linearVelocity.y);
            return;
        }

        if (!m_animator.GetBool("IdleBlock"))
        {
            float currentSpeed = isSprinting ? m_speed + sprintSpeedBonus : m_speed;
            m_body2d.linearVelocity = new Vector2(inputX * currentSpeed, m_body2d.linearVelocity.y);

            if (Mathf.Abs(inputX) > 0.1f && m_grounded)
            {
                if (isSprinting)
                {
                    AudioManager.Instance.StopWalkLoop();
                    AudioManager.Instance.PlayRunLoop();
                }
                else
                {
                    AudioManager.Instance.StopRunLoop();
                    AudioManager.Instance.PlayWalkLoop();
                }
            }
            else
            {
                AudioManager.Instance.StopWalkLoop();
                AudioManager.Instance.StopRunLoop();
            }
        }
        else
        {
            m_body2d.linearVelocity = new Vector2(0, m_body2d.linearVelocity.y);
            AudioManager.Instance.StopWalkLoop();
            AudioManager.Instance.StopRunLoop();
        }

        m_animator.SetFloat("AirSpeedY", m_body2d.linearVelocity.y);

        if (Input.GetMouseButtonDown(0) && m_timeSinceAttack > 0.25f && !m_animator.GetBool("IdleBlock") && stamina.UseStamina(3))
        {
            m_currentAttack++;

            if (m_currentAttack > 3)
                m_currentAttack = 1;

            if (m_timeSinceAttack > 1.0f)
                m_currentAttack = 1;

            m_animator.SetTrigger("Attack" + m_currentAttack);
            isAttacking = true;
            StartCoroutine(StopAttackingAfterDelay(0.5f));

            AudioManager.Instance.Play("Sword");

            m_timeSinceAttack = 0.0f;

            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
            foreach (Collider2D enemy in hitEnemies)
            {
                Vector2 knockDir = (enemy.transform.position - transform.position).normalized;
                var enemyHealth = enemy.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(attackDamage, knockDir);
                    continue;
                }

                var morokHealth = enemy.GetComponent<MorokHealth>();
                if (morokHealth != null)
                {
                    morokHealth.TakeDamage(attackDamage, knockDir);
                }
            }
        }
        else if (Input.GetMouseButtonDown(1))
        {
            m_animator.SetTrigger("Block");
            m_animator.SetBool("IdleBlock", true);
            IsBlocking = true;

            AudioManager.Instance.Play("Shield");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            m_animator.SetBool("IdleBlock", false);
            IsBlocking = false;
        }
        else if (Input.GetKeyDown("space") && m_grounded && !m_animator.GetBool("IdleBlock"))
        {
            m_animator.SetTrigger("Jump");
            m_grounded = false;
            m_animator.SetBool("Grounded", m_grounded);
            m_body2d.linearVelocity = new Vector2(m_body2d.linearVelocity.x, m_jumpForce);
        }
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && !m_animator.GetBool("IdleBlock"))
        {
            m_delayToIdle = 0.05f;
            m_animator.SetInteger("AnimState", 1);
        }
        else
        {
            m_delayToIdle -= Time.deltaTime;
            if (m_delayToIdle < 0)
                m_animator.SetInteger("AnimState", 0);
        }

        if (KBCounter > 0)
        {
            KnockBack();
            return;
        }
    }

    void LateUpdate()
    {
        if (attackPoint != null)
        {
            attackPoint.localPosition = new Vector3(
                Mathf.Abs(attackPointInitialLocalPos.x) * m_facingDirection,
                attackPointInitialLocalPos.y,
                attackPointInitialLocalPos.z
            );
        }
    }

    void KnockBack()
    {
        float direction = KnockFromRight ? -1 : 1;
        float forceMultiplier = IsBlocking ? 0.3f : 1f;
        m_body2d.linearVelocity = new Vector2(direction * KBForce * forceMultiplier, KBForce * forceMultiplier);
        KBCounter -= Time.deltaTime;
    }

    public void ShowSpikeWindow()
    {
        if (spikeWindowUI != null)
        {
            spikeWindowUI.SetActive(true);
        }
        Time.timeScale = 0f;
        GameObject.Find("HeroKnight").GetComponent<HeroKnight>().enabled = false;
    }

    void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);

        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    private IEnumerator StopAttackingAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
    }

    public void AddExperience(int amount)
    {
        currentExperience += amount;
        UpdateExperienceText();
    }

    private void UpdateExperienceText()
    {
        if (experienceText != null)
        {
            experienceText.text = currentExperience.ToString();
        }
    }
}
