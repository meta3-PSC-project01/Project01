using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTomato : EnemyBase
{
    //계속 플레이어에게 접근하다가
    //(move 딜레이마다 1번씩 움직임)
    //공격 범위 + 공격 타이밍일 경우 공격한다.
    //
    [SerializeField]
    public Coroutine routine = default;

    //공격 딜레이
    private float attackDelay = .6f;
    //현재 공격 딜레이
    private float currDelay = 0;
    //현재 공격 딜레이
    private float wait = 1f;

    //대시 공격 활성화 여부
    private bool onDashAttack = false;
    private bool upFloor = true;
    private float dashDistance = 4;
    private float dashSpeed = 10;
    private float dashJump = 3;

    //이동 가속도
    private float accel = 0f;

    //공격중인지 판별
    public bool isAttack = false;

    //공격 이펙트
    //인스펙터창에서 저장한다.
    private EnemyAttackData attackObject = null;



    // Start is called before the first frame update
    void Awake()
    {
        //base의 init 함수 실행
        Init();

        //초기 딜레이 값
        currDelay = attackDelay * .8f;
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
                //공격 진행중 x
                if (!isAttack)
                {
                    if (!isStun)
                    {
                        if (Random.Range(0, 100) >= 90)
                        {
                            onDashAttack = true;
                        }
                        else
                        {
                            onDashAttack = false;
                        }

                        if (!enemyAnimator.GetBool("Move"))
                        {
                            enemyAnimator.SetBool("Move", true);
                        }
                        enemyRigidbody.velocity = Vector2.zero;
                        //루틴 시작
                        routine = StartCoroutine(MonsterRoutine());
                    }
                }
            }

            //플레이어 타겟 지정중에 항상 공격 딜레이 증감
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

    //몬스터 루틴
    IEnumerator MonsterRoutine()
    {
        //항상
        while (true)
        {

            //스턴 상태 아닐경우
            if (isStun)
            {
                //아직 안만듬
                yield return new WaitForEndOfFrame();
                continue;

            }
            //스턴 상태일 경우
            else
            {
            

            //좌표 비교후 회전
            if (transform.position.x - target.transform.position.x > 0 && direction != DirectionHorizen.LEFT)
            {
                direction = DirectionHorizen.LEFT;
                turn();
                accel *= .75f;
            }
            else if (transform.position.x - target.transform.position.x < 0 && direction != DirectionHorizen.RIGHT)
            {
                direction = DirectionHorizen.RIGHT;
                turn();
                accel *= .75f;
            }

                //공격중이지 않고 현재 딜레이가 어택 딜레이보다 높을경우 조건 만족
                if (currDelay >= attackDelay && !isAttack)
                {
                    if (onDashAttack)
                    {
                        //해당 영역에 있는 적 정보 가져옴(공격 범위)

                        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (dashDistance - .25f) * (int)direction, transform.position.y), new Vector2(.5f, .8f), 0);

                        //hit배열을 모두 돈다
                        foreach (Collider2D hit in hits)
                        {
                            //player 검출시
                            if (hit.tag == "Player")
                            {
                                //대시공격 활성화+ 몬스터 땅에 있어야함
                                //특정거리+
                                //플레이어 점프안함
                                if (onDashAttack && upFloor)
                                {
                                    //정지
                                    enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                                    AttackStart();
                                    isAttack = true;
                                    break;
                                }
                            }
                        }
                        //공격 시작했을 경우
                        if (isAttack)
                        {
                            //공격 딜레이
                            currDelay = 0;
                            //루틴 종료
                            break;
                        }
                    }
                    {
                        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPosition.position, new Vector2(1.5f, 1), 0);

                        //hit배열을 모두 돈다
                        foreach (Collider2D hit in hits)
                        {
                            //player 검출시
                            if (hit.tag == "Player")
                            {
                                Debug.Log("일반공격");
                                enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                                AttackStart();
                                onDashAttack = false;
                                isAttack = true;
                                break;
                            }
                        }

                        //공격 시작했을 경우
                        if (isAttack)
                        {
                            //공격 딜레이
                            currDelay = 0;
                            //루틴 종료
                            break;
                        }
                    }

                    //이동
                    Move();
                    yield return new WaitForSeconds(Time.deltaTime);
                }
            }

        }

        //루틴 종료
        routine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (dashDistance - .25f) * (int)direction, transform.position.y), new Vector2(.5f, .8f));
        Gizmos.DrawWireCube(attackPosition.position, new Vector2(1.5f, 1));

    }

    //이동 재정의
    public override void Move()
    {
        if (direction == DirectionHorizen.LEFT)
        {
            accel -= Time.deltaTime;
        }
        else if (direction == DirectionHorizen.RIGHT)
        {
            accel += Time.deltaTime;
        }

        accel = Mathf.Clamp(accel, -1, 1);

        //무빙 플랫폼에서의 움직임
        //무빙 플랫폼의 힘만큼 더해서 움직인다.
        if (isMovingPlatform)
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * accel + platformBody.velocity.x, enemyRigidbody.velocity.y);
        }
        //기본 플랫폼에서의 움직임
        else
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * accel, enemyRigidbody.velocity.y);
        }
    }

    //애니메이션 시작
    public override void AttackStart()
    {
        Debug.Log("공격시작");
        enemyAnimator.SetTrigger("Attack");
    }

    //애니메이션 중 이펙트 인스탄트 = 공격 시작
    public void AttackStartEvent()
    {
        Debug.Log("인스턴스생성");
        attackObject = Instantiate(attackData[0].gameObject, attackPosition.position, transform.rotation, transform).GetComponent<EnemyAttackData>();

    }

    //애니메이션 중 이펙트 세팅 = 이펙트 발생
    //스몰토마토는 이펙트 대신 날라가는 모션
    public void AttackEffectEvent()
    {
        if (onDashAttack)
        {
            Debug.Log("모션");
            enemyRigidbody.AddForce(new Vector2((int)direction * dashSpeed, dashJump), ForceMode2D.Impulse);
        }
    }

    //애니메이션 중 콜라이더 ON = 공격 타이밍
    public void AttackColliderEvent()
    {
        Debug.Log("콜라이더");
        attackObject.UseCollider();
    }


    //애니메이션 중 공격 종료 = 공격 종료
    public void AttackEndEvent()
    {
        Debug.Log("인스턴스 파괴");
        Destroy(attackObject.gameObject);
    }

    //공격 종료 = 루틴 종료
    //공격 후 잠시의 딜레이를 위해 작성(wait)
    public void RoutineEndEvent()
    {
        //공격중이고 루틴이 없다면
        if (isAttack && routine == null)
        {
            accel = 0f;
            enemyAnimator.SetBool("Move", false);
            //딜레이 이벤트 시작
            routine = StartCoroutine(EndAnimation());
        }
    }

    //딜레이가 지난뒤에 다시 이동/공격 함
    //idle 상태에서 실행
    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(wait);
        isAttack = false;
        routine = null;
    }
}
