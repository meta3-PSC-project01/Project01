using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossWall : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("!");
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            Debug.Log("?");
            transform.parent.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
    }
}
