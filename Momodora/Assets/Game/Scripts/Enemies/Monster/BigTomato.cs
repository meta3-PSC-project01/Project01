using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigTomato : EnemyBase
{
    //계속 플레이어에게 접근하다가
    //공격 범위 + 공격 타이밍일 경우 공격한다.
    [SerializeField]
    public Coroutine routine = default;

    private float moveDelay = 1f;
    private float attackDelay = 1f;
    private float currDelay = 0;

    public bool isAttack = false;


    // Start is called before the first frame update
    void Awake()
    {
        Init();
        currDelay = attackDelay - 1f;
        enemySpeed = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            if (routine == null)
            {
                if (!isAttack)
                {
                    routine = StartCoroutine(MonsterRoutine());
                }
            }

            if (currDelay < attackDelay)
            {
                currDelay += Time.deltaTime;
            }
        }
    }

    IEnumerator MonsterRoutine()
    {
        while (true)
        {
            if (transform.position.x - target.transform.position.x > 0)
            {
                direction = Direction.LEFT;
                turn();
            }
            else
            {
                direction = Direction.RIGHT;
                turn();
            }
            if (currDelay >= attackDelay && !isAttack)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition.position, 2);

                //hit배열을 모두 돈다
                foreach (Collider2D hit in hits)
                {
                    if (hit.tag == "Player")
                    {
                        enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                        AttackStart();
                        isAttack = true;
                        currDelay = 0;
                    }
                }
                if (isAttack)
                {
                    break;
                }
            }
            
            if (!isStun)
            {
                Move();
                enemyAnimator.SetTrigger("Move");
                yield return new WaitForSeconds(moveDelay);
                enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
            }
            else
            {
                yield return new WaitForEndOfFrame();
            }
        }

        StopCoroutine(routine);
        routine = null;
    }

    public override void Move()
    {
        enemyRigidbody.velocity = new Vector2(enemySpeed * (int)direction, enemyRigidbody.velocity.y);
    }

    public override void AttackStart()
    {
        enemyAnimator.SetTrigger("Attack");
    }

    public void EndAttackAnimation()
    {
        if (isAttack && routine==null)
        {
            routine = StartCoroutine(EndAnimation());
        }
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttack = false;
        routine = null;
    }
}
