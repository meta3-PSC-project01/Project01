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
    int fieldSelect = -1;
    int escapeSelect = -1;

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
            escapeSelect = EditorGUILayout.Popup("선택 영역", escapeSelect, tmp);
        }

        if (EditorGUI.EndChangeCheck())
        {
            if (mapObject != null && mapObject.transform.childCount > 0)
            {
                fieldList = new List<FieldData>();
                listName = new string[mapObject.transform.childCount-1];
                // StageType에 맞는 Sprite이미지로 교체해준다.

                for (int i = 1; i < mapObject.transform.childCount; i++)
                {
                    fieldList.Add(mapObject.transform.GetChild(i).GetComponent<FieldData>());
                    listName[i-1] = mapObject.transform.GetChild(i).name;
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

                EscapeTile tile = null;
                if (t != null)
                {
                    tile = t.GetComponent<EscapeTile>();
                }
                escapeList.Insert(0, tile);
            }

            if (escapeSelect != -1)
            {
                currEscapeArea.nextTile = escapeList[escapeSelect];
            }
        }


        if (GUI.changed)
        {
            serializedObject.ApplyModifiedProperties();
            EditorUtility.SetDirty(currEscapeArea);
        }

    }
#endif
}
