using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public GameObject inventoryUi;
    public Items[] equipItems = new Items[5];
    //ȹ���� �������� �� �̰��� ����
    public List<Items> activeItems;
    public List<Items> durationItems;

    public int leaf = default;
    public int activeItemNum = default;
    public int[] activeItemCount = default;

    public bool lookAtInventory = false;
    public bool lookAtGameMenu = false;
    public bool inventoryCheckTime = false;
    public bool[] equipCheck = new bool[5];

    // ������ ���� ���̽�
    Dictionary<string, Items> itemDataBase = new Dictionary<string, Items>();

    //public static ItemManager CreateInstance()
    //{
    //    GameObject manager = new GameObject("ItemManager");
    //    instance = manager.AddComponent<ItemManager>();

    //    if (instance == null || instance == default)
    //    {
    //        return instance;
    //    }
    //    else
    //    {
    //        Destroy(manager);
    //        return instance;
    //    }
    //} 

    void Awake()
    {
        if (instance == null || instance == default) { instance = this; DontDestroyOnLoad(instance.gameObject); }
        else { Destroy(gameObject); }

        for (int i = 0; i < 5; i++) { equipCheck[i] = false; }

        activeItems = new List<Items>();
        durationItems = new List<Items>();

        leaf = 0;
        activeItemNum = 0;
    }

    void Start()
    {
        //StartCoroutine(InventoryCheck());

        itemDataBase.Add("[None]", new None());
        itemDataBase.Add("��� ����", new Items1());
        itemDataBase.Add("���� ����", new Items2());
        itemDataBase.Add("�ƽ�Ʈ�� ����", new Items3());
        itemDataBase.Add("�ʷղ�", new Items4());
    }

    // ������ ȹ�� �� ���� �ޱ� (2)
    public Items ItemData(string name)
    {
        if (itemDataBase.ContainsKey(name)) { Items item = itemDataBase[name]; return item; }
        else { return null; }
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
