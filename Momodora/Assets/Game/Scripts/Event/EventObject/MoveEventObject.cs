using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEventObject : MonoBehaviour, IEventPlay
{
    public Vector3 startPos;
    public Vector3 endPos;
    public float playTime;
    public BoxCollider2D boxCollider2D;

    private void Awake()
    {
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    public void Play()
    {
        StartCoroutine(MoveObjectRoutine());
    }

    IEnumerator MoveObjectRoutine()
    {
        float time = 0;
        while (time == playTime)
        {
            time += Time.deltaTime;
            if(time > playTime)
            {
                time = playTime;
            }

            transform.position = Vector2.Lerp(startPos, endPos, time/ playTime);

            yield return Time.deltaTime;
        }
    }
}
