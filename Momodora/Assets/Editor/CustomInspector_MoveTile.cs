using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;


//에너미 방향에 따른 각 수치들의 연동 위해서 커스텀 인스펙터창 제작
[CustomEditor(typeof(MoveTile), true)]
public class CustomInspector_MoveTile : Editor
{
    MoveTile moveTile;

    void OnEnable()
    {
        moveTile = target as MoveTile;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        int size = 1;
        switch (moveTile.externDirection)
        {
            case DirectionAxis.RIGHT :

                foreach (var child in moveTile.GetList())
                {
                    child.transform.localPosition = new Vector2(size, 0);
                    size += 1;
                }
                break;
            case DirectionAxis.LEFT:

                foreach (var child in moveTile.GetList())
                {
                    child.transform.localPosition = new Vector2(-size, 0);
                    size += 1;
                }
                break;
            case DirectionAxis.UP:

                foreach (var child in moveTile.GetList())
                {
                    child.transform.localPosition = new Vector2(0, size);
                    size += 1;
                }
                break;
            case DirectionAxis.DOWN:

                foreach (var child in moveTile.GetList())
                {
                    child.transform.localPosition = new Vector2(0, -size);
                    size += 1;
                }
                break;
            default:
                break;
        }

        if (GUILayout.Button("Create"))
        {
            GameObject child = Instantiate(moveTile.body, moveTile.transform.position, Quaternion.identity, moveTile.transform);            
            moveTile.GetList().Add(child);
            moveTile.childCount +=1;
            EditorUtility.SetDirty(GameObject.FindObjectOfType<Transform>());
        }

        if (GUILayout.Button("Remove"))
        {
            moveTile.RemoveLastIndex();
            EditorUtility.SetDirty(GameObject.FindObjectOfType<Transform>());
        }
    }
}