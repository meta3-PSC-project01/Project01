using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTile : MonoBehaviour
{
    private int[] dy = { 0, 0, 1, -1,0};
    private int[] dx = { 1, -1, 0, 0,0};

    public DirectionAxis externDirection = DirectionAxis.NONE;
    private List<GameObject> childBodyList = new List<GameObject>();

    public GameObject body;
    public int childCount;

    public List<GameObject> GetList()
    {
        return childBodyList;
    }
    public void RemoveLastIndex()
    {
        if (childBodyList.Count == 0) return;

        DestroyImmediate(childBodyList[childBodyList.Count - 1]);
        childBodyList.RemoveAt(childBodyList.Count - 1);
        childCount -= 1;
    }


    public float speed;
    public float roopTime;  
    public int distance;
    public DirectionAxis direction;

    public Vector2 startPos;
    public Vector2 endPos;
    private Vector3 targetPos;
    private Vector2 directionPos;

    private PlayerMove player;
    private Rigidbody2D rb;
    private CompositeCollider2D comCollider;

    public float time;

    private void Awake()
    {
        player = FindObjectOfType< PlayerMove >();
        startPos = transform.position;
        endPos = new Vector2(transform.position.x, transform.position.y) + (new Vector2(dx[(int)direction], dy[(int)direction]) * distance);
        rb = GetComponent<Rigidbody2D>();
        comCollider = GetComponent<CompositeCollider2D>();
        time = roopTime;
    }

    private void Start()
    {
        targetPos = endPos;
        GetDirectionPos();
    }

    private void Update()
    {
        if ( Vector2.Distance(transform.position, endPos) < .05f )
        {
            targetPos = startPos;
            GetDirectionPos();
            if (time <= roopTime)
            {
                directionPos = Vector2.zero;
            }
        }
        else if (Vector2.Distance(transform.position, startPos) < .05f)
        {
            targetPos = endPos;
            GetDirectionPos();
            if (time <= roopTime)
            {
                directionPos = Vector2.zero;
            }
        }
        else
        {
            if (time >= roopTime)
            {
                time = 0;
            }
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = directionPos * speed;
        time += Time.fixedDeltaTime;
    }

    private void GetDirectionPos()
    {
        directionPos = (targetPos - transform.position).normalized;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "FloorDetectCollider")
        {
            player.playerRigidbody.gravityScale *= 5;
            if (rb != null)
            {
                player.isMovingPlatform = true;
                player.platformBody = rb;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.name == "FloorDetectCollider")
        {
            if (rb != null)
            {
                player.isMovingPlatform = false;
            }
            player.playerRigidbody.gravityScale *= .2f;
        }
    }



}

public enum DirectionAxis
{
    RIGHT = 0,
    LEFT = 1,
    UP = 2,
    DOWN = 3,
    NONE
}