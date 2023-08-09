using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public GameObject inventoryUi;

    public bool lookAtInventory = false;
    public bool inventoryCheckTime = false;

    // 아이템 관리 베이스
    Dictionary<string, Items> itemDataBase = new Dictionary<string, Items>();

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

    void Start()
    {
        StartCoroutine(InventoryCheck());

        itemDataBase.Add("[None]", new None());
        itemDataBase.Add("등가의 훈장", new Items1());
        itemDataBase.Add("세공 반지", new Items2());
        itemDataBase.Add("아스트랄 부적", new Items3());
        itemDataBase.Add("초롱꽃", new Items4());
    }

    // 아이템 획득 시 정보 받기 (2)
    public Items ItemData(string name)
    {
        if (itemDataBase.ContainsKey(name))
        {
            Items item = itemDataBase[name];

            return item;
        }
        else
        {
            return null;
        }
    }

    IEnumerator InventoryCheck()
    {
        inventoryCheckTime = true;
        ItemManager.instance.GetComponent<Inventory>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        ItemManager.instance.GetComponent<Inventory>().enabled = false;
        inventoryCheckTime = false;

    }
}
