using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject inventoryUi;

    public bool lookAtInventory = false;

    void Awake()
    {
        if (instance == null || instance == default)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
