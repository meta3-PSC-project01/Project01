using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public int damage;
    public GameObject effect; 
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerMove player = collision.GetComponentInParent<PlayerMove>();
            player.Hit(damage, 1);
            if (effect != null)
            {
                Instantiate(effect, transform.position, Quaternion.identity);
            }
        }
    }
}
