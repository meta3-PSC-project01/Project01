using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : EnemyBase
{

    //계속 이동/점프
    //점프 힘, 이동 방향/힘 랜덤
    //그러다 공격 타이밍때 공격

    [SerializeField]
    public Coroutine routine = default;

    //공격 딜레이
    private float attackDelay = 5f;
    //현재 공격 딜레이
    private float currDelay = 0;

    //공격중인지 판별
    public bool isAttack = false;
    //점프 파워
    public bool isJump = false;
    public float jumpPower = 5f;

    //공격 이펙트
    //인스펙터창에서 저장한다.
    private EnemyAttackData attackObject = null;



    // Start is called before the first frame update
    void Awake()
    {
        //base의 init 함수 실행
        Init();

        //초기 딜레이 값
        currDelay = attackDelay * .5f;
    }

    // Update is called once per frame
    void Update()
    {
        //플레이어 타겟 지정
        if (target != null)
        {
            //루틴 진행중 x
            if (routine == null)
            {
                if (!isStun)
                {
                    //루틴 시작
                    routine = StartCoroutine(MonsterRoutine());
                    enemyAnimator.SetBool("Move", true);
                }
            }

            //플레이어 타겟 지정중에 항상 공격 딜레이 증감
            if (currDelay < attackDelay)
            {
                currDelay += Time.deltaTime;
            }

            if (!isStun)
            {
                if (transform.position.x - target.transform.position.x > 0 && direction != DirectionHorizen.LEFT)
                {
                    direction = DirectionHorizen.LEFT;
                    turn();
                }
                else if (transform.position.x - target.transform.position.x < 0 && direction != DirectionHorizen.RIGHT)
                {
                    direction = DirectionHorizen.RIGHT;
                    turn();
                }

                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, transform.up * -1, .52f);

                foreach (var hit in hits)
                {
                    //바닥에 닿을 경우
                    if (hit.collider.tag == "Floor")
                    {
                        isJump = false;
                        Move();
                    }

                }
            }

            if (isStun && currDelay > attackDelay)
            {
                currDelay = attackDelay * .6f;
            }
        }
    }

    //몬스터 루틴
    IEnumerator MonsterRoutine()
    {
        //항상
        while (true)
        {
            //스턴 상태일 경우
            if (isStun)
            {
                //아직 안만듬
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //공격중이지 않고 현재 딜레이가 어택 딜레이보다 높을경우 조건 만족
                if (currDelay >= attackDelay && !isAttack)
                {
                    Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + ((int)direction * Vector3.right * 8), new Vector2(16, 28), 0);

                    //hit배열을 모두 돈다
                    foreach (Collider2D hit in hits)
                    {
                        //player 검출시
                        if (hit.tag == "Player")
                        {
                            AttackStart();
                            isAttack = true;
                            break;
                        }
                    }

                    //공격 시작했을 경우
                    if (isAttack)
                    {
                        //공격 딜레이
                        currDelay = 0;
                        yield return new WaitForEndOfFrame();
                        //루틴 초기화
                        continue;
                    }
                    yield return new WaitForEndOfFrame();
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
                yield return new WaitForEndOfFrame();

            }


        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + ((int)direction * Vector3.right * 4), new Vector2(8, 28));
        Debug.DrawRay(transform.position, transform.up * -1, Color.green);

    }

    //이동 재정의
    public override void Move()
    {
        isJump = true;
        float jumpResult = enemyRigidbody.velocity.y + jumpPower;
        
        if(jumpResult > 5f)
        {
            jumpResult = 5f;
        }

        if (isTouch)
        {
            enemySpeed = 0f;
        }

        if (isMovingPlatform)
        {
            enemyRigidbody.velocity = new Vector2(-enemySpeed * (int)direction + platformBody.velocity.x, jumpResult);
        }
        //기본 플랫폼에서의 움직임
        else
        {
            enemyRigidbody.velocity = new Vector2(-enemySpeed * (int)direction, jumpResult);
        }
    }

    Coroutine hitReactionCoroutine = null;

    public override void HitReaction(int direction)
    {
        base.HitReaction(direction);
        if (hitReactionCoroutine != null)
        {
            StopCoroutine(hitReactionCoroutine);
        }
        //if (attackObject != null)
        {
            AttackEndEvent();
        }
        hitReactionCoroutine = StartCoroutine(ReactionRoutine(direction));
    }

    IEnumerator ReactionRoutine(int direction)
    {
        Vector3 tmp;
        //.2초 떨림
        for (int i = 0; i < 10; i++)
        {
            tmp = new Vector3(Random.Range(0, .2f), Random.Range(0, .2f));
            transform.position = transform.position + tmp;
            yield return new WaitForSeconds(.02f);
            transform.position = transform.position - tmp;
        }
        yield return new WaitForEndOfFrame();

        // enemyRigidbody.velocity = new Vector2(-direction * 5, 3);

    }
    //애니메이션 시작
    public override void AttackStart()
    {
        enemyAnimator.SetTrigger("Attack");
    }

    //애니메이션 중 이펙트 인스탄트 = 공격 시작
    //레인지 공격일 경우 투척 타이밍때 생성할것
    public void AttackStartEvent()
    {
        attackObject = Instantiate(attackData[0].gameObject, attackPosition.position, transform.rotation).GetComponent<EnemyAttackData>();
        attackObject.transform.SetParent(GameManager.instance.currMap.transform);

    }

    //애니메이션 중 이펙트 세팅 = 이펙트 발생
    //레인지 공격일 경우 추가 이펙트 존재시 추가로 생성 
    public void AttackEffectEvent()
    {
        //임프는 사용안함
    }

    //애니메이션 중 콜라이더 ON = 공격 타이밍
    //레인지 공격일 경우 사용 안함(투사체가 자체적으로 사용)
    public void AttackColliderEvent()
    {
        //임프는 사용안함
    }


    //애니메이션 중 공격 종료 = 공격 종료
    public void AttackEndEvent()
    {
        StartCoroutine(EndAnimation());
    }

    //공격 종료 = 루틴 종료
    //공격 후 잠시의 딜레이를 위해 작성(wait)
    public void RoutineEndEvent()
    {
        //임프는 사용하지않음
    }

    //딜레이가 지난뒤에 다시 이동/공격 함
    IEnumerator EndAnimation()
    {
        yield return null;
        isAttack = false;
    }
}
