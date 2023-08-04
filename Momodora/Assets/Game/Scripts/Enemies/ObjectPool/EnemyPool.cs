using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public static EnemyPool instance = null;

    public List<EnemyCommon> enemyDataList;
    private EnemyBuilderHelper builderHelper;
    private EnemyBuilder builder;

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


        builder = new EnemyBuilder();
        builderHelper = new EnemyBuilderHelper();
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

    public GameObject GetEnemy(string enemyName)
    {
        Debug.Assert(poolingObjects!=null, "풀링큐생성안댐?");

        EnemyCommon tmp = enemyDataList.Find(x => x.name.Equals(enemyName));

        if (poolingObjects.Count != 0)
        {
            GameObject outObject = poolingObjects.Dequeue();
            builder.SetEnemy(outObject, tmp);
            builderHelper.Concreate(builder);
            outObject.transform.SetParent(null);
            outObject.SetActive(true);

            return outObject;
        }
        else
        {
            GameObject outObject = new GameObject();

            builder.SetEnemy(outObject, tmp);
            builderHelper.Concreate(builder);
            outObject.transform.SetParent(null);
            outObject.SetActive(true);

            return outObject;

        }
    }

    public void ReturnEnemy(GameObject returnObject)
    {
        Debug.Assert(poolingObjects != null, "풀링큐생성안댐?");
        EnemyCommon tmp = returnObject.GetComponent<EnemyCommon>();
        if (tmp != null)
        {
            tmp.Remove();
            returnObject.gameObject.SetActive(false);
            returnObject.transform.SetParent(transform);
            poolingObjects.Enqueue(returnObject);
        }
    }

}
