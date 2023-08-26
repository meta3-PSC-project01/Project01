using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class partRotation : MonoBehaviour
{
    public float speed;

    private Vector3 direction;
    public Transform target;
    
    
    void Update()
    {
        direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, speed * Time.deltaTime);
        
    }
}
