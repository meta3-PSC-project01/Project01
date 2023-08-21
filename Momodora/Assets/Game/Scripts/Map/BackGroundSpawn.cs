using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSpawn : MonoBehaviour
{
    private float widthX;
    private float width_add;

    void Awake()
    {
       // widthX = backGroundCollider.size.x;
        width_add = widthX * 2f;
    }

    void Update()
    {
        if (transform.position.x <= -width_add)
        {
            Reposition();
        }
    }

    private void Reposition()
    {
        Vector2 offset = new Vector2(widthX * 5.22f, 0);
        transform.position = transform.position.AddVector(offset);
    }
}
