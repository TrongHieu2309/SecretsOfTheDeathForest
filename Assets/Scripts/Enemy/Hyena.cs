using UnityEngine;

public class Hyena : EnemyController
{
    protected override void Awake()
    {
        base.Awake();
        damageValue = 20f;
        timerRest = .7f;
        coolDownAttack = 2f;
    }
}
