using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;

public class TestWindow : EditorWindow
{
    public EnemyCommon_Legacy obj;
    Queue<GameObject> testList = new Queue<GameObject>();

    [MenuItem("CustomMenu/BuildTest")]
    static void BuildTestOpen()
    {
        TestWindow window = (TestWindow)EditorWindow.GetWindow(typeof(TestWindow));
        window.Show();
    }


    void OnGUI()
    {
        if (GUILayout.Button("RandomEnemy"))
        {
            string[] enemiesName = EnemyPool_Legacy.instance.GetEnemiesName();
            GameObject tmp = EnemyPool_Legacy.instance.GetEnemy(enemiesName[Random.Range(0, enemiesName.Length)]);
            tmp.transform.position = Vector3.zero;
            testList.Enqueue(tmp);
        }

        if (GUILayout.Button("RemoveObject"))
        {
            GameObject tmp = testList.Dequeue();
            EnemyPool_Legacy.instance.ReturnEnemy(tmp);
        }


        if (GUILayout.Button("Shoot"))
        {
            EnemyCommon_Legacy tmp = AssetDatabase.LoadAssetAtPath<EnemyCommon_Legacy>("Assets/Game/Prefabs/Enemies/BigTomata.prefab");

            Debug.Log(tmp);

            obj = Instantiate(tmp, new Vector3(0,0,0), Quaternion.identity);
            obj.Attack();
        }
    }
}
