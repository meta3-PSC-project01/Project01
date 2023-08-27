using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBase : MonoBehaviour
{
    public BossState state;

    public bool isDie = false;

    Rigidbody2D bossRigidbody;
    PlayerMove player;
    public Transform body;
    Animator animator;

    public GameObject _bomb;
    public GameObject _vomit;
    public GameObject _ClawEffect;

    GameObject bombCopy;
    GameObject vomitCopy;
    GameObject ClawEffectCopy;

    public GameObject intro1;
    public GameObject intro2;
    public GameObject battle;

    private void Awake()
    {
        intro1.SetActive(true);
        intro2.SetActive(false);
        battle.SetActive(false);

    }

    void Start()
    {
        animator = GetComponent<Animator>();
        player = FindObjectOfType<PlayerMove>();
        bossRigidbody = GetComponent<Rigidbody2D>();
    }

    public void AttackClaw() 
    {
        Vector2 position = body.position + Vector3.up * .75f + Vector3.right * -1f;
        ClawEffectCopy = Instantiate(_ClawEffect, position, Quaternion.identity);
    }

    public void AttackBomb()
    {
        Vector2 position = body.position + Vector3.up * .6f + Vector3.right * -.7f;
        bombCopy = Instantiate(_bomb, position, Quaternion.identity);
    }

    public void AttackVomit()
    {
        Vector2 position = body.position + Vector3.up * 0f + Vector3.right * -.5f;
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

    public void EndIntro()
    {
        intro1.SetActive(false); 
        intro2.SetActive(false);
        battle.SetActive(true);

        animator.SetBool("Idle", true);
    }
}

public enum BossState
{
    ATTACK,
    MOVE,
    DEAD
}
