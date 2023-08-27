using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadControl : MonoBehaviour, IHitControl
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    public BossHeadState currState;


    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        currState = BossHeadState.IDLE;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currState) 
        {
            case BossHeadState.IDLE:
                animator.SetBool("Idle", true);

                animator.SetBool("Claw", false);
                animator.SetBool("Bomb", false);
                animator.SetBool("Vomit", false);
                break;

            case BossHeadState.CLAW:
                animator.SetBool("Idle", false);

                animator.SetBool("Claw", true);
                break;

            case BossHeadState.BOMB:
                animator.SetBool("Idle", false);

                animator.SetBool("Bomb", true);
                break;

            case BossHeadState.VOMIT:
                animator.SetBool("Idle", false);

                animator.SetBool("Vomit", true);
                break;

            case BossHeadState.DEAD:
                animator.SetBool("Dead", true);
                
                animator.SetBool("Idle", false);

                animator.SetBool("Claw", false);
                animator.SetBool("Bomb", false);
                animator.SetBool("Vomit", false);
                break;
        }
        
    }

    public int hp;

    public void Hit(int damage, int direction)
    {
        if (IsHitPossible())
        {
            HitReaction(direction);
            hp -= damage;

            if (hp < 0)
            {
                Die();
            }
        }
    }

    Coroutine coroutine = null;
    public void HitReaction(int direction)
    {
        if(coroutine==null)
            coroutine = StartCoroutine(HitReactionRoutine());
    }

    IEnumerator HitReactionRoutine()
    {
        for(int i = 0; i < 4; i++)
        {
            spriteRenderer.color = new Color(255, 0, 0, 200);
            yield return new WaitForSeconds(.05f);
            spriteRenderer.color = new Color(255, 255, 255, 255);
            yield return new WaitForSeconds(.05f);
        }

        coroutine = null;

    }

    public bool IsHitPossible()
    {
        if(hp>0)
            return true;

        else 
            return false;
    }
    public bool Die()
    {
        throw new System.NotImplementedException();
    }
}


public enum BossHeadState
{
    IDLE=0,
    CLAW,
    BOMB,
    VOMIT,
    DEAD

}