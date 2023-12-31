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

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && collision.tag == "Player")
        {
            isActive = false;
            collision.GetComponentInParent<PlayerMove>().playerHp -= damage;
            //플레이어 반응 
            collision.GetComponentInParent<PlayerMove>().Hit(damage, (int)transform.right.x);
        }

    }

}
