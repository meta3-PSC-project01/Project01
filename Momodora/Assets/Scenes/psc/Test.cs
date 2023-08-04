using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEditor.UI;

public class TestWindow : EditorWindow
{
    Queue<GameObject> testList = new Queue<GameObject>();

    [MenuItem("CustomMenu/BuildTest")]
    static void BuildTestOpen()
    {
        TestWindow window = (TestWindow)EditorWindow.GetWindow(typeof(TestWindow));
        window.Show();
    }


    void OnGUI()
    {
        if (GUILayout.Button("CreateShieldImp"))
        {
            GameObject tmp = EnemyPool.instance.GetEnemy("ShieldImp");
            tmp.transform.position = Vector3.zero;
            testList.Enqueue(tmp);
        }

        if (GUILayout.Button("RemoveObject"))
        {
            GameObject tmp = testList.Dequeue();
            EnemyPool.instance.ReturnEnemy(tmp);
        }
    }
}
