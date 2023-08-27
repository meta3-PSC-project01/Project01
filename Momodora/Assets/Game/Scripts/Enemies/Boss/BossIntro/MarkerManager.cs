using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerManager : MonoBehaviour
{
    public class Marker
    {
        public Vector3 position;
        public Quaternion rotation;


        public Marker(Vector3 position, Quaternion rotation)
        {
            this.position = position;
            this.rotation = rotation;
        }

    }

    public List<Marker> markers = new List<Marker>();
    public GameObject nextSegment;

    private void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (gameObject.name.Split("(Clone)")[0] != "Tail")
        {
            Debug.Log(gameObject.name + "(" + markers.Count + ")");
            {
                UpdateMarkerList();
            }
        }
    }

    public void UpdateMarkerList()
    {
        markers.Add(new Marker(transform.position, transform.rotation));
    }

    public void ClearMarkerList()
    {
        markers.Clear();
        markers.Add(new Marker(transform.position, transform.rotation));

    }
    public void SetNextSegment(GameObject Segment)
    {
        nextSegment = Segment;
    }

}
