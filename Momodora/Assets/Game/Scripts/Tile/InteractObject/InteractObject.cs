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

    private void Awake()
    {

        SetEventPossible();
    }


    private void Start()
    {
        /*
        아이템 먹을때 쓸거
        
            if (!GameManager.instance.eventManager.eventCheck.ContainsKey(GameManager.instance.currMap.name.Split("(Clone)")[0]))
            {
                MapEvent _event = GameManager.instance.currMap.GetComponent<MapEvent>().Copy();
                _event.canActive = false;
                GameManager.instance.eventManager.eventCheck.Add(GameManager.instance.currMap.name.Split("(Clone)")[0], _event);
            }
         */
        if (interactObjectType == InteractObjectType.ITEM)
        {
            isActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
            if (!isActive)
            {
                GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    private void Update()
    {
        if (interactObjectType == InteractObjectType.ITEM)
        {

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
            collision.GetComponentInParent<PlayerMove>().currInteract = this;
            popupText.OpenPopup(str);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInParent<PlayerMove>().SetInteraction(InteractObjectType.CLOSE);
            collision.GetComponentInParent<PlayerMove>().currInteract = null;
            popupText.ClosePopup();
        }
    }

    public void SetEventPossible()
    {
        if (interactObjectType == InteractObjectType.ITEM)
        {

            if (GameManager.instance.eventManager.eventCheck.ContainsKey(GameManager.instance.nextMapName))
            {
                isActive = GameManager.instance.eventManager.eventCheck[GameManager.instance.nextMapName].canActive;
            }
            else
            {
                isActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
            }
        }
        else if(interactObjectType == InteractObjectType.NPC)
        {
            //npc 대사 스크립트 변경
        }

    }
}
