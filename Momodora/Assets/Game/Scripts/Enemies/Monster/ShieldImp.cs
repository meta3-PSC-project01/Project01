using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldImp : EnemyBase
{
    //플레이어를 인식한곳, 배치된 위치에서 크게 벗어나지않은 범위내에서 앞뒤로 랜덤하게 움직인다.
    //이동->방어->공격 식의 루틴
    //튜토리얼 용으로 바라보는 위치가 고정되는 옵션이 필요
    //움직이는 동안 플레이어를 바라보게 턴한다.    
    //방패를 들고있거나 공격중에는 턴을 하지않는다.
    //공격 범위 높이 : 자신보다 2칸 이하(아래부분은 상관없음), 카메라 범위정도

    [SerializeField]
    public Coroutine routine = default;

    //공격 딜레이
    private float attackDelay = 5f;
    //현재 공격 딜레이
    private float currDelay = 0;
    //행동 딜레이
    private float wait = 1f;
    //방어 딜레이
    private float defenceTime = .5F;
    //이동 딜레이
    private float moveTime = .5F;

    //공격중인지 판별
    public bool isAttack = false;
    //방어중인지
    public bool isDefence = false;
    //이동중인지
    public bool isMove = false;

    Vector2 firstPoint;

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
        firstPoint = transform.position;
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
                        //루틴 시작
                        isDefence = true;
                        routine = StartCoroutine(MonsterRoutine());
                    }
                }
            }

            //플레이어 타겟 지정중에 항상 공격 딜레이 증감
            if (currDelay < attackDelay)
            {
                currDelay += Time.deltaTime;
            }

            if (!isStun)
            {
                //이동 상태일경우
                if (isMove)
                {
                    //좌표 비교후 회전
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
                    Move();
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
                    Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + ((int)direction * Vector3.right * 4), new Vector2(8, 28), 0);

                    //hit배열을 모두 돈다
                    foreach (Collider2D hit in hits)
                    {
                        //player 검출시
                        if (hit.tag == "Player" && hit.transform.position.y - transform.position.y <= 2)
                        {
                            enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                            isAttack = true;
                            isMove = false;
                            isDefence = false;
                            AttackStart();
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


                }

                //스턴 상태 아닐경우
                if (!isAttack)
                {
                    //이동
                    isMove = true;
                    yield return new WaitForSeconds(moveTime);
                    enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                    isMove = false;

                    //방어타임
                    yield return new WaitForSeconds(defenceTime);

                }

                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + ((int)direction * Vector3.right * 4), new Vector2(8, 28));

    }

    //이동 재정의
    public override void Move()
    {
        if(Mathf.Abs(transform.position.x - firstPoint.x) >= 2)
        {
            enemyRigidbody.velocity = new Vector2(firstPoint.x - transform.position.x, enemyRigidbody.velocity.y);
        }
        else
        {
            if (isMovingPlatform)
            {
                enemyRigidbody.velocity = new Vector2(enemySpeed * (int)direction + platformBody.velocity.x, enemyRigidbody.velocity.y);
            }
            //기본 플랫폼에서의 움직임
            else
            {
                enemyRigidbody.velocity = new Vector2(enemySpeed * (int)direction, enemyRigidbody.velocity.y);
            }

        }
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
        yield return new WaitForSeconds(wait);
        isAttack = false;
        isDefence = true;
    }
}
