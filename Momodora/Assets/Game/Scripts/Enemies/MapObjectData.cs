using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapObjectData : MonoBehaviour
{
    public bool isActive;
    public ObjectType type;
}


public enum ObjectType
{
    Enemy, Interaction
}