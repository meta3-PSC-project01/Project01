using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMove : MonoBehaviour
{
    private Rigidbody2D arrowRigidbody = default;

    private float arrowSpeed = default;

    void Awake()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();

        arrowSpeed = 30f;
    }

    void Start()
    {
        arrowRigidbody = GetComponent<Rigidbody2D>();
        arrowRigidbody.AddForce(arrowSpeed * transform.right, ForceMode2D.Impulse);

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
