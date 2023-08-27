using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeadControl : MonoBehaviour
{
    private Animator animator;
    public BossHeadState currState;


    private void Awake()
    {
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
}


public enum BossHeadState
{
    IDLE=0,
    CLAW,
    BOMB,
    VOMIT,
    DEAD

}