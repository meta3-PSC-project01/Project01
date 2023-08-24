using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootReactionController : ControlBase
{
   // BoxCollider2D collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlayEnd)
        {
            if (collision.tag == "Player")
            {
                PlayEvent();                

                isPlayEnd = true;
                
            }

        }
    }
}
