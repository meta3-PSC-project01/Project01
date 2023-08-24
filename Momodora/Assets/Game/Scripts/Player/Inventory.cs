using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public GameObject[] noneSlot = new GameObject[5];
    public GameObject[] actSlot = new GameObject[5];
    public GameObject[] inventoryColor = new GameObject[5];
    public Text[] inventorySlot = new Text[5];
    public Text[] itemString = new Text[5];
    public Image[] itemSpace = new Image[5];
    public Image[] itemImage = new Image[4];
    public Image noneImage;

    private int selectSlot = default;
    private int selectInventory = default;

    private bool lookAtInventorySlot = false;
    private bool itemTypeCheck = false;

    void Start()
    {
        GetItem("[None]");
    }

    // 아이템 획득 시 분류 기능 (1)
    public void GetItem(string name)
    {
        Items item = ItemManager.instance.ItemData(name);
        if (item == null) { return; }

        if (item.itemName == "[None]")
        {
            ItemManager.instance.activeItems.Add(item);
            ItemManager.instance.durationItems.Add(item);

            return;
        }

        if (item.type == ItemType.ACTIVE) { ItemManager.instance.activeItems.Add(item); }
        else if (item.type == ItemType.DURATION) { ItemManager.instance.durationItems.Add(item); }
    }
    
    public void SelectItems()
    {
        //활성 아이템 슬롯
        if (selectSlot <= 2)
        {
            for (int i = 0; i < ItemManager.instance.activeItems.Count; i++) { inventorySlot[i].text = 
                    string.Format(ItemManager.instance.activeItems[i].itemName); }
        }
        else
        {
            for (int i = 0; i < ItemManager.instance.durationItems.Count; i++) { inventorySlot[i].text = 
                    string.Format(ItemManager.instance.durationItems[i].itemName); }
        }

        if (ItemManager.instance.equipCheck[selectSlot] == true)
        {
            for (int i = 0; i < ItemManager.instance.equipItems[selectSlot].explanationX; i++)
            {
                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.equipItems[selectSlot].itemName); }
                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.equipItems[selectSlot].effect); }
                else { itemString[i].text = string.Format(ItemManager.instance.equipItems[selectSlot].explanation[i - 2]); }
            }
        }
        else { for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); } }
    }

    public void CleanSlot()
    {
        if (selectSlot <= 2)
        {
            for (int i = 0; i < inventorySlot.Length; i++) { inventorySlot[i].text = string.Format(" "); }
        }
        else
        {
            for (int i = 0; i < inventorySlot.Length; i++) { inventorySlot[i].text = string.Format(" "); }
        }

        for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }
    }

    //해당 인벤토리 목록에서 확인키를 누를경우 아이템 장착
    public void EquipItem()
    {
        List<Items> list;
        //활성 아이템 슬롯
        if (selectSlot <= 2) { list = ItemManager.instance.activeItems; }
        else { list = ItemManager.instance.durationItems; }

        if (selectInventory == 0)
        {
            if (ItemManager.instance.equipCheck[selectSlot] == true)
            {
                list.Add(ItemManager.instance.equipItems[selectSlot]);
                ItemManager.instance.equipItems[selectSlot] = null;
                ItemManager.instance.equipCheck[selectSlot] = false;

                for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                itemSpace[selectSlot].sprite = noneImage.sprite;

                Color color = itemSpace[selectSlot].GetComponent<Image>().color;
                color.a = 0f;
                itemSpace[selectSlot].GetComponent<Image>().color = color;
            }
        }
        else
        {
            if (ItemManager.instance.equipCheck[selectSlot] == true)
            {
                list.Add(ItemManager.instance.equipItems[selectSlot]);
                ItemManager.instance.equipItems[selectSlot] = null;
                ItemManager.instance.equipItems[selectSlot] = list[selectInventory];
                list.RemoveAt(selectInventory);

                for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                itemSpace[selectSlot].sprite = itemImage[ItemManager.instance.equipItems[selectSlot].itemImage].sprite;
            }
            else
            {

                ItemManager.instance.equipItems[selectSlot] = list[selectInventory];
                list.RemoveAt(selectInventory);
                ItemManager.instance.equipCheck[selectSlot] = true;

                for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                Color color = itemSpace[selectSlot].GetComponent<Image>().color;
                color.a = 1f;
                itemSpace[selectSlot].GetComponent<Image>().color = color;
                itemSpace[selectSlot].sprite = itemImage[ItemManager.instance.equipItems[selectSlot].itemImage].sprite;
            }
        }

        inventoryColor[selectInventory].SetActive(false);
        noneSlot[selectSlot].SetActive(false);
        actSlot[selectSlot].SetActive(true);
        selectInventory = 0;
        lookAtInventorySlot = false;

        CleanSlot();
        SelectItems();
    }

    void Update()
    {
        if (ItemManager.instance.inventoryCheckTime == true) { return; }

        if (Input.GetKeyDown(KeyCode.S) && ItemManager.instance.lookAtInventory == true)
        {
            if (lookAtInventorySlot == false)
            {
                for (int i = 0; i < ItemManager.instance.activeItems.Count; i++) { inventorySlot[i].text = string.Format(" "); }

                for (int i = 0; i < ItemManager.instance.durationItems.Count; i++) { inventorySlot[i].text = string.Format(" "); }

                Time.timeScale = 1f;
                actSlot[selectSlot].SetActive(false);
                noneSlot[selectSlot].SetActive(true);
                StartCoroutine(InventoryClosed());
                ItemManager.instance.inventoryUi.SetActive(false);
                ItemManager.instance.GetComponent<Inventory>().enabled = false;
            }
            else
            {
                lookAtInventorySlot = false;
                inventoryColor[selectInventory].SetActive(false);
                noneSlot[selectSlot].SetActive(false);
                actSlot[selectSlot].SetActive(true);
                selectInventory = 0;

                for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && lookAtInventorySlot == false)
        {
            actSlot[selectSlot].SetActive(false);
            noneSlot[selectSlot].SetActive(true);

            CleanSlot();

            if (selectSlot == 0) { selectSlot = 4; }
            else { selectSlot -= 1; }

            noneSlot[selectSlot].SetActive(false);
            actSlot[selectSlot].SetActive(true);

            SelectItems();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && lookAtInventorySlot == false)
        {
            actSlot[selectSlot].SetActive(false);
            noneSlot[selectSlot].SetActive(true);

            CleanSlot();

            if (selectSlot == 4) { selectSlot = 0; }
            else { selectSlot += 1; }

            noneSlot[selectSlot].SetActive(false);
            actSlot[selectSlot].SetActive(true);

            SelectItems();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (lookAtInventorySlot == false)
            {
                actSlot[selectSlot].SetActive(false);
                noneSlot[selectSlot].SetActive(true);

                CleanSlot();

                if (selectSlot == 2) { selectSlot = 4; }
                else if (selectSlot <= 1) { selectSlot += 3; }
                else if (selectSlot >= 3) { selectSlot -= 3; }

                noneSlot[selectSlot].SetActive(false);
                actSlot[selectSlot].SetActive(true);

                SelectItems();
            }
            else
            {
                if (selectInventory == 0)
                {
                    inventoryColor[selectInventory].SetActive(false);

                    if (itemTypeCheck == true) { selectInventory = ItemManager.instance.activeItems.Count - 1; }
                    else { selectInventory = ItemManager.instance.durationItems.Count - 1; }
                    
                    inventoryColor[selectInventory].SetActive(true);

                    for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                    if (selectInventory != 0)
                    {
                        if (itemTypeCheck == true)
                        {
                            for (int i = 0; i < ItemManager.instance.activeItems[selectInventory].explanationX; i++)
                            {
                                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].itemName); }
                                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].effect); }
                                else { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].explanation[i - 2]); }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ItemManager.instance.durationItems[selectInventory].explanationX; i++)
                            {
                                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].itemName); }
                                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].effect); }
                                else { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].explanation[i - 2]); }
                            }
                        }
                    }
                }
                else
                {
                    inventoryColor[selectInventory].SetActive(false);

                    selectInventory -= 1;

                    inventoryColor[selectInventory].SetActive(true);

                    for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                    if (selectInventory != 0)
                    {
                        if (itemTypeCheck == true)
                        {
                            for (int i = 0; i < ItemManager.instance.activeItems[selectInventory].explanationX; i++)
                            {
                                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].itemName); }
                                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].effect); }
                                else { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].explanation[i - 2]); }
                            }
                        }
                        else
                        {
                            for (int i = 0; i < ItemManager.instance.durationItems[selectInventory].explanationX; i++)
                            {
                                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].itemName); }
                                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].effect); }
                                else { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].explanation[i - 2]); }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (lookAtInventorySlot == false)
            {
                actSlot[selectSlot].SetActive(false);
                noneSlot[selectSlot].SetActive(true);

                CleanSlot();

                if (selectSlot == 2) { selectSlot = 4; }
                else if (selectSlot <= 1) { selectSlot += 3; }
                else if (selectSlot >= 3) { selectSlot -= 3; }

                noneSlot[selectSlot].SetActive(false);
                actSlot[selectSlot].SetActive(true);

                SelectItems();
            }
            else
            {
                if (itemTypeCheck == true)
                {
                    if (selectInventory == ItemManager.instance.activeItems.Count - 1)
                    {
                        inventoryColor[selectInventory].SetActive(false);

                        selectInventory = 0;

                        inventoryColor[selectInventory].SetActive(true);

                        for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }
                    }
                    else
                    {
                        inventoryColor[selectInventory].SetActive(false);

                        selectInventory += 1;

                        inventoryColor[selectInventory].SetActive(true);

                        for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                        if (selectInventory != 0)
                        {
                            for (int i = 0; i < ItemManager.instance.activeItems[selectInventory].explanationX; i++)
                            {
                                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].itemName); }
                                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].effect); }
                                else { itemString[i].text = string.Format(ItemManager.instance.activeItems[selectInventory].explanation[i - 2]); }
                            }
                        }
                    }
                }
                else
                {
                    if (selectInventory == ItemManager.instance.durationItems.Count - 1)
                    {
                        inventoryColor[selectInventory].SetActive(false);

                        selectInventory = 0;

                        inventoryColor[selectInventory].SetActive(true);

                        for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }
                    }
                    else
                    {
                        inventoryColor[selectInventory].SetActive(false);

                        selectInventory += 1;

                        inventoryColor[selectInventory].SetActive(true);

                        for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }

                        if (selectInventory != 0)
                        {
                            for (int i = 0; i < ItemManager.instance.durationItems[selectInventory].explanationX; i++)
                            {
                                if (i == 0) { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].itemName); }
                                else if (i == 1) { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].effect); }
                                else { itemString[i].text = string.Format(ItemManager.instance.durationItems[selectInventory].explanation[i - 2]); }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            if (lookAtInventorySlot == false)
            {
                lookAtInventorySlot = true;
                actSlot[selectSlot].SetActive(false);
                noneSlot[selectSlot].SetActive(true);

                if (selectSlot <= 2) { itemTypeCheck = true; }
                else { itemTypeCheck = false; }

                inventoryColor[selectInventory].SetActive(true);

                for (int i = 0; i < 5; i++) { itemString[i].text = string.Format(" "); }
            }
            else { EquipItem(); }
        }
    }

    IEnumerator InventoryClosed()
    {
        yield return new WaitForSeconds(0.2f);

        ItemManager.instance.lookAtInventory = false;
    }

    void OnEnable()
    {
        selectSlot = 0;
        selectInventory = 0;
        noneSlot[selectSlot].SetActive(false);
        actSlot[selectSlot].SetActive(true);
        lookAtInventorySlot = false;

        SelectItems();
    }

    public string[] SaveEquipItem(string[] saveEquipItems)
    {
        for (int i = 0; i < 5; i++) { saveEquipItems[i] = ItemManager.instance.equipItems[i].itemName; }

        return saveEquipItems;
    }
}
