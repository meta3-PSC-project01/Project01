using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items
{
    public string name = default;
    public string effect = default;
    public string[] explanation = new string[5];
    public int explanationX = default;
    public int itemImage = default;
    public ItemType type;

    public virtual void Init()
    {
        /* Empty */
    }

    public virtual void Print()
    {
        /* Empty */
    }

    public virtual void Use()
    {
        /* Empty */
    }
}

public enum ItemType
{
    ACTIVE,
    DURATION,
    KEY
}
