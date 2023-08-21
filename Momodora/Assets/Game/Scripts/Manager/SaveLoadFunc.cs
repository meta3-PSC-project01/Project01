using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveLoadFunc : MonoBehaviour
{
    public string[] saveEquipItems = new string[5];

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //SaveLoad character = new SaveLoad(1);

            //GameManager.Save(character, "save_001");
        }

        /////////////////////////////////////////////////////////////////////////

        if (Input.GetKeyDown(KeyCode.L))
        {
            SaveLoad loadData = GameManager.Load("save_001");
            //Debug.Log(string.Format("LoadData Result => name : {0}, age : {1}, power : {2}, stage 1 : {3}, stage 2 : {4}", loadData.name, loadData.age, loadData.power, loadData.stage[0], loadData.stage[1]));
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            for (int i = 0; i < 5; i++)
            {
                if (ItemManager.instance.equipCheck[i] == true)
                {
                    Debug.Log(ItemManager.instance.equipItems[i].name);
                }
                else
                {
                    Debug.Log("Null");
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.N))
        {
            for (int i = 0; i < ItemManager.instance.activeItems.Count; i++)
            {
                Debug.Log(ItemManager.instance.activeItems[i].name);
            }

            Debug.Log(ItemManager.instance.activeItems.Count);
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            for (int i = 0; i < ItemManager.instance.durationItems.Count; i++)
            {
                Debug.Log(ItemManager.instance.durationItems[i].name);
            }

            Debug.Log(ItemManager.instance.durationItems.Count);
        }
    }
}
