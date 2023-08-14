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
    private bool isReverse = false;

    private TestPlayer player;
    private Rigidbody2D rb;
    private CompositeCollider2D comCollider;

    private void Awake()
    {
        player = FindObjectOfType< TestPlayer >();
        startPos = transform.position;
        endPos = new Vector2(transform.position.x, transform.position.y) + (new Vector2(dx[(int)direction], dy[(int)direction]) * distance);
        rb = GetComponent<Rigidbody2D>();
        comCollider = GetComponent<CompositeCollider2D>();
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
        }
        else if (Vector2.Distance(transform.position, startPos) < .05f)
        {
            targetPos = endPos;
            GetDirectionPos();
        }

    }

    private void FixedUpdate()
    {
        rb.velocity = directionPos * speed;
    }

    private void GetDirectionPos()
    {
        directionPos = (targetPos - transform.position).normalized;
    } 

    private void OnCollisionEnter2D(Collision2D collision)
    {
        foreach(var tmp in collision.contacts)
        {
            Debug.Log(collision.contacts[0].point.x + "  " + comCollider.bounds.min + "/" + comCollider.bounds.max +"/" + comCollider.bounds.size);

        }

        if (collision.transform.tag == "Player" &&
            (collision.contacts[0].point.y>transform.position.y) &&
            comCollider.bounds.min.x+.01f < collision.contacts[0].point.x &&
            comCollider.bounds.max.x + .01f > collision.contacts[0].point.x)

        {
            Debug.Log("in");
            player.isMovingPlatform = true;
            player.platformBody = rb;
            player.playerRigidbody.gravityScale *= 4;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" && player.isMovingPlatform)
        {
            Debug.Log("out");
            player.isMovingPlatform = false;
            player.playerRigidbody.gravityScale *= .25f;

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