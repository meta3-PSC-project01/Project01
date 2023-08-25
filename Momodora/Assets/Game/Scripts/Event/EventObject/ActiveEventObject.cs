using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEventObject : MonoBehaviour, IEventTilePlay
{
    public GameObject target;

    public bool isPlaying = false;

    public void Play(ControlBase controller)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            target.SetActive(true);
        }
    }

}
