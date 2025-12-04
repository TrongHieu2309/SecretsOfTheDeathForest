using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerController : Entity
{
    public static PlayerController instance;

    [Header("WALL JUMP DETAILS")]
    [SerializeField] private float wallSlideSpeed;
    [SerializeField] private float wallJumpForce;
    [SerializeField] private float wallJumpHorizontalForce;
    [SerializeField] private float wallJumpDuration;

    /*-----BLOCK-----*/
    [Header("BLOCK")]
    private bool canBlock;
    [SerializeField] private Image blockedCoolDown;
    [SerializeField] private Animator animShieldCooldown;
    private float blockCoolDown = 5f;
    public float blockTimer;
    [HideInInspector] public bool blocked;

    /*-----ATTACK COMBO-----*/
    private float currentCombo = 1;
    private float maxCombo = 3;
    private float comboTimer = 0;
    private float comboTiming = 0.6f;
    private bool readyAttack;

    /*-----SOUND MANAGEMENT-----*/
    [Header("Sound Management")]
    [SerializeField] private AudioClip sword;

    /*-----POISONING-----*/
    [Header("POISON")]
    [SerializeField] private GameObject poison;
    [SerializeField] private Image poisonImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private PlatformEffector2D platformEffector2D;
    [SerializeField] private AudioClip playerHurt;
    [SerializeField] private AudioClip playerJump;
    [SerializeField] private AudioClip landingSound;
    public float maxSkill;
    public float currentSkill;
    private float damageSkill = 1f;
    private float damagePoison = 5f;
    private float timerDamagePoison = 2f;
    private float coolDownPoison = 10f;
    public float timerPoison;
    private bool isPoisoning;
    private float dodgeSpeed = 15f;
    private float dodgeDuration = 0.5f;

    private CircleCollider2D circleCollider2D;
    private bool isDodge = false;
    private bool wasGrounded;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        circleCollider2D = GetComponent<CircleCollider2D>();
        circleCollider2D.enabled = false;
        blockTimer = 5f;
        poison.SetActive(false);
        currentSkill = maxSkill;
    }

    protected override void Update()
    {
        base.Update();

        if (blockTimer < blockCoolDown)
        {
            blockTimer += Time.deltaTime;
            animShieldCooldown.SetBool("shield", true);
        }
        else
        {
            animShieldCooldown.SetBool("shield", false);
        }

        HandleInput();
        WallSlide(wallSlideSpeed);
        HandleFlip();
        OnSetBlockCooldown();
        Combo();
        EnableAttack();
        UpdateBlockedCoolDown();
        Poisoning();
        UpdatePoisonUI();
    }

    void LateUpdate()
    {
        bool isGrounded = OnGrounded();

        if (isGrounded && !wasGrounded)
        {
            // Player vừa đáp đất
            SoundManager.instance.PlaySound(landingSound);
        }

        wasGrounded = isGrounded; // Cập nhật trạng thái cho frame tiếp theo
    }

    protected override void Animations()
    {
        anim.SetFloat("xVelocity", Mathf.Abs(horizontalInput));
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", OnGrounded());
        anim.SetBool("wallSlide", isWallSliding);
        anim.SetBool("blocked", blocked);
        // animShieldCooldown.SetBool("shield", blocked);
    }
    
    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (OnGrounded())
                Jump();
            else if (isWallSliding)
                WallJump();
        }

        if (Input.GetMouseButton(1) && canBlock == true)
        {
            canMove = false;
            readyAttack = false;
            if (!isBlocking)
            {
                isBlocking = true;
                Block(true);
            }
        }
        else
        {
            canMove = true;
            if (isBlocking)
            {
                isBlocking = false;
                Block(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            platformEffector2D.surfaceArc = 0;
        }
        else if (Input.GetKeyUp(KeyCode.S))
        {
            platformEffector2D.surfaceArc = 120;
        }
    }

    /*-----HANDLE EFFECT-----*/
    #region Handle Effect
    protected override void HandleDead()
    {
        base.HandleDead();
    }

    private void OnSetBlockCooldown()
    {
        if (blockTimer < blockCoolDown)
        {
            canBlock = false;
        }
        else 
            canBlock = true;
    }

    private void Block(bool block)
    {
        if (OnGrounded() && (horizontalInput != 0 || horizontalInput == 0))
        {
            anim.SetBool("block", block);
        }
    }
    #endregion

    /*-----ATTACK COMBO-----*/
    #region Attack Combo
    private void Combo()
    {
        comboTimer -= Time.deltaTime;

        if (Input.GetMouseButtonDown(0) && comboTimer < 0f && readyAttack == true)
        {
            anim.SetTrigger("attack" + currentCombo);
            comboTimer = comboTiming;
        }
        else if (Input.GetMouseButtonDown(0) && comboTimer > 0f)
        {
            currentCombo++;
            if (currentCombo > maxCombo) currentCombo = 1;

            anim.SetTrigger("attack" + currentCombo);
            comboTimer = comboTiming;
        }

        if (comboTimer < 0) currentCombo = 1;
    }

    private void EnableAttack()
    {
        bool isGrounded = OnGrounded();
        readyAttack = isGrounded && (circleCollider2D == null || !circleCollider2D.enabled);
        canBlock = isGrounded && (circleCollider2D == null || !circleCollider2D.enabled) && blockTimer >= blockCoolDown;
        if (Input.GetMouseButtonUp(1))
            readyAttack = true;
    }
    #endregion

    /*-----HANDLE MOVEMENT-----*/
    #region Handle Movement
    protected void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }

    protected void WallSlide(float wallSlideSpeed)
    {
        if (IsWall() && !OnGrounded() /* && horizontalInput != 0 */)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void WallJump()
    {
        isWallJumping = true;

        float direction = isFacingRight ? -1 : 1;
        rb.linearVelocity = Vector2.zero;

        Vector2 jumpDir = new Vector2(direction * wallJumpHorizontalForce, wallJumpForce);
        rb.AddForce(jumpDir, ForceMode2D.Impulse);

        if (isFacingRight && direction == -1 || !isFacingRight && direction == 1)
            Flip(transform);

        Invoke(nameof(StopWallJump), wallJumpDuration);
    }

    private void StopWallJump() => isWallJumping = false;

    // private void Dodge()
    // {
    //     if (!OnGrounded() || isDodge) return;

    //     if (Input.GetKeyDown(KeyCode.LeftControl))
    //     {
    //         isDodge = true;
    //         canMove = false;
    //         anim.SetTrigger("dodge");

    //         float direction = isFacingRight ? 1 : -1;
    //         rb.linearVelocity = new Vector2(direction * dodgeSpeed, rb.linearVelocity.y);

    //         StartDodge();
    //         Invoke(nameof(EndDodge), dodgeDuration);
    //     }
    // }

    // private void StartDodge()
    // {
    //     boxCollider2D.enabled = false;
    //     circleCollider2D.enabled = true;
    // }

    // private void EndDodge()
    // {
    //     boxCollider2D.enabled = true;
    //     circleCollider2D.enabled = false;
    //     isDodge = false;
    //     canMove = true;
    //     rb.linearVelocity = Vector2.zero;
    // }

    private void HandleFlip()
    {
        if (canMove == true)
        {
            if (isWallJumping) return;

            if (horizontalInput > 0 && !isFacingRight)
                Flip(transform);
            else if (horizontalInput < 0 && isFacingRight)
                Flip(transform);
        }
    }
    #endregion

    /*-----POISONING-----*/
    #region Poisoning
    public void Poison()
    {
        isPoisoning = true;
        poison.SetActive(true);
        timerPoison = coolDownPoison;
    }

    private void Poisoning()
    {
        if (isPoisoning)
        {
            timerDamagePoison -= Time.deltaTime;
            if (timerDamagePoison <= 0f)
            {
                if (currentSkill > 0f)
                {
                    currentSkill = Mathf.Max(currentSkill - damageSkill, 0f);
                    UpdateSkillBar();
                    timerDamagePoison = 2f;
                }
                else
                {
                    Entity entity = this.GetComponent<Entity>();
                    if (entity != null)
                        entity.TakeDamage(damagePoison);
                    timerDamagePoison = 2f;
                }
            }
            timerPoison -= Time.deltaTime;
        }

        if (timerPoison <= 0f)
        {
            isPoisoning = false;
            poison.SetActive(false);
        }
    }

    private void UpdatePoisonUI()
    {
        if (poisonImage != null)
        {
            poisonImage.fillAmount = timerPoison / coolDownPoison;
        }
    }

    public void UpdateSkillBar()
    {
        if (skillImage != null)
        {
            skillImage.fillAmount = currentSkill / maxSkill;
        }
    }
    #endregion

    /*-----EVENT ANIMATION-----*/
    #region Event Animation
    private void Blocked() => blocked = false;
    private void ResetTimerBlock() => blockTimer = 0f;

    private void DamageEnemy()
    {
        Collider2D[] enemyCollider = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, targetLayer);
        
        foreach (Collider2D enemies in enemyCollider)
        {
            if (enemies.CompareTag("Enemy"))
            {
                Entity entity = enemies.GetComponent<Entity>();
                if (entity != null)
                    entity.TakeDamage(5);
            }
        }
    }

    private void SwordSound() => SoundManager.instance.PlaySound(sword);
    private void HurtSound() => SoundManager.instance.PlaySound(playerHurt);
    private void JumpSound() => SoundManager.instance.PlaySound(playerJump);
    #endregion

    private void UpdateBlockedCoolDown()
    {
        if (blockedCoolDown != null)
            blockedCoolDown.fillAmount = blockTimer / blockCoolDown;
    }

    private void GameOver()
    {
        GameManager.instance.GameOverManagement();
    }
}
