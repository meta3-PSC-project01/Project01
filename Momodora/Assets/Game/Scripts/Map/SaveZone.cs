using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveZone : MonoBehaviour
{
    public InteractObjectType interactObjectType;
    public PopupText popupText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.tag);
        Debug.Log(collision.name);
        if (collision.tag == "Player")
        {
            collision.GetComponentInParent<PlayerMove>().SetInteraction(interactObjectType);
            //텍스트창.open();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInParent<PlayerMove>().SetInteraction(interactObjectType);
            //텍스트창.close();
        }
    }
}
