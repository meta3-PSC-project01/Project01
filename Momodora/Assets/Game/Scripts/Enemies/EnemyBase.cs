using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//enemy관련 최상단 클래스

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
    public TestPlayer target = null;

    //에너미 속성
    public int enemyHp = default;           //체력
    public float enemySpeed = default;      //속도
    public Direction direction = Direction.LEFT;   //방향
    public int enemyDamageRegist = default; //스턴 데미지 한도

    public bool isStun = false;     //경직

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

    //몬스터 공격
    //최하단 자식 클래스에서 구현한다.
    //애니메이션 도중에 실행된다.
    //충돌 후처리는(공격 성공) 해당 공격 prefab에서 처리한다.
    public virtual void Attack()
    {
        Instantiate(attackData[0].prefab, attackPosition.position, transform.rotation, transform);
    }

    //몬스터의 콜라이더 이벤트시
    public void Touch(TestPlayer player)
    {
        player.hp -= 1;
        //플레이어 반응 
        //player.Hit();
    }

    //해당 몬스터가 플레이어 공격 맞을시(플레이어의 ontrigger이벤트)
    //상대가 호출한다.
    //데미지 높은 공격시에 스턴에 걸린다.
    public void Hit(bool isStun)
    {
        //피격관련 재생(애니메이션, 소리)
        //enemyAnimator.SetTrigger("Hit");
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Hit"));

        //플레이어의 공격으로 체력이 적용된 상태로 온다.
        if (enemyHp <= 0)
        {
            Dead();
            return;
        }

        //플레이어 인식
        target = FindObjectOfType<TestPlayer>();

        if (isStun)
        {
            this.isStun = true;
            //일정시간 경직
            //enemyAnimator.SetBool("Stun", true);
            StartCoroutine(StunDelay(1f));
        }
    }

    //몬스터 죽을시
    //추후 확장성을 위해서 virtual로 지정(죽을때 효과있는 몬스터)
    public virtual void Dead()
    {
        //죽음관련 재생(애니메이션, 소리)
        //enemyAnimator.SetTrigger("Dead");
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Dead"));

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
        if (direction == Direction.LEFT)
        {
            transform.localEulerAngles = new Vector3(0, 0, 0);
            sight.RotateAngleZ(-90);
        }
        else if (direction == Direction.RIGHT)
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
            Touch(collision.collider.GetComponent<TestPlayer>());
        }
    }

    //일정시간동안 스턴이 걸린다.
    public IEnumerator StunDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isStun = false;
        //enemyAnimator.SetBool("Stun", false);
    }
}


public enum Direction
{
    LEFT = -1,
    RIGHT = 1
}