using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

[CustomEditor(typeof(EscapeTile), true)]
public class CustomInspector_EscapeArea : Editor
{
    EscapeTile currEscapeArea;
    MapData mapObject;

    List<FieldData> fieldList;
    List<EscapeTile> escapeList;
    List<string> direction = null;
    string[] listName;
    int fieldSelect = 0;
    int escapeSelect = 0;

    void OnEnable()
    {
        currEscapeArea = target as EscapeTile;
        fieldList = new List<FieldData>();
        escapeList = new List<EscapeTile>();
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
#if UNITY_EDITOR

        EditorGUI.BeginChangeCheck();
        mapObject = EditorGUILayout.ObjectField("부모 맵", mapObject, typeof(MapData), true) as MapData;

        if (listName != null && listName.Length > 0)
        {
            fieldSelect = EditorGUILayout.Popup("선택 필드", fieldSelect, listName);
        }


        if (direction != null && direction.Count > 0)
        {
            string[] tmp = direction.ToArray();
            EditorGUI.BeginChangeCheck();
            escapeSelect = EditorGUILayout.Popup("선택 영역", escapeSelect, tmp);
        }

        if (EditorGUI.EndChangeCheck())
        {
            if (mapObject != null && mapObject.transform.childCount > 0)
            {
                fieldList = new List<FieldData>();
                listName = new string[mapObject.transform.childCount];
                // StageType에 맞는 Sprite이미지로 교체해준다.

                for (int i = 0; i < mapObject.transform.childCount; i++)
                {
                    fieldList.Add(mapObject.transform.GetChild(i).GetComponent<FieldData>());
                    listName[i] = mapObject.transform.GetChild(i).name;
                }
            }


            direction = new List<string>()
                {
                    "EscapeUp", "EscapeDown", "EscapeLeft", "EscapeRight"
                };

            for (int i = 3; i >= 0; i--)
            {
                Transform t = fieldList[fieldSelect].transform.GetChild(0);
                t = t.Find(direction[i]);

                if (t != null)
                {
                    EscapeTile tile = t.GetComponent<EscapeTile>();
                    escapeList.Insert(0,tile);
                }
                else
                {
                    direction.RemoveAt(i);
                }
            }


            currEscapeArea.nextTile = escapeList[escapeSelect];
            escapeList[escapeSelect].nextTile = currEscapeArea;

            serializedObject.ApplyModifiedProperties();
        }


        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(currEscapeArea);
        }

    }
#endif
}
