using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

//

//에너미 방향에 따른 각 수치들의 연동 위해서 커스텀 인스펙터창 제작
[CustomEditor(typeof(EnemyBase), true)]
public class CustomInspector_EnemyBase : Editor
{
    EnemyBase enemyObject;

    void OnEnable()
    {
        enemyObject = target as EnemyBase;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (enemyObject.direction == DirectionHorizen.LEFT)
        {
            enemyObject.turn();
        }
        else if (enemyObject.direction == DirectionHorizen.RIGHT)
        {
            enemyObject.turn();
        }
    }
}
