using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ControlBase : MonoBehaviour
{
    public List<IEventPlay> mEvents;

    //1회용인지 아닌지
    public bool isPreserve = false;

    //실행했는지 안했는지
    public bool isPlay = false;

    //모드 기본값 0
    public int mode = 0;

    private void Awake()
    {
        mEvents = new List<IEventPlay>();

        mEvents.AddRange(transform.parent.GetComponentsInChildren<IEventPlay>().ToList());
    }

    protected virtual void PlayEvent()
    {
        foreach (var mEvent in mEvents) 
        {
            mEvent.Play(this);
        }
    }
}
