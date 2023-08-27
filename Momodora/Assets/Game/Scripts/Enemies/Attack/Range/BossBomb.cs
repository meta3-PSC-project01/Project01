using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBomb : MonoBehaviour
{
    Rigidbody2D bulletRigidbody;

    public GameObject poison;
    public bool isBoom = false;

    public float speed = 10;
    int direction;

    public int damage = 10;


    private void Awake()
    {
        bulletRigidbody = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    void Start()
    {
        bulletRigidbody.velocity = new Vector2(-speed, -speed);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        //何婬磨 版快 厘魄康开 积己
        if ((collision.collider.tag == "Player" || collision.gameObject.layer == 9) && !isBoom)
        {
            isBoom = true;
            CameraMove.ShakingCamera(Camera.main, .15f, 1.5f);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(collision.collider.ClosestPoint(transform.position), 1);
            foreach (var collider in colliders)
            {
                if (collider.tag == "Player")
                {
                    //player.hp -= damage;
                    collider.GetComponentInParent<PlayerMove>().Hit(damage, -direction);
                }
            }
            bulletRigidbody.velocity = Vector3.zero;
            bulletRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            Instantiate(poison, transform.position, Quaternion.identity, GameManager.instance.currMap.transform);
            Destroy(gameObject);
        }
    }

}
