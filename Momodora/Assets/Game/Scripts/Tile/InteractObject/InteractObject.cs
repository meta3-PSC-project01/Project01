using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractObject : MonoBehaviour
{
    public InteractObjectType interactObjectType;
    public PopupText popupText;
    public string str;

    public bool isActive = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isActive)
        {
            collision.GetComponentInParent<PlayerMove>().SetInteraction(interactObjectType);
            popupText.OpenPopup(str);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isActive)
        {
            collision.GetComponentInParent<PlayerMove>().SetInteraction(interactObjectType);
            popupText.ClosePopup();
        }
    }
}
