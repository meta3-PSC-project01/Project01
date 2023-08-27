using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossIdle : StateMachineBehaviour
{
    float LIMIT_LEFT = 6;
    float LIMIT_RIGHT = 14;

    BossBase boss;

    int count = 0;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        boss = animator.GetComponent<BossBase>();
        count = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        count += 1;
        if (count >= 50)
        {
            boss.state = (BossState)(Random.Range(0, 15)*.1f);

            if (boss.state == BossState.MOVE)
            {
                Debug.Log(boss.transform.position.x);
                int random = Random.Range(0, 1);
                if (random == 0 && boss.transform.position.x > LIMIT_LEFT && boss.transform.position.x < LIMIT_RIGHT)
                {
                    animator.SetTrigger("MoveLeft");
                    count = 0;
                }
                else if (random == 1 && boss.transform.position.x > LIMIT_LEFT && boss.transform.position.x < LIMIT_RIGHT)
                {
                    animator.SetTrigger("MoveRight");
                    count = 0;
                }
                else if (boss.transform.position.x >= LIMIT_RIGHT)
                {
                    animator.SetTrigger("MoveLeft");
                    count = 0;
                }
                else if (boss.transform.position.x <= LIMIT_LEFT)
                {
                    animator.SetTrigger("MoveRight");
                    count = 0;
                }
            }
            else
            {
                int random = Random.Range(0, 4);

                if (random == 0)
                {
                    animator.SetTrigger("Claw");
                    count = 0;
                }
                else
                if (random == 1)
                {
                    animator.SetTrigger("Tail");
                    count = 0;
                }
                else
                if (random == 2)
                {
                    animator.SetTrigger("Vomit");
                    count = 0;
                }
                else
                if (random == 3)
                {
                    animator.SetTrigger("Bomb");
                    count = 0;
                }
            }
        }


    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

}
