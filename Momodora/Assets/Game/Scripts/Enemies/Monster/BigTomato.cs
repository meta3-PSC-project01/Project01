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
    private float attackDelay = 1.5f;
    //공격후 일정 시간 대기
    private float wait = 1f;
    private float currDelay = 0;

    public bool isAttack = false;

    private EnemyAttackData attackObject = null;



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
                    if (!isStun)
                    {
                        routine = StartCoroutine(MonsterRoutine());
                    }
                }
            }

            if (currDelay < attackDelay)
            {
                currDelay += Time.deltaTime;
            }

            if (isStun && currDelay > attackDelay)
            {
                currDelay = attackDelay * .6f;
            }
        }
    }

    IEnumerator MonsterRoutine()
    {
        while (true)
        {
            if (isStun)
            {
                yield return new WaitForEndOfFrame();
            }
            else
            {

                if (transform.position.x - target.transform.position.x > 0)
                {
                    direction = DirectionHorizen.LEFT;
                    turn();
                }
                else
                {
                    direction = DirectionHorizen.RIGHT;
                    turn();
                }

                if (currDelay >= attackDelay && !isAttack)
                {
                    Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition.position, 2);

                    //hit배열을 모두 돈다
                    foreach (Collider2D hit in hits)
                    {
                        Debug.Log("!");
                        if (hit.tag == "Player")
                        {
                            Debug.Log("!");
                            enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                            AttackStart();
                            isAttack = true;
                            currDelay = 0;
                            break;
                        }
                    }
                    if (isAttack)
                    {
                        break;
                    }
                    yield return new WaitForEndOfFrame();
                }
                else 
                {

                    Move();
                    enemyAnimator.SetTrigger("Move");
                    yield return new WaitForSeconds(moveDelay);
                    enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                }
                    

            }
            
        }

        routine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(attackPosition.position, 2);

    }

    public override void Move()
    {
        if (isMovingPlatform)
        {
            enemyRigidbody.velocity = new Vector2((enemySpeed * (int)direction)+platformBody.velocity.x, enemyRigidbody.velocity.y);
        }
        else
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * (int)direction, enemyRigidbody.velocity.y);

        }
    }

    //애니메이션 시작
    public override void AttackStart()
    {
        enemyAnimator.SetTrigger("Attack");
    }

    //애니메이션 중 공격 세팅
    public void AttackStartEvent()
    {
        attackObject = Instantiate(attackData[0].gameObject, attackPosition.position, transform.rotation, transform).GetComponent<EnemyAttackData>();
        
    }

    //애니메이션 중 콜라이더 세팅
    public void AttackColliderEvent()
    {
        
        attackObject.UseCollider();
    }

    //애니메이션 중 이펙트 세팅
    public void AttackEffectEvent()
    {
    
        attackObject.UseEffect();
    }

    //애니메이션 중 공격 종료
    public void AttackEndEvent()
    {
       
        Destroy(attackObject.gameObject);
    }

    public void RoutineEndEvent()
    {
        if (isAttack && routine == null)
        {
            routine = StartCoroutine(EndAnimation());
        
        }
    }

    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(wait);
        isAttack = false;
        routine = null;
    }
}
