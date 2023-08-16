using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//enemy관련 최상단 클래스


//trigger -> collider체력-공격력
//collider hit()
public class EnemyBase : MonoBehaviour
{
    //에너미 기본 컴포넌트
    protected Rigidbody2D enemyRigidbody;
    protected BoxCollider2D enemyCollider;
    protected SpriteRenderer enemyRenderer;
    protected Animator enemyAnimator;
    protected AudioSource enemyAudio;

    public EnemyAudioManager enemyAudioManager;

    //공격관련 컴포넌트
    public Transform attackPosition;    //공격 생성 위치
    public EnemyAttackData[] attackData;      //투사체, 공격범위 collider등을 저장

    //추적관련 컴포넌트
    public EnemySight sight;

    //적 prefab 하단의 sight 스크립트가 해당 변수를 컨트롤한다. 
    public PlayerMove target = null;

    //에너미 속성
    public int enemyHp = default;           //체력
    public float enemySpeed = default;      //속도
    public DirectionHorizen direction = DirectionHorizen.LEFT;   //방향

    private Coroutine stunCoroutine = null;
    public int enemyStunRegistValue = default; //스턴 데미지 한도
    public int enemyStunRegistMaxCount = default; //스턴 데미지 횟수
    public int enemyStunRegistCurrCount = default; //스턴 데미지 횟수

    public bool isStun = false;     //경직


    public Rigidbody2D platformBody;
    public bool isMovingPlatform = false;

    //초기화
    public virtual void Init()
    {
        sight = GetComponentInChildren<EnemySight>();
        enemyRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponentInChildren<Animator>();

        enemyAudio = GetComponent<AudioSource>();
        enemyCollider = GetComponent<BoxCollider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();
    }

    //몬스터 공격시 애니메이션 재생
    //2개 이상의 공격 방식을 가진 적이 있을 수 있다.
    public virtual void AttackStart()
    {
        //공격 시작 관련 재생(애니메이션, 소리)
        //enemyAnimator.SetTrigger("AttackStart");
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "AttackStart"));
    }

    //몬스터의 콜라이더 이벤트시
    public void Touch(PlayerMove player)
    {
        player.playerHp -= 1;
        //플레이어 반응 
        //player.Hit();
    }

    //해당 몬스터가 플레이어 공격 맞을시(플레이어의 ontrigger이벤트)
    //상대가 호출한다.
    //데미지 높은 공격시에 스턴에 걸린다.
    public void Hit(int damage)
    {
        enemyRigidbody.velocity = Vector3.zero;
        enemyHp -= damage;
        

        if (enemyStunRegistValue <= damage)
        {
            Debug.Log("스턴카운트+1");
            enemyStunRegistCurrCount += 1;
            if (enemyStunRegistMaxCount <= enemyStunRegistCurrCount)
            {
                HitReaction();
                Debug.Log("스턴");
                enemyStunRegistCurrCount = 0;
                isStun = true;
                enemyAnimator.SetTrigger("Hit");
            }
        }

        //피격관련 재생(애니메이션, 소리)
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Hit"));

        //플레이어의 공격으로 체력이 적용된 상태로 온다.
        if (enemyHp <= 0)
        {
            Dead();
            return;
        }

        //맞으면 무조건 플레이어 인식
        target = FindObjectOfType<PlayerMove>();

        if (isStun)
        {
            if (stunCoroutine != null)
            {
                StopCoroutine(stunCoroutine);
            }
            //일정시간 경직
            enemyAnimator.SetBool("Stun", true);
            stunCoroutine = StartCoroutine(StunDelay(1f));
        }
    }

    //몬스터 hit시 반응
    //기본은 색 바뀌기
    public virtual void HitReaction()
    {
        //색 바뀌는 리액션
    }

    //몬스터 죽을시
    //추후 확장성을 위해서 virtual로 지정(죽을때 효과있는 몬스터)
    public virtual void Dead()
    {
        //죽음관련 재생(애니메이션, 소리)
        enemyAnimator.SetBool("Dead", true);
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Dead"));

        Debug.Log(gameObject.name+"죽음");
        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        enemyCollider.enabled = false;
        Destroy(gameObject,.5f);
    }

    //이동 함수
    //최하단 자식 클래스에서 구현한다.
    //target 지정 시에 실행
    public virtual void Move()
    {
        float moveDistance = enemySpeed;
        enemyRigidbody.AddForce(new Vector2(moveDistance*(int)direction, 0));
    }

    //회전 함수
    //기본 : 양옆으로 이동
    //추후 확장성을 위해서 virtual로 지정(플레이어의 위치를 직접적으로 바라보는 몬스터)
    public virtual void turn()
    {
        if (direction == DirectionHorizen.LEFT)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            sight.RotateAngleZ(-90);
        }
        else if (direction == DirectionHorizen.RIGHT)
        {
            transform.localEulerAngles = new Vector3(0, 180, 0);
            sight.RotateAngleZ(90);
        }        
    }

    //touch용
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Touch(collision.collider.GetComponent<PlayerMove>());
        }
    }

    //일정시간동안 스턴이 걸린다.
    public IEnumerator StunDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isStun = false;
        enemyAnimator.SetBool("Stun", false);        
    }    
}


public enum DirectionHorizen
{
    LEFT = -1,
    RIGHT = 1
}