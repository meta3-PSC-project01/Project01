using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractObject : MonoBehaviour, IEventControl
{
    public InteractObjectType interactObjectType;
    public PopupText popupText;
    public string str;

    public bool isActive = true;



    private void Start()
    {
        if (interactObjectType == InteractObjectType.ITEM)
        {
            isActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
            if (!isActive)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

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

    public void SetEventPossible()
    {
        if (interactObjectType == InteractObjectType.ITEM)
        {
            isActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
            if (!isActive)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
        else if(interactObjectType == InteractObjectType.NPC)
        {
            //npc 대사 스크립트 변경
        }

    }
}
