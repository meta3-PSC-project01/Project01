using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Imp : EnemyBase
{

    //��� �̵�/����
    //���� ��, �̵� ����/�� ����
    //�׷��� ���� Ÿ�ֶ̹� ����

    [SerializeField]
    public Coroutine routine = default;

    //���� ������
    private float attackDelay = 5f;
    //���� ���� ������
    private float currDelay = 0;

    //���������� �Ǻ�
    public bool isAttack = false;
    //���� �Ŀ�
    public float jumpPower = 5f;

    //���� ����Ʈ
    //�ν�����â���� �����Ѵ�.
    private EnemyAttackData attackObject = null;



    // Start is called before the first frame update
    void Awake()
    {
        //base�� init �Լ� ����
        Init();

        //�ʱ� ������ ��
        currDelay = attackDelay * .5f;
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾� Ÿ�� ����
        if (target != null)
        {
            //��ƾ ������ x
            if (routine == null)
            {
                if (!isStun)
                {
                    //��ƾ ����
                    routine = StartCoroutine(MonsterRoutine());
                }
            }

            //�÷��̾� Ÿ�� �����߿� �׻� ���� ������ ����
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

                if(isGround)
                {
                    isGround = false;
                    enemyAnimator.SetBool("Move", true);
                    Move();
                }
            }

            if (isStun && currDelay > attackDelay)
            {
                currDelay = attackDelay * .6f;
            }
        }
    }

    //���� ��ƾ
    IEnumerator MonsterRoutine()
    {
        //�׻�
        while (true)
        {
            //���� ������ ���
            if (isStun)
            {
                //���� �ȸ���
                yield return new WaitForEndOfFrame();
            }
            else
            {
                //���������� �ʰ� ���� �����̰� ���� �����̺��� ������� ���� ����
                if (currDelay >= attackDelay && !isAttack)
                {
                    Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + ((int)direction * Vector3.right * 8), new Vector2(16, 28), 0);

                    //hit�迭�� ��� ����
                    foreach (Collider2D hit in hits)
                    {
                        //player �����
                        if (hit.tag == "Player")
                        {
                            AttackStart();
                            isAttack = true;
                            break;
                        }
                    }

                    //���� �������� ���
                    if (isAttack)
                    {
                        //���� ������
                        currDelay = 0;
                        yield return new WaitForEndOfFrame();
                        //��ƾ �ʱ�ȭ
                        continue;
                    }
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

    //�̵� ������
    public override void Move()
    {
        float jumpResult = enemyRigidbody.velocity.y + jumpPower;
        float directionResult = -1f;

        if (jumpResult > 5f)
        {
            jumpResult = 5f;
        }

        if (isTouch)
        {
            directionResult = 0f;
        }

        if(Random.Range(0,10) <= 2)
        {
            directionResult = 1f;
        }

        if (isMovingPlatform)
        {
            enemyRigidbody.velocity = new Vector2(directionResult * enemySpeed * (int)direction + platformBody.velocity.x, jumpResult);
        }
        //�⺻ �÷��������� ������
        else
        {
            enemyRigidbody.velocity = new Vector2(directionResult * enemySpeed * (int)direction, jumpResult);
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
        //.2�� ����
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
    //�ִϸ��̼� ����
    public override void AttackStart()
    {
        enemyAnimator.SetTrigger("Attack");
    }

    //�ִϸ��̼� �� ����Ʈ �ν�źƮ = ���� ����
    //������ ������ ��� ��ô Ÿ�ֶ̹� �����Ұ�
    public void AttackStartEvent()
    {
        attackObject = Instantiate(attackData[0].gameObject, attackPosition.position, transform.rotation).GetComponent<EnemyAttackData>();
        attackObject.transform.SetParent(GameManager.instance.currMap.transform);

    }

    //�ִϸ��̼� �� ����Ʈ ���� = ����Ʈ �߻�
    //������ ������ ��� �߰� ����Ʈ ����� �߰��� ���� 
    public void AttackEffectEvent()
    {
        //������ ������
    }

    //�ִϸ��̼� �� �ݶ��̴� ON = ���� Ÿ�̹�
    //������ ������ ��� ��� ����(����ü�� ��ü������ ���)
    public void AttackColliderEvent()
    {
        //������ ������
    }


    //�ִϸ��̼� �� ���� ���� = ���� ����
    public void AttackEndEvent()
    {
        StartCoroutine(EndAnimation());
    }

    //���� ���� = ��ƾ ����
    //���� �� ����� �����̸� ���� �ۼ�(wait)
    public void RoutineEndEvent()
    {
        //������ �����������
    }

    //�����̰� �����ڿ� �ٽ� �̵�/���� ��
    IEnumerator EndAnimation()
    {
        yield return null;
        isAttack = false;
    }
}
