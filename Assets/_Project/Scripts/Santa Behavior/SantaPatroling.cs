using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class SantaPatroling : StateMachineBehaviour
{
    Animator animator;
    Transform transform;
    SantaAI ai;

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
            ai.target = signal.originalPos + Random.insideUnitCircle * signal.type.blurRadius;
            animator.SetBool("inspecting", true);

            // Clear footsteps after reaction
            ai.ClearSoundsOfType(signal.type.name);
            return true;
        }
        return false;
    }
    override public void OnStateEnter
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        this.animator = animator;
        ai = animator.GetComponent<SantaAI>();
        ai.agent.speed = ai.walkSpeed;
        Debug.Log("Patrolling");

        transform = animator.transform;
    }
    override public void OnStateUpdate
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.PointTowardsCartesian(ai.agent.velocity);

        if (ai.soundStack.Count != 0)
            EvaluateSound();

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
        ai.target = point;
        ai.agent.SetDestination(ai.target);
    }
    void FindRandomWalkablePoint()
    {
        Vector2 result = Vector2.zero;
        bool pointValid = false;
        Vector2 max = ai.maxWorldBounds;
        Vector2 min = ai.minWorldBounds;

        int attempts = 0;
        int maxAttempts = 50;
        do
        {
            attempts++;
            float x = Random.Range(min.x, max.x);
            float y = Random.Range(min.y, max.y);
            Vector2 posToTest = new Vector2(x, y);

            if (!ai.IsPathValid(posToTest))
                continue;

            result = posToTest;
            pointValid = true;
            
        } while (!pointValid && attempts < maxAttempts);

        if (!pointValid)
            return;

        ai.target = result;
        ai.agent.SetDestination(ai.target);
    }
    
}