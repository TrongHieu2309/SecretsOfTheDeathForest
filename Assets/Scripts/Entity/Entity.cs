using UnityEngine;
using UnityEngine.UI;

public class Entity : MonoBehaviour
{
    [Header("MOVEMENT DETAILS")]
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float jumpForce;

    [Header("HEALTH POINT BAR")]
    [SerializeField] protected float maxHp;
    /* [HideInInspector] */ public float currentHp;
    [SerializeField] private Image healthPointUI;

    [Header("CHECK DETAILS")]
    [SerializeField] public float attackDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] private Transform wallCheck;
    [SerializeField] protected Transform attackPoint;
    [SerializeField] protected LayerMask groundLayer;
    [SerializeField] protected LayerMask skullLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] protected LayerMask targetLayer;
    [SerializeField] protected Vector2 groundCheckSize;
    [SerializeField] private Vector2 wallCheckSize;

    protected bool canMove = true;
    [HideInInspector] public bool isBlocking;
    [HideInInspector] public bool attacking = false;
    protected bool isWallJumping = false;
    protected bool isWallSliding = false;
    [HideInInspector] public bool isFacingRight = true;

    protected float horizontalInput;
    protected Rigidbody2D rb;
    protected Animator anim;
    protected BoxCollider2D boxCollider2D;
    protected float dir;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>();
        currentHp = maxHp;
    }

    protected virtual void Update()
    {
        OnGrounded();
        if (GameManager.instance.isPauseGame || GameManager.instance.isGameOver || GameManager.instance.isWinGame)
        {
            Time.timeScale = 0;
            return;
        }
        else
        {
            Time.timeScale = 1;
        }
        Animations();
        if (canMove && currentHp > 0 && attacking == false)
        {
            Movement();            
        }
        else rb.linearVelocity = Vector2.zero;

        // dir = EnemyController.instance.isFacingRight ? 1 : -1;
    }

    protected virtual void Animations()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(rb.linearVelocity.x));
    }

    protected void Flip(Transform transform)
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0, 180, 0);
    }

    public void TakeDamage(float damageValue)
    {
        currentHp -= damageValue;
        currentHp = Mathf.Max(currentHp, 0);
        UpdateHpBar();

        if (currentHp <= 0)
        {
            anim.SetTrigger("dead");
            HandleDead();
        }
        else
        {
            anim.SetTrigger("hurt");
            canMove = true;
        }
    }

    protected virtual void HandleDead()
    {
        canMove = false;
        boxCollider2D.enabled = false;
        rb.gravityScale = 0;
    }

    protected virtual void Movement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if (!isWallJumping)
        {
            rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
        }
    }

    protected void UpdateHpBar()
    {
        if (healthPointUI != null)
        {
            healthPointUI.fillAmount = currentHp / maxHp;
        }
    }

    #region Check Methods
    protected bool OnGrounded()
    {
        return Physics2D.OverlapBox(groundCheck.position, groundCheckSize, 0, groundLayer | skullLayer) != null;
    }

    protected bool IsWall()
    {
        return Physics2D.OverlapBox(wallCheck.position, wallCheckSize, 0, wallLayer) != null;
    }
    #endregion

    /*-----EVENT ANIMATION-----*/
    #region Event Animation
    private void EnableMovement() => canMove = true;
    private void DisableMovement() => canMove = false;
    private void StartAttack() => attacking = true;
    private void EndAttack() => attacking = false;
    #endregion

    protected virtual void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(groundCheck.position, groundCheckSize);
        Gizmos.DrawWireCube(wallCheck.position, wallCheckSize);
        Gizmos.DrawWireSphere(attackPoint.position, attackDistance);
    }
}