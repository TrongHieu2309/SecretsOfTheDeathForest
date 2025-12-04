using UnityEngine;

public class Scorpion : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        damageValue = 10f;
        timerRest = .5f;
        coolDownAttack = 2f;
    }

    protected override void DamagePlayer()
    {
        if (DetectPlayer())
        {
            if (PlayerController.instance != null && !PlayerController.instance.isBlocking)
            {
                PlayerController.instance.TakeDamage(damageValue);
                PlayerController.instance.Poison();
            }
            if (PlayerController.instance.isBlocking)
            {
                PlayerController.instance.blocked = true;
                Debug.Log("Blocked by Player");
            }  
        }
    }
}
