using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackData : MonoBehaviour
{
    //타입
    public EnemyAttackType type;
    
    //총알, 이펙트 들어감
    public GameObject prefab;

    //공격력
    public int damage;
}

public enum EnemyAttackType
{
    RANGE,
    MELEE,
    NOTHING
}
