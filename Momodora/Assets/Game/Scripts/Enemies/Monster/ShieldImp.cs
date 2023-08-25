using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldImp : EnemyBase, IHitControl
{
    //�÷��̾ �ν��Ѱ�, ��ġ�� ��ġ���� ũ�� ��������� ���������� �յڷ� �����ϰ� �����δ�.
    //�̵�->���->���� ���� ��ƾ
    //Ʃ�丮�� ������ �ٶ󺸴� ��ġ�� �����Ǵ� �ɼ��� �ʿ�
    //�����̴� ���� �÷��̾ �ٶ󺸰� ���Ѵ�.    
    //���и� ����ְų� �����߿��� ���� �����ʴ´�.
    //���� ���� ���� : �ڽź��� 2ĭ ����(�Ʒ��κ��� �������), ī�޶� ��������

    [SerializeField]
    public Coroutine routine = default;

    //���� ������
    private float attackDelay = 5f;
    //���� ���� ������
    private float currDelay = 0;
    //�ൿ ������
    private float wait = 1f;
    //��� ������
    private float defenceTime = 2F;
    //�̵� ������
    private float moveTime = .5F;

    //���������� �Ǻ�
    public bool isAttack = false;
    //���������
    public bool isDefence = false;
    //�̵�������
    public bool isMove = false;

    public bool isBack = false;

    Vector2 firstPoint;

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
        firstPoint = transform.position;
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
                        //��ƾ ����
                        isDefence = true;
                        routine = StartCoroutine(MonsterRoutine());
                    }
                }
            }

            //�÷��̾� Ÿ�� �����߿� �׻� ���� ������ ����
            if (currDelay < attackDelay)
            {
                currDelay += Time.deltaTime;
            }

            if (!isStun)
            {
                //�̵� �����ϰ��
                if (isMove)
                {
                    //��ǥ ���� ȸ��
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

    public void Hit(int damage, int direction)
    {
        //�������϶��� ���� ����


        //�÷��̾�� ���ʿ� ���� ��(-1) ����(-1) ���� ���� ���� 
        if (isAttack || direction * (int)this.direction < 0)
        {
            base.Hit(damage, direction);
            Debug.Log(!isAttack);
            Debug.Log(direction);
            Debug.Log((int)this.direction);
            //��� ����
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
                    Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + ((int)direction * Vector3.right * 4), new Vector2(8, 28), 0);

                    //hit�迭�� ��� ����
                    foreach (Collider2D hit in hits)
                    {
                        //player �����
                        if (hit.tag == "Player" && hit.transform.position.y - transform.position.y <= 2)
                        {
                            enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                            isAttack = true;
                            isMove = false;
                            isDefence = false;
                            isBack = false;
                            AttackStart();
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

                        if (Mathf.Abs(transform.position.x - firstPoint.x) >= 2.5f && !isBack)
                        {
                            isBack = true;
                        }
                        continue;
                    }


                }

                //���� ���� �ƴҰ��
                if (!isAttack)
                {
                    //�̵�
                    isMove = true;
                    yield return new WaitForSeconds(moveTime);
                    enemyRigidbody.velocity = new Vector2(0, enemyRigidbody.velocity.y);
                    isMove = false;

                    //���Ÿ��
                    yield return new WaitForSeconds(defenceTime);

                }

                yield return new WaitForEndOfFrame();
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(transform.position + ((int)direction * Vector3.right * 4), new Vector2(8, 28));

    }

    //�̵� ������
    public override void Move()
    {
        int directionResult = 1;

        if (isBack)
        {
            if (Mathf.Abs(firstPoint.x-transform.position.x) > 0)
            {
                directionResult = -1;
            }
            else if (Mathf.Abs(firstPoint.x - transform.position.x) > 0)
            {
                directionResult = 1;
            }
        }

        else if (Random.Range(0, 10) < 3)
        {
            directionResult = -1;
        }

        float enemyMoveResult = enemySpeed * (int)direction * directionResult;

        if (isMovingPlatform)
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * (int)direction + platformBody.velocity.x, enemyRigidbody.velocity.y);
        }
        //�⺻ �÷��������� ������
        else
        {
            enemyRigidbody.velocity = new Vector2(enemySpeed * (int)direction, enemyRigidbody.velocity.y);
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
        yield return new WaitForSeconds(wait);
        isAttack = false;
        isDefence = true;
    }
}
