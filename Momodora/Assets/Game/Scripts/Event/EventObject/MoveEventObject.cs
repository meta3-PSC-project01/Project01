using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventObject : MonoBehaviour, IEventPlay
{
    public Transform start;
    public Transform end;
    private Vector3 startPos;
    private Vector3 endPos;
    public float playTime;
    public BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
        startPos = start.position;
        endPos = end.position;
    }

    public void Play(int mode)
    {
        StartCoroutine(MoveObjectRoutine(mode));
    }

    IEnumerator MoveObjectRoutine(int mode)
    {
        float time = 0;
        while (time != playTime)
        {
            time += Time.deltaTime;
            if(time > playTime)
            {
                time = playTime;
            }

            if (mode == 0)
            {
                transform.position = Vector2.Lerp(startPos, endPos, time / playTime);

            }
            else if (mode == 1)
            {
                transform.position = Vector2.Lerp(endPos, startPos, time / playTime);
            }
            yield return Time.deltaTime;
        }
    }
}
