using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventObject : MonoBehaviour, IEventTilePlay
{
    public Transform start;
    public Transform end;
    private Vector3 startPos;
    private Vector3 endPos;
    public float playTime;
    public bool isPlaying = false;
    public BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        startPos = start.position;
        endPos = end.position;
    }

    public void Play(ControlBase controller)
    {
        if (!isPlaying)
        {
            isPlaying = true;
            StartCoroutine(MoveObjectRoutine(controller));
        }
    }

    IEnumerator MoveObjectRoutine(ControlBase controller)
    {
        float time = 0;
        Vector2 _endPos = endPos;
        Vector2 _startPos = startPos;

        if (controller.mode == 0)
        {
            _endPos = endPos;
            _startPos = startPos;

        }
        else if (controller.mode == 1)
        {
            _endPos = startPos;
            _startPos = endPos;
        }


        while (time != playTime)
        {
            time += Time.deltaTime;
            if(time > playTime)
            {
                time = playTime;
            }

            transform.position = Vector2.Lerp(_startPos, _endPos, time / playTime);

            yield return Time.deltaTime;
        }

        isPlaying = false;        

        if (controller.isPreserve)
        {
            controller.isPlayEnd = false;
        }
    }
}
