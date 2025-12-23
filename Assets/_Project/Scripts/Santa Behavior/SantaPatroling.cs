using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SantaPatroling : StateMachineBehaviour
{
    Transform transform;
    EnemyAI ai;

    Vector3 target;
    private bool headToBells = false;
    private Vector2 _bellPoint;

    private void OnEnable()
    {
        ElfDetectionController.ElfDetectedEvent += HandleBellEvent;
    }
    
    private void OnDisable()
    {
        ElfDetectionController.ElfDetectedEvent -= HandleBellEvent;
    }
    
    void HandleBellEvent(bool currentCondition, Vector2 bellPoint)
    {
        headToBells = currentCondition;
        _bellPoint = bellPoint;
    }

    override public void OnStateEnter
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai = animator.GetComponent<EnemyAI>();
        ai.agent.speed = ai.walkSpeed;
        Debug.Log("Patrolling");

        transform = animator.transform;
    }
    override public void OnStateUpdate
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.PointTowardsCartesian(ai.agent.velocity);

        if (headToBells)
        {
            ai.agent.SetDestination(_bellPoint);
        }
        if (!ai.IsAgentMoving())
            DecideNewPoint();
    }
    void DecideNewPoint()
    {
        if (Random.Range(0f, 1f) < ai.chanceToPatrolTask)
            FindRandomTaskPoint();
        else
            FindRandomWalkablePoint();
    }
    void FindRandomTaskPoint()
    {
        int attempts = 0;
        int maxAttempts = ai.taskPatrolPoints.Length;
        Transform selectedTransform = null;

        while (selectedTransform == null && attempts < maxAttempts)
        {
            int index = Random.Range(0, ai.taskPatrolPoints.Length);
            selectedTransform = ai.taskPatrolPoints[index];
            attempts++;
        }

        // If no valid transform found, fall back to random walkable point
        if (selectedTransform == null)
        {
            Debug.LogWarning("All task patrol points have been destroyed, using random walkable point");
            FindRandomWalkablePoint();
            return;
        }

        Vector2 point = selectedTransform.position;
        target = point;
        ai.agent.SetDestination(target);
    }
    void FindRandomWalkablePoint()
    {
        Vector2 result = Vector2.zero;
        bool pointValid = false;
        Vector2 max = new Vector2(30, 30f); // original 11 6.5
        Vector2 min = new Vector2(-10, -5.5f);

        int attempts = 0;
        int maxAttempts = 50;
        do
        {
            attempts++;
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            Vector2 posToTest = new Vector2(x, y);

            if (ai.IsPathValid(posToTest))
                continue;

            result = posToTest;
            pointValid = true;
            
        } while (!pointValid && attempts < maxAttempts);

        if (!pointValid)
            return;

        target = result;
        ai.agent.SetDestination(target);
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