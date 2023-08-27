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

    PlayerMove player ;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach (var tmp in collision.contacts)
        {
            // Debug.Log(collision.contacts[0].point.x + "  " + comCollider.bounds.min + "/" + comCollider.bounds.max +"/" + comCollider.bounds.size);

        }

        if (collision.transform.tag == "Player" &&
            (collision.contacts[0].point.y > transform.position.y))

        {

            player = collision.collider.GetComponent<PlayerMove>();
            //Debug.Log("in");
            player.isMovingPlatform = true;
            player.playerRigidbody.gravityScale *= 4;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && player.isMovingPlatform)
        {
            //Debug.Log("out");
            player.isMovingPlatform = false;
            player.playerRigidbody.gravityScale *= .25f;
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
