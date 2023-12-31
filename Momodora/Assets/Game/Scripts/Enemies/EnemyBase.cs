using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


//enemy관련 최상단 클래스


//trigger -> collider체력-공격력
//collider hit()
public class EnemyBase : MonoBehaviour, IHitControl
{
    //에너미 기본 컴포넌트
    protected Rigidbody2D enemyRigidbody;
    protected BoxCollider2D parentColiider;
    protected BoxCollider2D childColiider;
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

    public GameObject gold;

    //에너미 속성
    public int enemyHp = default;           //체력
    public float enemySpeed = default;      //속도
    public DirectionHorizen direction = DirectionHorizen.LEFT;   //방향
    public int goldCount = 5;

    protected Coroutine stunCoroutine = null;
    public int enemyStunRegistValue = default; //스턴 데미지 한도
    public int enemyStunRegistMaxCount = default; //스턴 데미지 횟수
    public int enemyStunRegistCurrCount = default; //스턴 데미지 횟수

    public bool isStun = false;     //경직
    public bool isTouch = false;


    public Rigidbody2D platformBody;
    public bool isMovingPlatform = false;
    public bool isGround = true;

    public Vector3 firstPosition;

    //초기화
    public virtual void Init()
    {
        firstPosition = transform.position;

        sight = GetComponentInChildren<EnemySight>();
        enemyRenderer = GetComponentInChildren<SpriteRenderer>();
        enemyAnimator = GetComponentInChildren<Animator>();

        enemyAudio = GetComponent<AudioSource>();
        parentColiider = GetComponent<BoxCollider2D>();
        childColiider = transform.Find("Collider").GetComponent<BoxCollider2D>();
        enemyRigidbody = GetComponent<Rigidbody2D>();


        Physics2D.IgnoreCollision(parentColiider, childColiider);
    }

    private void Start()
    {
        transform.position = firstPosition;
        transform.localPosition = firstPosition;
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
        //플레이어 반응 
        player.Hit(1, -(int)direction);
    }

    //해당 몬스터가 플레이어 공격 맞을시(플레이어의 ontrigger이벤트)
    //상대가 호출한다.
    //데미지 높은 공격시에 스턴에 걸린다.
    public void Hit(int damage, int direction)
    {
        if (enemyHp > 0)
        {
            enemyRigidbody.velocity = Vector3.zero;


            if (enemyStunRegistValue <= damage)
            {
                //Debug.Log("스턴카운트+1");
                enemyStunRegistCurrCount += 1;
                if (enemyStunRegistMaxCount <= enemyStunRegistCurrCount)
                {
                    HitReaction(direction);
                    //Debug.Log("스턴");
                    enemyStunRegistCurrCount = 0;
                    isStun = true;
                    enemyAnimator.SetTrigger("Hit");
                }
            }

            enemyHp -= damage;

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
    }

    //몬스터 hit시 반응
    //기본은 색 바뀌기
    public virtual void HitReaction(int direction)
    {
        StartCoroutine(HitReactionRoutine());
    }

    IEnumerator HitReactionRoutine()
    {
        for(int i = 0; i < 6; i++)
        {
            enemyRenderer.color = new Color(255,0,0,200);
            yield return new WaitForSeconds(.05f);
            enemyRenderer.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(.05f);
        }
    }

    //몬스터 죽을시
    //추후 확장성을 위해서 virtual로 지정(죽을때 효과있는 몬스터)
    public virtual void Dead()
    {
        //죽음관련 재생(애니메이션, 소리)
        enemyAnimator.SetBool("Dead", true);

        int countResult = goldCount;
        if (ItemManager.instance.IsEquipItem("아스트랄 부적"))
        {
            if (Random.Range(0, 10) >= 7)
            {
                countResult = goldCount * 2;
            }
        }


        for(int i = 0; i < countResult; i++)
        {
            GameObject tmp = Instantiate(gold, transform.position, Quaternion.identity, GameManager.instance.currMap.transform);
            tmp.tag = "Gold";
            tmp.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle*Random.Range(4f,5f), ForceMode2D.Impulse);
        }
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Dead"));

        //Debug.Log(gameObject.name+"죽음");
        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        parentColiider.enabled = false;
        childColiider.enabled = false;
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


    //일정시간동안 스턴이 걸린다.
    public IEnumerator StunDelay(float time)
    {
        yield return new WaitForSeconds(time);
        isStun = false;
        enemyAnimator.SetBool("Stun", false);        
    }

    public bool IsHitPossible()
    {

        return enemyHp<=0  ? false : true;
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.name == "BorderCollider")
        {
            if (collision.collider.GetComponentInParent<PlayerMove>() != null)
            {
                Touch(collision.collider.GetComponentInParent<PlayerMove>());
            }
        }
    }

    public void StopWhenDash()
    {
        enemyRigidbody.velocity = Vector3.zero;
    }
}


public enum DirectionHorizen
{
    LEFT = -1,
    RIGHT = 1
}