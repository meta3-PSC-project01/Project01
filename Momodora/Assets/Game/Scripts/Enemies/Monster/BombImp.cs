using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombImp : EnemyBase
{
    //정지된 상태에서 플레이어를 향해 투사체를 던진다.
    //투사체는 일정거리 이하로는 발사되지않으며 포물선을 그린다.
    //투사체는 터진공간에 일정시간동안 디버프 장판을 생성한다.
    //디버프 공간에서 일정시간 서있으면 "독" 디버프를 얻는다.

    [SerializeField]
    public Coroutine routine = default;

    //공격 딜레이
    private float attackDelay = 5f;
    //현재 공격 딜레이
    private float currDelay = 0;

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
        enemyAnimator.SetBool("Idle", true);
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
                //루틴 시작
                routine = StartCoroutine(MonsterRoutine());
            }

            //플레이어 타겟 지정중에 항상 공격 딜레이 증감
            if (currDelay < attackDelay)
            {
                currDelay += Time.deltaTime;
            }

            //계속 플레이어를 바라봄
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

        }
    }

    //몬스터 루틴
    IEnumerator MonsterRoutine()
    {
        //항상
        while (true)
        {
            //공격중이지 않고 현재 딜레이가 어택 딜레이보다 높을경우 조건 만족
            if (currDelay >= attackDelay && !isAttack)
            {
                AttackStart();
                isAttack = true;
                //공격 딜레이
                currDelay = 0;
                yield return new WaitForEndOfFrame();
                //루틴 초기화
            }

            //스턴 상태일 경우
            if (isStun)
            {
                //아직 안만듬
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //그외 예외처리
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
        //폭탄 임프는 사용하지않음 
    }

    //애니메이션 시작
    public override void AttackStart()
    {
        Debug.Log("_공격시작");
        enemyAnimator.SetTrigger("Attack");
    }

    //애니메이션 중 이펙트 인스탄트 = 공격 시작
    //레인지 공격일 경우 투척 타이밍때 생성할것
    public void AttackStartEvent()
    {
        Debug.Log("인스턴스생성");
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
        yield return null;
        isAttack = false;
    }
}
