using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBuild
{
    void BuildAttackType();
    void BuildComponents();
    void BuildChasingAi();

    EnemyCommon GetEnemy();
}
