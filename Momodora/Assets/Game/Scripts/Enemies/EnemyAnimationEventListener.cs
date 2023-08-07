using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyAnimationEventListener : MonoBehaviour
{
    public UnityEvent[] events;

    void CallEvent(int index)
    {
        events[index].Invoke();
    }
}
