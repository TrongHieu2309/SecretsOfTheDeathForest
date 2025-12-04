using UnityEngine;

public class Mushroom : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        anim = GetComponent<Animator>();
        damageValue = 25f;
        timerRest = 0.7f;
        coolDownAttack = 2f;
    }

    protected override void DamagePlayer()
    {
        if (DetectPlayer())
        {
            if (PlayerController.instance != null && !PlayerController.instance.isBlocking)
            {
                PlayerController.instance.TakeDamage(damageValue);
            }
            if (PlayerController.instance.isBlocking)
            {
                PlayerController.instance.blocked = true;
                anim.SetTrigger("stun");
                Debug.Log("Blocked by Player");
            }
        }
    }
}
