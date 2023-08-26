using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    public GameObject inventoryUi;
    public GameObject playerUi;
    public Items[] equipItems = new Items[5];
    //ȹ���� �������� �� �̰��� ����
    public List<Items> activeItems;
    public List<Items> durationItems;

    public int leaf = default;
    public int[] activeItemNum = new int[5];
    public int activeItemSeleting = default;
    public int[] activeItemCount = default;

    public bool lookAtInventory = false;
    public bool lookAtGameMenu = false;
    public bool inventoryCheckTime = false;
    public bool[] equipCheck = new bool[5];

    // ������ ���� ���̽�
    Dictionary<string, Items> itemDataBase = new Dictionary<string, Items>();

    void Awake()
    {
        if (instance == null || instance == default) { instance = this; DontDestroyOnLoad(instance.gameObject); }
        else { Destroy(gameObject); }

        for (int i = 0; i < 5; i++)
        {
            equipCheck[i] = false;
            activeItemNum[i] = 0;
        }

        activeItems = new List<Items>();
        durationItems = new List<Items>();
        leaf = 0;
        activeItemSeleting = 0;
        activeItemCount[1] = 3;

    }

    void Start()
    {
        StartCoroutine(InventoryCheck());
    }

    // ������ ȹ�� �� ���� �ޱ� (2)
    public Items ItemData(string name, out Items item)
    {
        if (itemDataBase.ContainsKey(name)) 
        {
            item = itemDataBase[name];
            return item;
        }
        else 
        {
            item = null;
            return null;
        }
    }

    IEnumerator InventoryCheck()
    {
        inventoryCheckTime = true;
        ItemManager.instance.GetComponent<Inventory>().enabled = true;

        itemDataBase.Add("[None]", new None());
        itemDataBase.Add("��� ����", new Items1());
        itemDataBase.Add("���� ����", new Items2());
        itemDataBase.Add("�ƽ�Ʈ�� ����", new Items3());
        itemDataBase.Add("�ʷղ�", new Items4());

        yield return new WaitForSeconds(0.5f);

        ItemManager.instance.GetComponent<Inventory>().enabled = false;
        inventoryCheckTime = false;
    }
}
