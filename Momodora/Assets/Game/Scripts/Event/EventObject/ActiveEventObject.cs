using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveEventObject : MonoBehaviour, IEventTilePlay
{
    public GameObject[] targets;

    public bool isPlaying = false;

    public void Play(ControlBase controller)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            foreach(GameObject target in targets)
            {
                target.SetActive(true);
            }
        }
    }

}
