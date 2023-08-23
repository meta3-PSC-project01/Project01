using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapEvent : MonoBehaviour
{
    public bool canActive = true;
    public int[] position;

    public MapEvent(bool eventPossible, int stage, int number)
    {
        canActive = eventPossible;
        position = new int[2] { stage, number };
    }
}
