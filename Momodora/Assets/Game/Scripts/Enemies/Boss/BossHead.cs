using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHead : MonoBehaviour
{
    public List<BossBody> parts = new List<BossBody>();
    public List<GameObject> points;

    int count =0;
    int maxCount = 0;
    private void Awake()
    {
        maxCount = points.Count * 100;
    }

    private void Update()
    {
        count += 1;

        for (int i = 0; i < parts.Count; i++)
        {
            int tmp = count;
            if(parts[i].currPoint ==-1 && count >= i * Time.fixedDeltaTime )
            {
                parts[i].currPoint = 0;
            }
            else
            if(parts[i].currPoint ==-1 && count < i * Time.fixedDeltaTime )
            {
                continue;
            }

            if (parts[i].currPoint >=0 && parts[i].currPoint < points.Count-1)
            {
                parts[i].box.transform.position = Vector2.Lerp(points[parts[i].currPoint].transform.position, points[parts[i].currPoint+1].transform.position, (count/(1+ parts[i].currPoint))/100);X`
                parts[i].transform.rotation = points[parts[i].currPoint].transform.rotation;
                
            }
        }
    }
}
