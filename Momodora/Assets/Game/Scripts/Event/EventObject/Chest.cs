using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

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
        isActive = true;
        open = transform.Find("OpenSprite").GetComponent<SpriteRenderer>();
        close = transform.Find("CloseSprite").GetComponent<SpriteRenderer>();
        box = GetComponent<BoxCollider2D>();
        SetStatus();
        foreach(var tmp in FindObjectOfType<PlayerMove>().GetComponentsInChildren<BoxCollider2D>())
        {
            Physics2D.IgnoreCollision(transform.GetComponent<BoxCollider2D>(), tmp);

        }
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
            close.gameObject.SetActive(false);
            open.gameObject.SetActive(true);
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
                MapEvent _event = GameManager.instance.currMap.GetComponent<MapEvent>().Copy();
                _event.canActive = false;
                GameManager.instance.eventManager.eventCheck.Add(GameManager.instance.currMap.name.Split("(Clone)")[0], _event);
                
                Dead();
                return;
            }
        }
    }

    //몬스터 hit시 반응
    //기본은 색 바뀌기
    public void HitReaction(int direction)
    {
        StartCoroutine(HitReactionRoutine());
    }

    IEnumerator HitReactionRoutine()
    {
        for(int i = 0; i < 10; i++)
        {
            transform.localScale = Random.insideUnitCircle + Vector2.one;
            yield return new WaitForSeconds(.01f);
            transform.localScale = Vector2.one;
        }
        transform.localScale = Vector2.one;
    }

    //몬스터 죽을시
    //추후 확장성을 위해서 virtual로 지정(죽을때 효과있는 몬스터)
    public virtual void Dead()
    {

        for (int i = 0; i < goldCount; i++)
        {
            GameObject tmp = Instantiate(gold, transform.position, Quaternion.identity);
            tmp.GetComponent<Rigidbody2D>().AddForce(Random.insideUnitCircle * Random.Range(4f, 5f), ForceMode2D.Impulse);
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
        if (GameManager.instance.eventManager.eventCheck.ContainsKey(GameManager.instance.nextMapName))
        {
            isActive = GameManager.instance.eventManager.eventCheck[GameManager.instance.nextMapName].canActive;
        }
        else
        {
            isActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
        }
    }
}

