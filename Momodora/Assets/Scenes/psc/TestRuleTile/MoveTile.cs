using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    private int[] dy = { 0, 0, 1, -1,0};
    private int[] dx = { 1, -1, 0, 0,0};

    public DirectionAxis externDirection = DirectionAxis.NONE;

    public float speed;
    public float roopTime;  //time 초마다
                        //양쪽으로 반복
    public int distance;
    public DirectionAxis direction;

    private Vector2 startPos;
    private Vector2 endPos;
    private bool isReverse = false;

    public GameObject body;
    public int childCount;
    private List<GameObject> childBodyList = new List<GameObject>();

    public List<GameObject> GetList()
    {
        return childBodyList;
    }
    public void RemoveLastIndex()
    {
        if (childBodyList.Count == 0) return;

        childBodyList.RemoveAt(childBodyList.Count - 1);
        childCount -= 1;
    }

    private void Awake()
    {
        isReverse = true;
        startPos = transform.position;
        endPos = new Vector2(transform.position.x, transform.position.y) + (new Vector2(dx[(int)direction], dy[(int)direction]) * distance);
    }

    private void Start()
    {
        StartCoroutine(TileMove());
    }

    IEnumerator TileMove()
    {
        float time = 0f;
        while (true)
        {
            time += speed * Time.deltaTime/ roopTime;

            if (isReverse)
            {
                transform.position = Vector2.Lerp(startPos, endPos, time);
            }
            else
            {
                 transform.position = Vector2.Lerp(endPos, startPos, time);
            }

            yield return new WaitForSeconds(Time.deltaTime);

            if(time >= roopTime)
            {
                time = 0f;
                isReverse = !isReverse;
               
            }
        
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Enemy")
        {
            collision.transform.SetParent(transform);
            //colliders.Add(collision);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.tag == "Player" || collision.transform.tag == "Enemy")
        {
            collision.transform.SetParent(null);
            //colliders.Remove(collision);
        }
    }
}

public enum DirectionAxis
{
    EAST = 0,
    WEST = 1,
    NORTH = 2,
    SOUTH = 3,
    NONE
}