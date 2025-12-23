using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class SantaInspecting : StateMachineBehaviour
{
    UnityEngine.Transform transform;
    SantaAI ai;

    override public void OnStateEnter
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        transform = animator.transform;
        ai = animator.GetComponent<SantaAI>();
        ai.agent.speed = ai.inspectSpeed;
        Debug.Log("Inspecting");

        ai.agent.SetDestination(ai.target);
    }
    override public void OnStateUpdate
        (Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        ai.PointTowardsCartesian(ai.agent.velocity);

        if (!ai.IsAgentMoving() || 
            Vector2.Distance(transform.position, ai.agent.pathEndPosition) < .75f)
            animator.SetBool("inspecting", false);
    }
}
