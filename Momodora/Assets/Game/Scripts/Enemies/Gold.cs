using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gold : MonoBehaviour
{
    private void Awake()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Gold"), LayerMask.NameToLayer("Gold"), true);
    }
}
