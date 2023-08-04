using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyBuilder : IEnemyBuild
{
    EnemyCommon target;
    EnemyCommon buildEnemy;


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
    

    public EnemyCommon GetEnemy()
    {
        return buildEnemy;
    }

    public void SetEnemy(GameObject _gameObject, EnemyCommon _target)
    {
        target = _target;
        buildEnemy = _gameObject.AddComponent<EnemyCommon>();
    }

}
