using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpBomb : EnemyAttackData
{
    Rigidbody2D bulletRigidbody;

    public GameObject poisonFloor;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        int direction;
        if (transform.right.x > 0)
        {
            direction = -1;
        }
        else
        {
            direction = 1;
        }
        GetParabolaAtHeight(new Vector3(2.5f,1.5f), direction, bulletRigidbody);
    }


    public void OnTriggerStay2D(Collider2D collision)
    {
        //부딫칠 경우 장판영역 생성
        if ((collision.tag == "Player" || collision.tag == "Wall" || collision.tag == "Floor"))
        {
            Debug.Log("?");
            CameraMove.ShakingCamera(Camera.main.GetComponent<CameraMove>());
            TestPlayer player = collision.GetComponent<TestPlayer>();
            if (player != null)
            {
                player.hp -= damage;
                //player.Hit();
            }
            Destroy(gameObject, .5f);
            Instantiate(poisonFloor, collision.transform.position, Quaternion.identity);
            //맞을경우 뭔가 뜨게하는거 추가
            //플레이어 반응 
        }


    }


    //그래비티 값이 높을수록 빨라짐
    private void GetParabolaAtHeight(Vector2 maxHeightDisplacement, int direction, Rigidbody2D rigid)
    {

        // m*k*g*h = m*v^2/2 (단, k == gravityScale) <= 역학적 에너지 보존 법칙 적용
        float v_y = Mathf.Sqrt(2 * rigid.gravityScale * -Physics2D.gravity.y * maxHeightDisplacement.y);
        // 포물선 운동 법칙 적용
        float v_x = direction * maxHeightDisplacement.x * v_y / (2 * maxHeightDisplacement.y);

        Vector2 force = rigid.mass * (new Vector2(v_x, v_y) - rigid.velocity);
        rigid.velocity = force;
    }
}
