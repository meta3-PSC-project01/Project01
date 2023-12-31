using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlBase : MonoBehaviour, IEventControl
{
    public List<IEventTilePlay> mEvents;

    //1회용인지 아닌지
    public bool isPreserve = false;

    //실행했는지 안했는지
    public bool canActive = true;

    //모드 기본값 0
    public int mode = 0;

    private void Awake()
    {
        mEvents = new List<IEventTilePlay>();

        mEvents.AddRange(transform.parent.GetComponentsInChildren<IEventTilePlay>().ToList());

        SetEventPossible();
    }

    private void Start()
    {
        if (!canActive)
        {
            PlayEvent();
        }
    }

    protected virtual void PlayEvent()
    {
        foreach (var mEvent in mEvents) 
        {
            mEvent.Play(this);
        }

        if (mode == 0)
        {
            mode = 1;
        }
        else if (mode == 1)
        {
            mode = 0;
        }

    }

    public void SetEventPossible()
    {
        if (!isPreserve)
        {
            if (GameManager.instance.eventManager.eventCheck.ContainsKey(GameManager.instance.nextMapName))
            {
                canActive = GameManager.instance.eventManager.eventCheck[GameManager.instance.nextMapName].canActive;
            }
            else
            {
                canActive = transform.parent.parent.parent.parent.GetComponent<MapEvent>().canActive;
            }
        }
    }
}
