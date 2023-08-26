using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoveObject : MonoBehaviour, IEventTilePlay
{
    public Transform start;
    public Transform target;

    private Vector3 startPos;
    private Vector3 targetPos;

    public float moveTime;
    public float waitTime;

    public bool uturn = false;
    public bool isPlaying = false;
    

    public void StopRigidBody(Rigidbody2D rb)
    {
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }
    public void PlayRigidBody(Rigidbody2D rb)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    private void Awake()
    {
        startPos = start.position;
        targetPos = target.position;
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
        StopRigidBody(FindObjectOfType<PlayerMove>().GetComponent<Rigidbody2D>());
        Camera mainCamera = Camera.main;
        GameManager.instance.cameraStop = true;

        float time = 0;
        Vector3 _startPos = new Vector3(startPos.x, startPos.y, -10);
        Vector3 _endPos = new Vector3(targetPos.x, targetPos.y, -10);

        while (time != moveTime)
        {
            time += Time.deltaTime;
            if (time > moveTime)
            {
                time = moveTime;
            }

            mainCamera.transform.position = Vector3.Lerp(_startPos, _endPos, time / moveTime);

            yield return new WaitForSeconds(Time.deltaTime);
        }

        yield return new WaitForSeconds(waitTime);

        if (uturn)
        {
            time = 0;
            while (time != moveTime)
            {
                time += Time.deltaTime;
                if (time > moveTime)
                {
                    time = moveTime;
                }


                mainCamera.transform.position = Vector3.Lerp(_endPos, _startPos, time / moveTime);

                yield return new WaitForSeconds(Time.deltaTime);
            }
        }

        isPlaying = false;

        GameManager.instance.cameraStop = false;
        PlayRigidBody(FindObjectOfType<PlayerMove>().GetComponent<Rigidbody2D>());
    }
}
