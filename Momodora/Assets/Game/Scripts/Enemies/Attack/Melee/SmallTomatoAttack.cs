using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTomatoAttack : EnemyAttackData
{

    public override void UseEffect()
    {
        animator.SetTrigger("Attack");
    }
    public override void UseCollider()
    {
        isActive = true;
    }

}
