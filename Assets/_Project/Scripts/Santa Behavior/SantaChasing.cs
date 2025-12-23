using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class SantaChasing : StateMachineBehaviour
{
    Transform transform;
    Animator animator;
    SantaAI ai;

    Transform player;

    bool isMovingDirectlyToPlayer; //true if santa can see the player
                                   //and has a path to DIRECTLY THE PLAYER

    Vector2 lastPlayerPosOrigin;
    float lastPlayerRadius = 0;

    //SETTINGS
    float returnToPatrolDelay = 4f; //When santa loses the player
                                    //how long until begins to patrol again
    public static event Action SantaChasingStarted;
    
    override public void OnStateEnter
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Debug.Log("WE CHASING NOW");
        animator.ResetTrigger("lostPlayer");
        animator.SetBool("inspecting", false);
        
        SantaChasingStarted?.Invoke();
        ai = animator.GetComponent<SantaAI>();
        ai.agent.speed = ai.chaseSpeed;
        ai.target = ai.player.transform.position;
        transform = animator.transform;
        this.animator = animator;
        isMovingDirectlyToPlayer = true;
        player = PlayerManager.Instance.transform;
    }
    override public void OnStateUpdate
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.PointTowardsCartesian(ai.agent.velocity);
        if (isMovingDirectlyToPlayer)
        {
            if (ai.canSeePlayer)
            {
                ai.target = player.position;
                ai.agent.SetDestination(ai.target);
                UpdatePlayerPos();
            }
            else
            {
                IncreasePossiblePlayerPos();
                EvaluateSound();
            }

            //If santa lost the direct sight of the player
            if (!ai.IsAgentMoving())
                isMovingDirectlyToPlayer = false;
        }
        else //Santa cant see player and is infering where they are
        {
            IncreasePossiblePlayerPos();
            if (!EvaluateSound() && !ai.IsAgentMoving())
                GuessPlayerPos();

            //If santa finds the player again after infering
            if (ai.canSeePlayer)
            {
                isMovingDirectlyToPlayer = true;
            }
        }
    }
    void GuessPlayerPos()
    {
        int attempt = 0;
        int maxAttempt = 50;
        do
        {
            attempt++;
            Vector2 posToTest =
                lastPlayerPosOrigin + Random.insideUnitCircle * lastPlayerRadius;
            if (ai.IsPathValid(posToTest))
            {
                ai.agent.SetDestination(posToTest);
                break;
            }
        }
        while (attempt < maxAttempt);
    }
    /// <returns>false if no valid sound in stack</returns>
    bool EvaluateSound()
    {
        if (ai.soundStack.Count == 0)
            return false;
        for (int i = 0; i < ai.soundStack.Count; i++)
        {
            SoundSignal signal = ai.soundStack[i];

            // Remove expired sounds
            if (Time.time > signal.timeStamp + signal.type.lifeTime)
            {
                ai.soundStack.RemoveAt(i);
                i--;
                if (ai.soundStack.Count == 0)
                    return false;
                continue;
            }

            // React to best sound
            ai.agent.SetDestination(
                signal.originalPos +
                Random.insideUnitCircle * signal.type.blurRadius);

            UpdatePlayerPos();

            // Clear type after reaction
            ai.ClearSoundsOfType(signal.type.name);
            return true;
        }
        return false;
    }

    void IncreasePossiblePlayerPos()
    {
        lastPlayerRadius += ai.player._playerMovement.moveSpeed * Time.deltaTime;
        DebugDrawCircle(lastPlayerPosOrigin, lastPlayerRadius);
        if (lastPlayerRadius >= returnToPatrolDelay * ai.player._playerMovement.moveSpeed)
            animator.SetTrigger("lostPlayer");
    }
    void UpdatePlayerPos()
    {
        lastPlayerPosOrigin = ai.agent.destination;
        lastPlayerRadius = 0;
    }

    //-----DEBUG
    void DebugDrawCircle(Vector2 center, float radius)
    {
        int rays = 16;
        float angle = 0f;
        float angleStep = 360f / rays;

        for (int i = 0; i < rays; i++)
        {
            Vector2 dir = Quaternion.Euler(0, 0, angle) * Vector2.right;
            Debug.DrawRay(center, dir * radius);
            angle += angleStep;
        }
    }



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
}
