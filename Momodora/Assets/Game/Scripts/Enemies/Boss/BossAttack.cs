using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMove player = collision.GetComponentInParent<PlayerMove>();
            player.Hit(damage, 1);            
        }        
    }
}
