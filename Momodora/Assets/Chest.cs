using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour, IHitControl, IEventControl
{
    bool isActive;
    SpriteRenderer open;
    SpriteRenderer close;
    BoxCollider2D box;
    public int chestHp=6;
    public int goldCount=30;

    public GameObject gold;

    private void Awake()
    {
        open = transform.Find("OpenSprite").GetComponent<SpriteRenderer>();
        Debug.Log(open);
        close = transform.Find("CloseSprite").GetComponent<SpriteRenderer>();
        Debug.Log(close);
        box = GetComponent<BoxCollider2D>();
        SetStatus();
    }

    private void SetStatus()
    {
        SetEventPossible();

        if (isActive)
        {
            close.gameObject.SetActive(true);
            open.gameObject.SetActive(false);
        }
        else
        {
            Dead();
        }
    }
    public void Hit(int damage, int direction)
    {
        if (chestHp > 0)
        {
            chestHp -= damage;
            HitReaction(direction);

            if (chestHp <= 0)
            {
                Dead();
                return;
            }
        }
    }

    //���� hit�� ����
    //�⺻�� �� �ٲ��
    public void HitReaction(int direction)
    {
        //�� �ٲ�� ���׼�
    }

    //���� ������
    //���� Ȯ�强�� ���ؼ� virtual�� ����(������ ȿ���ִ� ����)
    public virtual void Dead()
    {
        GameManager.instance.eventManager.eventCheck[GameManager.instance.currMap.name].canActive = false;

        for (int i = 0; i < goldCount; i++)
        {
            GameObject tmp = Instantiate(gold, transform.position, Quaternion.identity);
            tmp.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(8f, 10f) * ((Random.Range(0, 2) == 0) ? -1 : 1), -Random.Range(6f, 8f)), ForceMode2D.Impulse);
            Destroy(tmp,3f);
        }

        close.gameObject.SetActive(false);
        open.gameObject.SetActive(true);
        isActive = false;
    }

    public bool IsHitPossible()
    {
        return isActive;
    }

    public void SetEventPossible()
    {
        isActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
    }
}

