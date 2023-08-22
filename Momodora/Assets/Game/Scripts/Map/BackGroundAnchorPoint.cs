using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundAnchorPoint : MonoBehaviour
{
    public void ResetPosition(Vector2 position)
    {
        int count = 1;
        foreach(var tmp in GetComponentsInChildren<BackGroundMovement>())
        {
            tmp.transform.localPosition = position*count;
            count += 1;
        }
    }

    public void MoveBackground(Vector2 position)
    {
        foreach (var tmp in GetComponentsInChildren<BackGroundMovement>())
        {
            tmp.Move(position);
        }
    }
}
