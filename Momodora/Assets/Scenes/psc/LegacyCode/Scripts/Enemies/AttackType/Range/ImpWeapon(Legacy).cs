using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpWeapon_Legacy : EnemyRangeWeapon_Legacy
{
    new Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        Destroy(gameObject, 10f);
    }

    private void Start()
    {
        useWeapon(EnemyDirection_Legacy.RIGHT);
    }

    public override void useWeapon(EnemyDirection_Legacy direction)
    {
        rigidbody.velocity = new Vector2(weaponSpeed*(int)direction, 0f);
        base.useWeapon(direction);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("Player"))
        {
            Destroy(gameObject);
            PlayerMove player = collision.gameObject.GetComponentInParent<PlayerMove>();
            player.playerHp -= weaponDamage;
        }

        if (collision.tag.Equals("Finish"))
        {
            gameObject.tag = "Untagged";
            rigidbody.velocity = Vector2.zero;
            rigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

}
