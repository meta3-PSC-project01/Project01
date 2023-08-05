using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommon_Legacy : MonoBehaviour
{
    //에너미 기본 컴포넌트
    public Rigidbody2D enemyRigidbody;
    public SpriteRenderer enemyRenderer;
    public BoxCollider2D enemyCollider;
    public Animator enemyAnimator;

    //공격관련 컴포넌트
    public EnemyWeapon_Legacy attackObject;    //공격 오브젝트(근거리/원거리 나눠짐)
    public Transform attackPosition;    //공격 생성 위치

    //탐지 컴포넌트
    public Transform targetObject;

    //에너미 속성
    public EnemyDirection_Legacy enemyDirection = default; //방향

    public int enemyHp = default;
    public float enemySpeed = default;

    public bool isStun = false;     //경직
    public bool isWait = false;     //탐지못함
    public bool isDelay = false;    //공격 딜레이

    public virtual void Init()
    {
        enemyAnimator = GetComponent<Animator>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyRenderer = GetComponent<SpriteRenderer>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    public virtual void Remove()
    {
        Destroy(enemyAnimator);
        Destroy(enemyCollider);
        Destroy(enemyRenderer);
        Destroy(enemyRigidbody);
    }

    public virtual void Attack()
    {
        EnemyWeapon_Legacy weaponObject = Instantiate(attackObject, attackPosition.position, Quaternion.identity);
        weaponObject.useWeapon(enemyDirection); 
    }

    public virtual void Touch() { }
    public virtual void Hit() { }
    public virtual void Dead() { }
    public virtual void Move() 
    {
    }
    public virtual void Defance() { }

    private void Update()
    {
        
    }
}

public enum EnemyDirection_Legacy {
    LEFT = -1, RIGHT = 1
}