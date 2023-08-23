using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackReactionController : ControlBase, IHitControl
{
    public void Hit(int damage, int direction)
    {
        if (IsHitPossible())
        {
            PlayEvent();

            //¿Ã∫•∆Æ dictionary on/off
        }
    }

    public void HitReaction(int direction)
    {
    }

    public bool IsHitPossible()
    {
        return !isPlayEnd;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Arrow"))
        {
            Hit(0,0);
        }

    }
}
