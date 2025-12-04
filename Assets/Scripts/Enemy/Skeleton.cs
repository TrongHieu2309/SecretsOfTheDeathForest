using UnityEngine;

public class Skeleton : EnemyController
{
    private CircleCollider2D circleCollider;
    private BoxCollider2D boxCollider;

    protected override void Awake()
    {
        base.Awake();
        damageValue = 25f;
        timerRest = 0.7f;
        coolDownAttack = 2f;
        boxCollider = GetComponent<BoxCollider2D>();
        circleCollider = GetComponent<CircleCollider2D>();
    }

    protected override void Start()
    {
        base.Start();
        circleCollider.enabled = false;
        boxCollider.enabled = true;
        if (currentHp <= 0) anim.SetTrigger("dead");
    }

    private void Dead()
    {
        circleCollider.enabled = true;
        boxCollider.enabled = false;
        gameObject.tag = "Skull";
        gameObject.layer = LayerMask.NameToLayer("Skull");
        rb.gravityScale = 20f;
        rb.freezeRotation = false;
    }
}