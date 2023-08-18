using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundRolling1_3 : MonoBehaviour
{
    private float speed = default;

    void Awake()
    {
        speed = 0.5f;
    }


    void Update()
    {
        float xInput = Input.GetAxis("Horizontal");
        float zInput = Input.GetAxis("Vertical");
        float xSpeed = xInput * speed * Time.deltaTime;
        float zSpeed = zInput * speed * Time.deltaTime;
        Vector3 newvelocity = new Vector3(xSpeed, 0f, zSpeed);
        transform.Translate(Vector3.right * -xSpeed);
    }
}
