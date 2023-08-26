using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = 0.2f;

    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 360;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> snakeBody = new List<GameObject>();
    public GameObject head;

    float countUp = 0;

    private void Awake()
    {
        CreateBodyParts();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (bodyParts.Count > 0)
        {
            CreateBodyParts();
        }
        SnakeMovement();

    }

    void CreateBodyParts()
    {
        if (snakeBody.Count == 0)
        {
            if (!head.GetComponent<MarkerManager>())
            {
                head.AddComponent<MarkerManager>();
            }

            if (!head.GetComponent<Rigidbody2D>())
            {
                head.AddComponent<Rigidbody2D>();
                head.GetComponent<Rigidbody2D>().gravityScale = 0;
            }
            snakeBody.Add(head);
            bodyParts.RemoveAt(0);
        }

        MarkerManager markM = snakeBody[snakeBody.Count-1].GetComponent<MarkerManager>();
        if (countUp == 0)
        {
            markM.ClearMarkerList();
        }


        countUp += Time.deltaTime;
        if (countUp >= distanceBetween)
        {
            GameObject tmp = Instantiate(bodyParts[0], markM.markers[0].position, markM.markers[0].rotation, transform);
            if (!tmp.GetComponent<MarkerManager>())
            {
                tmp.AddComponent<MarkerManager>();
            }

            if (!tmp.GetComponent<Rigidbody2D>())
            {
                tmp.AddComponent<Rigidbody2D>();
                tmp.GetComponent<Rigidbody2D>().gravityScale = 0;
            }

            snakeBody.Add(tmp);
            bodyParts.RemoveAt(0);
            tmp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }

    void SnakeMovement()
    {
        if (snakeBody.Count > 1)
        {
            for (int i = 1; i < snakeBody.Count; i++)
            {
                MarkerManager marker = snakeBody[i - 1].GetComponent<MarkerManager>();
                 if (marker.markers.Count>=3)
               // if ((snakeBody[i-1].transform.position- snakeBody[i].transform.position).magnitude>=.001)
                {
                    snakeBody[i].transform.position = marker.markers[0].position;
                    snakeBody[i].transform.rotation = marker.markers[0].rotation;
                    marker.markers.RemoveAt(0);
                }
            }
        }
    }

}
