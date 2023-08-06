using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Rigidbody2D arrowRigidbody = default;
    public SpriteRenderer arrowRenderer = null;

    public bool flipX = false;

    private float arrowSpeed = default;

    private void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();
        arrowRenderer = GetComponent<SpriteRenderer>();

        arrowSpeed = 30f;
    }

    void Start()
    {
        if (flipX == false)
        {
            Vector2 newVelocity = new Vector2(arrowSpeed, 0f);
            arrowRigidbody.velocity = newVelocity;
        }
        else
        {
            Vector2 newVelocity = new Vector2(-arrowSpeed, 0f);
            arrowRigidbody.velocity = newVelocity;
        }

        Destroy(gameObject, 3f);
    }

    //void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Player")
    //    {
    //        PlayerController playerController = other.GetComponent<PlayerController>();

    //        if (playerController != null)
    //        {
    //            playerController.Die();
    //        }
    //    }
    //}
}
