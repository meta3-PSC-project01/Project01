using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
        player = FindObjectOfType<PlayerMove>();
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

    PlayerMove player ;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "FloorDetectCollider")
        {
            Debug.LogWarning(player.playerRigidbody.gravityScale);
            player.playerRigidbody.gravityScale *= 5;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "FloorDetectCollider")
        {
            Debug.LogWarning(player.playerRigidbody.gravityScale);
            player.playerRigidbody.gravityScale *= .2f;
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
            if (transform.name != "Elavater")
            {
                CameraMove.ShakingCamera(Camera.main, .1f, .2f);
            }
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
            controller.canActive = true;
        }
    }
}
