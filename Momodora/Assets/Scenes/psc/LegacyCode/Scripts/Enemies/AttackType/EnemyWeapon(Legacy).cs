using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeapon_Legacy : MonoBehaviour
{
    //공격 타입
    public WeaponType_Legacy weaponType;    
    //공격력
    public int weaponDamage;
    //공속
    public float weaponSpeed;

    public virtual void useWeapon(EnemyDirection_Legacy direction) { }
}

public enum WeaponType_Legacy
{
    RANGE,  //원거리 공격
    MELEE,  //근거리 공격(공격 애니메이션 있음)
    NOTHING //근거리 공격(공격 애니메이션 없음)
} 