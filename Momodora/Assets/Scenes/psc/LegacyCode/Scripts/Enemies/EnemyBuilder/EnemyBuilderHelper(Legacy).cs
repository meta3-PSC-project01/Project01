using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilderHelper_Legacy
{
    public void Concreate(IEnemyBuild_Legacy enemyBuild)
    {
        enemyBuild.BuildComponents();
        enemyBuild.BuildAttackType();
        enemyBuild.BuildChasingAi();
    }
}
