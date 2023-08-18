using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif


[CustomEditor(typeof(ControlBase), true)]
public class CustomInspector_ControlBase : Editor
{
    ControlBase control;
    Object obj;

    void OnEnable()
    {
        control = target as ControlBase;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

#if UNITY_EDITOR



#endif
    }
}
