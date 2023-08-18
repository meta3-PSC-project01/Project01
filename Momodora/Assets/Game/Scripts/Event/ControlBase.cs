using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlBase : MonoBehaviour
{
    public IEventPlay mEvent;

    //1회용인지 아닌지
    public bool isPreserve = false;

    //실행했는지 안했는지
    public bool isPlay = false;

    protected virtual void PlayEvent()
    {
        mEvent.Play();
    }
}
