using System.Collections;
using UnityEngine;

public class SantaChasing : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}

    Transform transform;
    Animator animator;
    EnemyAI ai;
    Transform target;

    bool isMovingDirectlyToPlayer; //true if santa can see the player
                                   //and has a path to DIRECTLY THE PLAYER

    Vector2 lastPlayerPosOrigin;
    float lastPlayerRadius = 0;

    //SETTINGS
    float returnToPatrolDelay = 4f; //When santa loses the player
                                    //how long until begins to patrol again

    override public void OnStateEnter
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("WE CHASING NOW");
        animator.ResetTrigger("lostPlayer");

        ai = animator.GetComponent<EnemyAI>();
        target = ai.player.transform;
        transform = animator.transform;
        this.animator = animator;
        isMovingDirectlyToPlayer = true;
    }
    override public void OnStateUpdate
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (isMovingDirectlyToPlayer)
        {
            if (ai.canSeePlayer)
            {
                ai.agent.SetDestination(target.position);
                ai.PointTowardsCartesian(target.position - transform.position);
                UpdatePlayerPos();
            }
            else
            {
                ai.PointTowardsCartesian(ai.agent.velocity - transform.position);
                IncreasePossiblePlayerPos();
            }

            //If santa lost the direct sight of the player
            if (!ai.IsAgentMoving())
                isMovingDirectlyToPlayer = false;
        }
        else //Santa cant see player and is infering where they are
        {
            IncreasePossiblePlayerPos();
            Debug.Log("THE THINKER");

            //If santa finds the player again after infering
            if (ai.canSeePlayer)
            {
                isMovingDirectlyToPlayer = true;
            }
        }
    }
    void EvaluateSound()
    {
        //WORKING ON THIS RIGHT NOW
        if (ai.soundStack.Count == 0)
            return;
        SoundSignal signal = ai.soundStack.Pop();
        if (Time.time > signal.timeStamp + signal.type.lifeTime)
        {
            EvaluateSound();
            return;
        }
        ai.agent.SetDestination(target.position);
        UpdatePlayerPos();
    }
    void IncreasePossiblePlayerPos()
    {
        lastPlayerRadius += ai.player.movement.moveSpeed * Time.deltaTime;
        Debug.DrawRay(lastPlayerPosOrigin, Vector2.right * lastPlayerRadius);
        if (lastPlayerRadius >= returnToPatrolDelay * ai.player.movement.moveSpeed)
            animator.SetTrigger("lostPlayer");
    }
    void UpdatePlayerPos()
    {
        lastPlayerPosOrigin = ai.agent.destination;
        lastPlayerRadius = 0;
    }
}
