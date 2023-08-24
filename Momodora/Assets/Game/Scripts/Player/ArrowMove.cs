using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Rigidbody2D arrowRigidbody;
    private BoxCollider2D arrowCollider;
    private GameObject monster;
    public GameObject arrowEffect;

    public int damage = 1;

    private float arrowSpeed = default;

    void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();
        arrowCollider = GetComponent<BoxCollider2D>();

        arrowSpeed = 30f;
    }

    void Start()
    {
        arrowRigidbody.AddForce(arrowSpeed * transform.right, ForceMode2D.Impulse);

        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Enemy")
        {
            monster = collider.gameObject;
            if(monster.GetComponentInParent<IHitControl>().IsHitPossible())
            {
                monster.GetComponentInParent<IHitControl>().Hit(damage, -(int)transform.right.x);
                GameObject arrowEffect_ = Instantiate(arrowEffect, monster.transform.position, Quaternion.identity);
                this.gameObject.SetActive(false);
                Destroy(this.gameObject, 1f);
            }
        }
    }
}
