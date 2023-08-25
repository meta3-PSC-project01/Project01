using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    public BossState state;

    Rigidbody2D bossRigidbody;
    PlayerMove player;
    Transform body;
    Animator animator;

    public GameObject _bomb;
    public GameObject _vomit;
    public GameObject _ClawEffect;

    GameObject bombCopy;
    GameObject vomitCopy;
    GameObject ClawEffectCopy;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMove>();
        bossRigidbody = GetComponent<Rigidbody2D>();
        body = transform.Find("Body");
    }

    public void AttackClaw() 
    {
        Vector2 position = body.position + Vector3.up * .5f + Vector3.right * -.5f;
        ClawEffectCopy = Instantiate(_ClawEffect, position, Quaternion.identity);
    }

    public void AttackBomb()
    {
        Vector2 position = body.position + Vector3.up * .5f + Vector3.right * -.5f;
        bombCopy = Instantiate(_bomb, position, Quaternion.identity);
        BossAttack tmp = bombCopy.GetComponent<BossAttack>();
        Rigidbody2D rb = tmp.GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(-10,-10);
    }

    public void AttackVomit()
    {
        Vector2 position = body.position + Vector3.up * -1f + Vector3.right * -.5f;
        vomitCopy = Instantiate(_vomit, position, Quaternion.identity);
    }

    public void EndClaw()
    {
        Destroy(ClawEffectCopy,1f);
    }

    public void EndBomb()
    {
        Destroy(bombCopy, 1f);
    }

    public void EndVomit()
    {
        Destroy(vomitCopy, 1f);
    }

}

public enum BossState
{
    ATTACK,
    MOVE,
    DEAD
}
