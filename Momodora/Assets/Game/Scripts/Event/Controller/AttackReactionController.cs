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
            if (!GameManager.instance.eventManager.eventCheck.ContainsKey(GameManager.instance.currMap.name.Split("(Clone)")[0]))
            {
                MapEvent _event = GameManager.instance.currMap.GetComponent<MapEvent>().Copy();
                _event.canActive = false;
                GameManager.instance.eventManager.eventCheck.Add(GameManager.instance.currMap.name.Split("(Clone)")[0], _event);
            }
        }
    }

    public void HitReaction(int direction)
    {
    }

    public bool IsHitPossible()
    {
        return canActive;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.tag == "Arrow"))
        {
            Hit(0,0);
        }

    }
}
