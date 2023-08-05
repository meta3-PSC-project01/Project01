using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyPool_Legacy : MonoBehaviour
{
    public static EnemyPool_Legacy instance = null;

    public List<EnemyCommon_Legacy> enemyDataList;
    private EnemyBuilderHelper_Legacy builderHelper;

    Queue<GameObject> poolingObjects;

    public void Awake()
    {
        if (instance == null)
        {
            instance = this;
            Init(10);
        }
        else
        {
            Destroy(this);
        }
    }

    private void Init(int initCount)
    {
        //DirectoryInfo di = new DirectoryInfo(Application.dataPath+"/Game/Prefabs/Enemies");


        builderHelper = new EnemyBuilderHelper_Legacy();
        //enemyDataList = new List<EnemyCommon>();

        poolingObjects = new Queue<GameObject>();
        /* foreach (var tmp in enemyDataList)
         {
             enemyDataList.Add(((GameObject)tmp).GetComponent<EnemyCommon>());
         }*/

        for (int i = 0; i < initCount; i++)
        {
            poolingObjects.Enqueue(CreateObject());
        }
    }

    private GameObject CreateObject()
    {
        GameObject newObject = new GameObject();

        newObject.name = "EmptyObject";
        newObject.gameObject.SetActive(false);
        newObject.transform.SetParent(transform);

        return newObject;
    }

    public string[] GetEnemiesName()
    {
        string[] arr = new string[enemyDataList.Count];
        for (int i = 0; i < enemyDataList.Count; i++)
        {
            arr[i] = enemyDataList[i].name;
        }

        return arr;
    }

    public GameObject GetEnemy(string enemyName)
    {
        Debug.Assert(poolingObjects!=null, "풀링큐생성안댐?");

        EnemyCommon_Legacy tmp = enemyDataList.Find(x => x.name.Equals(enemyName));

        if (poolingObjects.Count != 0)
        {
            GameObject outObject = poolingObjects.Dequeue();
            outObject.transform.SetParent(null);
            outObject.SetActive(true);

            return outObject;
        }
        else
        {
            GameObject outObject = new GameObject();

            builderHelper.Concreate(new EnemyBaseBuilder_Legacy(outObject, tmp));
            outObject.transform.SetParent(null);
            outObject.SetActive(true);

            return outObject;
        }
    }

    public void ReturnEnemy(GameObject returnObject)
    {
        Debug.Assert(poolingObjects != null, "풀링큐생성안댐?");
        EnemyCommon_Legacy tmp = returnObject.GetComponent<EnemyCommon_Legacy>();
        if (tmp != null)
        {
            tmp.Remove();
            returnObject.gameObject.SetActive(false);
            returnObject.transform.SetParent(transform);
            returnObject.name = "EmptyObject";
            poolingObjects.Enqueue(returnObject);
        }
    }

}
