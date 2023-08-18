using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReactionController : ControlBase
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isPlay && collision.tag == "Arrow")
        {
            PlayEvent();

            if (!isPreserve)
            {
                isPlay = true;
            }
        }
    }
}
