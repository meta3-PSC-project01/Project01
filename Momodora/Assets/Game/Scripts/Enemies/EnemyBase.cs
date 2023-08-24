using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.Tilemaps;
using UnityEngine.UI;


//enemy���� �ֻ�� Ŭ����


//trigger -> colliderü��-���ݷ�
//collider hit()
public class EnemyBase : MonoBehaviour, IHitControl
{
    //���ʹ� �⺻ ������Ʈ
    protected Rigidbody2D enemyRigidbody;
    protected BoxCollider2D parentColiider;
    protected BoxCollider2D childColiider;
    protected SpriteRenderer enemyRenderer;
    protected Animator enemyAnimator;
    protected AudioSource enemyAudio;


    public EnemyAudioManager enemyAudioManager;

    //���ݰ��� ������Ʈ
    public Transform attackPosition;    //���� ���� ��ġ
    public EnemyAttackData[] attackData;      //����ü, ���ݹ��� collider���� ����

    //�������� ������Ʈ
    public EnemySight sight;

    //�� prefab �ϴ��� sight ��ũ��Ʈ�� �ش� ������ ��Ʈ���Ѵ�. 
    public PlayerMove target = null;

    public GameObject gold;

    //���ʹ� �Ӽ�
    public int enemyHp = default;           //ü��
    public float enemySpeed = default;      //�ӵ�
    public DirectionHorizen direction = DirectionHorizen.LEFT;   //����
    public int goldCount = 5;

    private Coroutine stunCoroutine = null;
    public int enemyStunRegistValue = default; //���� ������ �ѵ�
    public int enemyStunRegistMaxCount = default; //���� ������ Ƚ��
    public int enemyStunRegistCurrCount = default; //���� ������ Ƚ��

    public bool isStun = false;     //����
    public bool isTouch = false;


    public Rigidbody2D platformBody;
    public bool isMovingPlatform = false;

    public Vector3 firstPosition;

    //�ʱ�ȭ
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


    //���� ���ݽ� �ִϸ��̼� ���
    //2�� �̻��� ���� ����� ���� ���� ���� �� �ִ�.
    public virtual void AttackStart()
    {
        //���� ���� ���� ���(�ִϸ��̼�, �Ҹ�)
        //enemyAnimator.SetTrigger("AttackStart");
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "AttackStart"));
    }

    //������ �ݶ��̴� �̺�Ʈ��
    public void Touch(PlayerMove player)
    {
        //�÷��̾� ���� 
        player.Hit(1, -(int)direction);
    }

    //�ش� ���Ͱ� �÷��̾� ���� ������(�÷��̾��� ontrigger�̺�Ʈ)
    //��밡 ȣ���Ѵ�.
    //������ ���� ���ݽÿ� ���Ͽ� �ɸ���.
    public void Hit(int damage, int direction)
    {
        if (enemyHp > 0)
        {
            enemyRigidbody.velocity = Vector3.zero;


            if (enemyStunRegistValue <= damage)
            {
                //Debug.Log("����ī��Ʈ+1");
                enemyStunRegistCurrCount += 1;
                if (enemyStunRegistMaxCount <= enemyStunRegistCurrCount)
                {
                    HitReaction(direction);
                    //Debug.Log("����");
                    enemyStunRegistCurrCount = 0;
                    isStun = true;
                    enemyAnimator.SetTrigger("Hit");
                }
            }

            enemyHp -= damage;

            //�ǰݰ��� ���(�ִϸ��̼�, �Ҹ�)
            //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Hit"));

            //�÷��̾��� �������� ü���� ����� ���·� �´�.
            if (enemyHp <= 0)
            {
                Dead();
                return;
            }

            //������ ������ �÷��̾� �ν�
            target = FindObjectOfType<PlayerMove>();

            if (isStun)
            {
                if (stunCoroutine != null)
                {
                    StopCoroutine(stunCoroutine);
                }
                //�����ð� ����
                enemyAnimator.SetBool("Stun", true);
                stunCoroutine = StartCoroutine(StunDelay(1f));
            }
        }
    }

    //���� hit�� ����
    //�⺻�� �� �ٲ��
    public virtual void HitReaction(int direction)
    {
        //�� �ٲ�� ���׼�
    }

    //���� ������
    //���� Ȯ�强�� ���ؼ� virtual�� ����(������ ȿ���ִ� ����)
    public virtual void Dead()
    {
        //�������� ���(�ִϸ��̼�, �Ҹ�)
        enemyAnimator.SetBool("Dead", true);
        for(int i = 0; i < goldCount; i++)
        {
            GameObject tmp = Instantiate(gold, transform.position, Quaternion.identity, GameManager.instance.currMap.transform);
            tmp.tag = "Gold";
            tmp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(8f, 10f) * ((Random.Range(0, 2) == 0) ? -1 : 1), -Random.Range(6f, 8f)), ForceMode2D.Impulse);
            Destroy(tmp, 5f);
        }
        //enemyAudio.PlayOneShot(enemyAudioManager.GetAudioClip(gameObject.name, "Dead"));

        //Debug.Log(gameObject.name+"����");
        enemyRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        parentColiider.enabled = false;
        childColiider.enabled = false;
        Destroy(gameObject,.5f);
    }

    //�̵� �Լ�
    //���ϴ� �ڽ� Ŭ�������� �����Ѵ�.
    //target ���� �ÿ� ����
    public virtual void Move()
    {
        float moveDistance = enemySpeed;
        enemyRigidbody.AddForce(new Vector2(moveDistance*(int)direction, 0));
    }

    //ȸ�� �Լ�
    //�⺻ : �翷���� �̵�
    //���� Ȯ�强�� ���ؼ� virtual�� ����(�÷��̾��� ��ġ�� ���������� �ٶ󺸴� ����)
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


    //�����ð����� ������ �ɸ���.
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
}


public enum DirectionHorizen
{
    LEFT = -1,
    RIGHT = 1
}