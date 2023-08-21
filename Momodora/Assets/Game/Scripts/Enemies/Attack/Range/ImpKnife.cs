using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpKnife : EnemyAttackData
{
    Rigidbody2D bulletRigidbody;
    BoxCollider2D bulletCollider;

    public float speed = 10;

    public override void UseEffect()
    {
        animator.SetTrigger("Attack");
    }
    public override void UseCollider()
    {
        isActive = true;
    }   

    public override void Init()
    {
        base.Init();
        bulletRigidbody = GetComponent<Rigidbody2D>();
        bulletCollider = GetComponent<BoxCollider2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody.velocity = -1 * transform.right * speed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerStay2D(Collider2D collision)
    {
        if (isActive && collision.tag == "Player")
        {
            isActive = false;

            //맞을경우 뭔가 뜨게하는거 추가
            //플레이어 반응 
            collision.GetComponentInParent<PlayerMove>().Hit(damage, (int)transform.right.x);
        }

        if (isActive && collision.tag == "Floor")
        {
            bulletCollider.isTrigger = false;
            bulletCollider.enabled = false;
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Destroy(gameObject, 5f);
        }

    }
}
