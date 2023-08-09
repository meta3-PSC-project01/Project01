using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackData : MonoBehaviour
{
    //타입
    public EnemyAttackType type;
    
    //총알, 이펙트 들어감
    public SpriteRenderer attackSprite;
    public Animator animator;

    public bool isActive = false;

    //공격력
    public int damage;

    public void Awake()
    {
        Init();
    }

    public void Init()
    {
        attackSprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponentInChildren<Animator>();
    }

    public virtual void UseCollider()
    {
        isActive = true;
    }

    public virtual void UseEffect()
    {

        animator.SetTrigger("Attack");

    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && collision.tag == "Player")
        {
            isActive = false;
            collision.GetComponent<TestPlayer>().hp -= damage;
            //플레이어 반응 
            //player.Hit();
        }

    }

}


public enum EnemyAttackType
{
    RANGE,
    MELEE,
    NOTHING
}

public interface IAttackControl
{
    public void UseItem();
}