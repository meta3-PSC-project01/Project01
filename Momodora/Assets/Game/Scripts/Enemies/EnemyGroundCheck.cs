using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGroundCheck : MonoBehaviour
{
    EnemyBase enemy;

    private void Awake()
    {
        enemy = GetComponentInParent<EnemyBase>();
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Debug.Log(enemy.name + " ���ٴ�");
            enemy.isGround = true;
            enemy.StopWhenDash();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 9)
        {
            Debug.Log(enemy.name + " ����");
            enemy.isGround = false;
        }

    }
}
