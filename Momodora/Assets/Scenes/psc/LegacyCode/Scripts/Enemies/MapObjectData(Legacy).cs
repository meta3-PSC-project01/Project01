using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectData_Legacy : MonoBehaviour
{
    public bool isActive;
    public ObjectType_Legacy type;
}


public enum ObjectType_Legacy
{
    Enemy, Interaction
}