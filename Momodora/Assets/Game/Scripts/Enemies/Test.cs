using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private Collider2D monsterCollider;
    private Rigidbody2D monsterRigidbody;

    private int monsterHp = default;

    void Awake()
    {
        monsterCollider = GetComponent<Collider2D>();
        monsterRigidbody = GetComponent<Rigidbody2D>();

        monsterHp = 100;
    }

    public void Hit(int damage, int location)
    {
        monsterHp -= damage;

        Debug.Log(monsterHp);
    }
}
