using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallTomato : EnemyBase
{
    //��� �÷��̾�� �����ϴٰ�
    //(move �����̸��� 1���� ������)
    //���� ���� + ���� Ÿ�̹��� ��� �����Ѵ�.
    //
    [SerializeField]
    public Coroutine routine = default;

    //���� ������
    private float attackDelay = .6f;
    //���� ���� ������
    private float currDelay = 0;
    //���� ���� ������
    private float wait = 1f;

    //��� ���� Ȱ��ȭ ����
    private bool onDashAttack = false;
    private bool upFloor = true;
    private float dashDistance = 4;
    private float dashSpeed = 15;
    private float dashJump = 3;

    //�̵� ���ӵ�
    private float accel = 0f;

    //���������� �Ǻ�
    public bool isAttack = false;

    //���� ����Ʈ
    //�ν�����â���� �����Ѵ�.
    private EnemyAttackData attackObject = null;



    // Start is called before the first frame update
    void Awake()
    {
        //base�� init �Լ� ����
        Init();

        //�ʱ� ������ ��
        currDelay = attackDelay * .8f;
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
                //���� ������ x
                if (!isAttack)
                {
                    if (!isStun)
                    {
                        if (Random.Range(0, 100) >=90)
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
                        //��ƾ ����
                        routine = StartCoroutine(MonsterRoutine());
                    }
                }
            }

            //�÷��̾� Ÿ�� �����߿� �׻� ���� ������ ����
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

    //���� ��ƾ
    IEnumerator MonsterRoutine()
    {
        //�׻�
        while (true)
        {

            //���� ���� �ƴҰ��
            if (isStun)
            {
                //���� �ȸ���
                yield return new WaitForEndOfFrame();
                continue;

            }
            //���� ������ ���
            else
            {


                //��ǥ ���� ȸ��
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

                //���������� �ʰ� ���� �����̰� ���� �����̺��� ������� ���� ����
                if (currDelay >= attackDelay && !isAttack)
                {
                    if (onDashAttack)
                    {
                        //�ش� ������ �ִ� �� ���� ������(���� ����)

                        Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + (dashDistance - .25f) * (int)direction, transform.position.y), new Vector2(.5f, .8f), 0);

                        //hit�迭�� ��� ����
                        foreach (Collider2D hit in hits)
                        {
                            //player �����
                            if (hit.tag == "Player")
                            {
                                //��ð��� Ȱ��ȭ+ ���� ���� �־����
                                //Ư���Ÿ�+
                                //�÷��̾� ��������
                                if (onDashAttack && upFloor)
                                {
                                    //����
                                    enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                                    AttackStart();
                                    isAttack = true;
                                    break;
                                }
                            }
                        }
                        //���� �������� ���
                        if (isAttack)
                        {
                            //���� ������
                            currDelay = 0;
                            //��ƾ ����
                            break;
                        }
                    }
                    {
                        Collider2D[] hits = Physics2D.OverlapBoxAll(attackPosition.position, new Vector2(1.5f, 1), 0);

                        //hit�迭�� ��� ����
                        foreach (Collider2D hit in hits)
                        {
                            //player �����
                            if (hit.tag == "Player")
                            {
                                enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                                AttackStart();
                                onDashAttack = false;
                                isAttack = true;
                                break;
                            }
                        }

                        //���� �������� ���
                        if (isAttack)
                        {
                            //���� ������
                            currDelay = 0;
                            //��ƾ ����
                            break;
                        }
                    }

                    if (isTouch)
                    {
                        enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                    }
                    else
                    {
                        //�̵�
                        Move();
                    }
                    yield return new WaitForSeconds(Time.deltaTime);
                }
                else
                {
                    yield return new WaitForEndOfFrame();
                }
            }

        }

        //��ƾ ����
        routine = null;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + (dashDistance - .25f) * (int)direction, transform.position.y), new Vector2(.5f, .8f));
        Gizmos.DrawWireCube(attackPosition.position, new Vector2(1.5f, 1));

    }

    Coroutine hitReactionCoroutine = null;

    public override void HitReaction(int direction)
    {
        base.HitReaction(direction);
        if (hitReactionCoroutine != null)
        {
            StopCoroutine(hitReactionCoroutine);
        }
        if (attackObject != null)
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

        enemyRigidbody.velocity = new Vector2(-direction* 5, 3);

    }

    //�̵� ������
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

        //���� �÷��������� ������
        //���� �÷����� ����ŭ ���ؼ� �����δ�.
        if (isMovingPlatform)
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * accel + platformBody.velocity.x, enemyRigidbody.velocity.y);
        }
        //�⺻ �÷��������� ������
        else
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * accel, enemyRigidbody.velocity.y);
        }
    }

    //�ִϸ��̼� ����
    public override void AttackStart()
    {
        enemyAnimator.SetTrigger("Attack");
    }

    //�ִϸ��̼� �� ����Ʈ �ν�źƮ = ���� ����
    public void AttackStartEvent()
    {
        attackObject = Instantiate(attackData[0].gameObject, attackPosition.position, transform.rotation).GetComponent<EnemyAttackData>();
        attackObject.transform.SetParent(transform);

    }

    //�ִϸ��̼� �� ����Ʈ ���� = ����Ʈ �߻�
    public void AttackEffectEvent()
    {
        if (onDashAttack)
        {
            enemyRigidbody.AddForce(new Vector2((int)direction * dashSpeed, dashJump), ForceMode2D.Impulse);
        }
    }

    //�ִϸ��̼� �� �ݶ��̴� ON = ���� Ÿ�̹�
    public void AttackColliderEvent()
    {
        attackObject.UseCollider();
    }


    //�ִϸ��̼� �� ���� ���� = ���� ����
    public void AttackEndEvent()
    {
        Destroy(attackObject.gameObject);
        attackObject = null;
    }

    //���� ���� = ��ƾ ����
    //���� �� ����� �����̸� ���� �ۼ�(wait)
    public void RoutineEndEvent()
    {
        //�������̰� ��ƾ�� ���ٸ�
        if (isAttack && routine == null)
        {
            accel = 0f;
            enemyAnimator.SetBool("Move", false);
            //������ �̺�Ʈ ����
            routine = StartCoroutine(EndAnimation());
        }
    }

    //�����̰� �����ڿ� �ٽ� �̵�/���� ��
    //idle ���¿��� ����
    IEnumerator EndAnimation()
    {
        yield return new WaitForSeconds(wait);
        isAttack = false;
        routine = null;
    }
}
