using UnityEngine;

public class FlyEnemy : EnemyController
{
    [SerializeField] private GameObject coin;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask bossLayer;
    private float damagePoison;

    protected override void Awake()
    {
        base.Awake();
        coolDownAttack = 5f;
        timer = 5f;
        damageValue = 35f;
        damagePoison = 10f;
        timerRest = 0.5f;
        coolDownAttack = 2f;
        coin.SetActive(false);
    }

    protected override void Update()
    {
        base.Update();
    }

    private void Attack()
    {
        rb.gravityScale = 400f;
    }

    private void Flying()
    {
        rb.gravityScale = 0f;
    }

    private void DestroyFlyEnemy()
    {
        Destroy(gameObject);
        coin.transform.position = transform.position;
        coin.SetActive(true);
    }

    private void PoisonPlayer()
    {
        Collider2D[] poisonHit = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, playerLayer);
        if (poisonHit != null)
        {
            foreach (Collider2D target in poisonHit)
            {
                Entity entity = target.GetComponent<Entity>();
                if (entity != null)
                {
                    entity.TakeDamage(damagePoison);
                    PlayerController.instance.Poison();
                }
            }
        }
    }

    private void DamageBoss()
    {
        Collider2D[] hitBoss = Physics2D.OverlapCircleAll(attackPoint.position, attackDistance, bossLayer);
        if (hitBoss != null)
        {
            foreach (Collider2D hit in hitBoss)
            {
                Boss boss = hit.GetComponent<Boss>();
                if (boss != null)
                {
                    boss.timerStun = 0;
                }
            }
        }
    }

    public void Dead()
    {
        PoisonPlayer();
        DamageBoss();
    }
}
