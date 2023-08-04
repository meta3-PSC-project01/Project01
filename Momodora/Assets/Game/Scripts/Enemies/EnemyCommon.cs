using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCommon : MonoBehaviour
{
    public Rigidbody2D enemyRigidbody;
    public SpriteRenderer enemyRenderer;
    public BoxCollider2D enemyCollider;
    public Animator enemyAnimator;

    public EnemyAttackObject attackObject;

    public int enemyHp = default;
    public float enemySpeed = default;
    public float enemyWidth = default;
    public float enemyHeight = default;

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
        gameObject.name = "EmptyObject";
    }

    public virtual void Attack() { }
    public virtual void Touch() { }
    public virtual void Hit() { }
    public virtual void Dead() { }
    public virtual void Move() { }
    public virtual void Defance() { }
    public virtual void Find() { }
}
