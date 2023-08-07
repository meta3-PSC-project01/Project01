using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public string name;
    public ItemType type;

    public string title = default;
    public string effect = default;
    public string[] explanation = new string[5];
    public int explanationX = default;

    public virtual void Init()
    {
        /*empty*/
    }

    public virtual void Print()
    { 
        /*empty*/ 
    }

    public virtual void Use()
    {
        /*empty*/
    }
}

public enum ItemType
{
    ACTIVE,
    DURATION,
    KEY
}
