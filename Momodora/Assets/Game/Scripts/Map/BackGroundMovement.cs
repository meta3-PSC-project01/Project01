using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMovement : MonoBehaviour
{
    public bool isUnder = false;
    public Vector2 rate = Vector2.one;
    PlayerMove player;
    Vector2 playerMove;

    private void Awake()
    {
        player = FindObjectOfType<PlayerMove>();
        playerMove = player.transform.position;
    }
    public void ResetPlayer()
    {
        playerMove = player.transform.position;
    }

    // Update is called once per frame
    public void Move(Vector2 position)
    {
        transform.position = new Vector2(transform.position.x+(position.x) *rate.x, transform.position.y+(position.y) * rate.y);
    }

    private void Update()
    {
        if(isUnder)
            transform.position = new Vector3(Camera.main.transform.position.x, transform.position.y);
    }
}
