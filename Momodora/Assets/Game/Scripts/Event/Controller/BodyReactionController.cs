using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyReactionController : ControlBase
{
    // BoxCollider2D collider;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (canActive)
        {
            PlayEvent();

            canActive = false;
        }
    }
}
