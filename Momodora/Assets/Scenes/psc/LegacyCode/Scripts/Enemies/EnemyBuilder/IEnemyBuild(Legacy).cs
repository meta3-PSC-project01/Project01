using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBuild_Legacy
{
    void BuildAttackType();
    void BuildComponents();
    void BuildChasingAi();

    EnemyCommon_Legacy GetEnemy();
}
