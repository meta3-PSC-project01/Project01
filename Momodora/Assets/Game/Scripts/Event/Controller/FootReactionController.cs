using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootReactionController : ControlBase
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        List<ContactPoint2D> contactPoint = new List<ContactPoint2D>();
        collision.GetContacts(contactPoint);
        foreach (ContactPoint2D contact in contactPoint)
        {
            if (contact.collider.tag=="Player" && contact.point.normalized.y > .7)
            {
                PlayEvent();
            }
        }
    }
}
