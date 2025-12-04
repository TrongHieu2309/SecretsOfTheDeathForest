using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyController : Entity
{
    public static EnemyController instance;

    [Header("MOVEMENT DETAILS")]
    [SerializeField] private Transform positionA;
    [SerializeField] private Transform positionB;
    [SerializeField] private GameObject hpBar;
    [SerializeField] protected Vector2 attackDistanceEnemy;
    [SerializeField] private AudioClip playerHurt;

    private Vector2 targetPosition;
    private bool moveToA = true;
    private bool isChangeDir;
    protected float coolDownAttack;
    public float timer;
    public bool blocked {get; private set;}
    public float damageValue;
    protected float timerRest;

    protected override void Awake()
    {
        base.Awake();
        instance = this;
    }
    protected virtual void Start()
    {
        targetPosition = positionA.position;
    }

    protected override void Update()
    {
        base.Update();
        HandleAttack();
    }

    /*-----ENEMY MOVEMENT-----*/
    #region Enemy Movement
    protected override void Movement()
    {
        if (isChangeDir) return;

        Vector2 direction = (targetPosition - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        if (Vector2.Distance(targetPosition, transform.position) < 0.5f && !isChangeDir)
        {
            StartCoroutine(nameof(ChangePos));
        }

        if (canMove == true && !isChangeDir)
        {
            if ((direction.x > 0) && !isFacingRight)
                Flip(transform);
            else if (direction.x < 0 && isFacingRight)
                Flip(transform);
        }
    }

    private IEnumerator ChangePos()
    {
        isChangeDir = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(timerRest);

        moveToA = !moveToA;
        targetPosition = moveToA ? positionA.position : positionB.position;
        isChangeDir = false;
    }
    #endregion
    
    /*-----DETECT TARGET & ATTACK-----*/
    #region Attack target
    protected virtual Collider2D DetectPlayer()
    {
        if (base.attackPoint == null && targetLayer.value == 0) return null;

        var hits = Physics2D.OverlapBoxAll(base.attackPoint.position, attackDistanceEnemy, 0, targetLayer);

        foreach (var hit in hits)
            if (hit != null && hit.CompareTag("Player"))
                return hit;

        return null;
    }

    protected virtual void HandleAttack()
    {
        timer += Time.deltaTime;
        if (DetectPlayer() && timer >= coolDownAttack) 
        {
            anim.SetTrigger("attack");
            timer = 0;
        }
    }
    #endregion

    /*-----EVENT ANIMATION-----*/
    #region Event Animation
    protected virtual void DamagePlayer()
    {
        if (DetectPlayer())
        {
            if (PlayerController.instance != null && !PlayerController.instance.isBlocking)
            {
                PlayerController.instance.TakeDamage(damageValue);
                SoundManager.instance.PlaySound(playerHurt);
            }
            if (PlayerController.instance.isBlocking)
            {
                PlayerController.instance.blocked = true;
                Debug.Log("Blocked by Player");
            }   
        }
    }

    private void DestroyHpBar()
    {
        Destroy(hpBar);
    }
    #endregion

    protected override void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(base.attackPoint.position, attackDistanceEnemy);
        Gizmos.DrawWireSphere(base.attackPoint.position, attackDistance);
    }
}