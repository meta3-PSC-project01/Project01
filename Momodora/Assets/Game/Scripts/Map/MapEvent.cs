using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : MonoBehaviour
{
    public bool canActive = true;
    public int[] position;
    public string eventName;

    public void SetActive()
    {
        IEventControl[] events = transform.GetChild(0).GetChild(0).GetChild(1).GetComponentsInChildren<IEventControl>();
        if (events != null && events.Length > 0)
        {
            foreach (var _event in events)
            {
                _event.SetEventPossible();
            }
        }
        events = transform.GetChild(0).GetChild(0).GetChild(2).GetComponentsInChildren<IEventControl>();
        if (events != null && events.Length > 0)
        {
            foreach (var _event in events)
            {
                _event.SetEventPossible();
            }
        }
    }

    public MapEvent(bool eventPossible, int stage, int number, string name)
    {
        canActive = eventPossible;
        position = new int[2] { stage, number };
        eventName = name;
    }

}
