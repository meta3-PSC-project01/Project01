using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBaseBuilder_Legacy : IEnemyBuild_Legacy
{
    EnemyCommon_Legacy target;
    EnemyCommon_Legacy buildEnemy;

    public EnemyBaseBuilder_Legacy(GameObject _gameObject, EnemyCommon_Legacy _target)
    {
        target = _target;
        buildEnemy = _gameObject.AddComponent<EnemyCommon_Legacy>();
    }

    public void BuildComponents()
    {
        buildEnemy.tag = "Enemy";
        buildEnemy.name = target.name;

        buildEnemy.enemyCollider = buildEnemy.AddComponent<BoxCollider2D>();
        buildEnemy.enemyCollider.offset = target.enemyCollider.offset;
        buildEnemy.enemyCollider.size = target.enemyCollider.size;

        buildEnemy.enemyRigidbody = buildEnemy.AddComponent<Rigidbody2D>();
        buildEnemy.enemyRigidbody.constraints = target.enemyRigidbody.constraints;


        buildEnemy.enemyRenderer = buildEnemy.AddComponent<SpriteRenderer>();
        buildEnemy.enemyRenderer.sprite = target.enemyRenderer.sprite;

        buildEnemy.enemyAnimator = buildEnemy.AddComponent<Animator>();
        buildEnemy.enemyAnimator.runtimeAnimatorController = target.enemyAnimator.runtimeAnimatorController;


    }

    public void BuildAttackType()
    {
        buildEnemy.attackObject = target.attackObject;
    }

    public void BuildChasingAi()
    {
    }
    

    public EnemyCommon_Legacy GetEnemy()
    {
        return buildEnemy;
    }


}
