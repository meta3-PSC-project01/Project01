using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class BossIntroManager : MonoBehaviour
{
    [SerializeField] float distanceBetween = 0.2f;

    [SerializeField] float speed = 280;
    [SerializeField] float turnSpeed = 360;
    [SerializeField] List<GameObject> bodyParts = new List<GameObject>();
    List<GameObject> bodyList = new List<GameObject>();
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
        MoveSegment();

    }

    void CreateBodyParts()
    {
        if (bodyList.Count == 0)
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
            bodyList.Add(head);
        }

        MarkerManager markM = bodyList[bodyList.Count-1].GetComponent<MarkerManager>();
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

            bodyList.Add(tmp);
            bodyParts.RemoveAt(0);
            tmp.GetComponent<MarkerManager>().SetNextSegment(bodyList[bodyList.Count - 1]);
            tmp.GetComponent<MarkerManager>().ClearMarkerList();
            countUp = 0;
        }
    }

    void MoveSegment()
    {
        if (bodyList.Count > 1)
        {
            for (int i = 1; i < bodyList.Count; i++)
            {
                MarkerManager marker = bodyList[i - 1].GetComponent<MarkerManager>();
                 if (marker.markers.Count>=3)
               // if ((snakeBody[i-1].transform.position- snakeBody[i].transform.position).magnitude>=.001)
                {
                    bodyList[i].transform.position = marker.markers[0].position;
                    bodyList[i].transform.rotation = marker.markers[0].rotation;

                   /* Vector3 direction = bodyList[i].GetComponent<MarkerManager>().nextSegment.transform.position - bodyList[i].transform.position;
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    Quaternion rotation = Quaternion.AngleAxis(angle+ bodyList[i].transform.rotation.y, Vector3.forward);
                    bodyList[i].transform.rotation = rotation;*/

                    marker.markers.RemoveAt(0);
                }
            }
        }
    }

}
