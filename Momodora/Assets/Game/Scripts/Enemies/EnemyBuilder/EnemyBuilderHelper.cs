using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBuilderHelper
{
    public void Concreate(IEnemyBuild enemyBuild)
    {
        enemyBuild.BuildComponents();
        enemyBuild.BuildAttackType();
        enemyBuild.BuildChasingAi();
    }
}
