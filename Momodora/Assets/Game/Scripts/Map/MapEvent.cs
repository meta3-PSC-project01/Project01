using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : MonoBehaviour
{
    public bool canActive = true;
    public int[] position;
    public string eventName;

    public MapEvent()
    {
        canActive = true;
    }

    public MapEvent(bool eventPossible, int stage, int number, string name)
    {
        canActive = eventPossible;
        position = new int[2] { stage, number };
        eventName = name;
    }

    public MapEvent Copy()
    {
        return (MapEvent)this.MemberwiseClone();
    }

}
