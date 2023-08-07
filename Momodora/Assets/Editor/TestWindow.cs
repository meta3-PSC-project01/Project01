using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UI;


//테스트 용도의 확장 에디터 윈도우
//플레이 타임중 테스트가 필요한 것들을 버튼에 연동해서 사용할 예정
public class TestWindow : EditorWindow
{
    /// <summary>
    /// Legacy코드
    /// </summary>
    //public EnemyCommon_Legacy obj;
    //Queue<GameObject> testList = new Queue<GameObject>();
    //===========================================================================================

    //메뉴에 추가
    [MenuItem("CustomMenu/BuildTest")]
    static void OpenTestWindow()
    {
        TestWindow window = (TestWindow)EditorWindow.GetWindow(typeof(TestWindow));
        window.Show();
    }


    void OnGUI()
    {
        ///<summary>
        /// Legacy코드
        ///</summary>
       /* if (GUILayout.Button("RandomEnemy"))
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
        }*/
    }
}
