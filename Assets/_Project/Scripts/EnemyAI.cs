using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Components
    [HideInInspector] public MonoBehaviour monoBehaviour;
    [HideInInspector] public NavMeshAgent agent;
    Animator animator;

    public PlayerManager player;

    [Header("Sight Settings")]
    [SerializeField] int rays;
    [SerializeField] float rayOffset;
    [SerializeField] float distance;
    [SerializeField] float angle;
    [SerializeField] bool debugShowSight;

    [HideInInspector] public Stack<SoundSignal> soundStack;
    [HideInInspector] public bool canSeePlayer = false;

    float rotationAngle;

    void Awake()
    {
        monoBehaviour = this;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        animator = GetComponent<Animator>();
    }
    private void Start()
    {
        player = PlayerManager.Instance;
    }
    private void Update()
    {
        canSeePlayer = CanSeeLayer(LayerMask.NameToLayer("Player"));
        animator.SetBool("canSeePlayer", canSeePlayer);
    }
    void HearSound(SoundSignal sound)
    {
        soundQueue.Push(sound);
    }
    bool CanSeeLayer(LayerMask layer)
    {
        bool canSee = false;
        float angleStep = angle / (rays - 1);
        float startAngle = (-angle / 2f) + rotationAngle;

        for (int i = 0; i < rays; i++)
        {
            float currentAngle = startAngle + angleStep * i + transform.eulerAngles.z;

            Vector2 direction =
                Quaternion.Euler(0f, 0f, currentAngle) * Vector2.right;

            Vector2 rayOrigin = (Vector2)transform.position + direction * rayOffset;

            RaycastHit2D hit = Physics2D.Raycast(
                rayOrigin,
                direction,
                distance
            );

            if (hit.collider != null)
            {
                if (hit.transform.gameObject.layer == layer)
                {
                    canSee = true;
                    if (debugShowSight)
                        Debug.DrawRay(rayOrigin, direction * hit.distance, Color.red);
                }
                else if (debugShowSight)
                {
                    Debug.DrawRay(rayOrigin, direction * hit.distance, Color.green);
                }
            }
            else if (debugShowSight)
            {
                Debug.DrawRay(rayOrigin, direction * distance, Color.green);
            }
        }

        return canSee;
    }
    /// <summary>
    /// Turns santa's sight & direction in the vector given
    /// </summary>
    public void PointTowardsCartesian(Vector2 direction)
    {
        direction.Normalize();

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            direction = direction.x > 0 ? Vector2.right : Vector2.left;
        else
            direction = direction.y > 0 ? Vector2.up : Vector2.down;

        rotationAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        //UPDATE ANIMATION VARS HERE
    }
    public bool IsAgentMoving()
    {
        // First, ensure the agent component exists and is active
        if (agent == null || !agent.enabled || !agent.isOnNavMesh)
        {
            return false;
        }

        // Wait until the path is calculated
        if (agent.pathPending)
        {
            return true; // The path is being calculated, so movement is intended
        }

        // Check if the agent has a path and is near the destination within stopping distance
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            // The agent is close to the destination.
            // Check if it actually has no path left OR its velocity is zero (stuck or arrived)
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // Ensure remainingDistance isn't infinity (which can happen if the path is invalid)
                if (agent.remainingDistance != Mathf.Infinity)
                {
                    return false; // Agent has arrived or is stuck
                }
            }
        }

        // If none of the above conditions are met, the agent is actively moving
        return true;
    }
}
public struct SoundSignal
{
    public SoundType type;
    public Vector2 originalPos;
    public float timeStamp;
}
public struct SoundType
{
    public SoundName name;
    public float travelDistance;
    public float blurRadius; //The range where enemies will check where this sound occured
    public float lifeTime; //The time until this sound is declared stale and discarded

    public SoundType(SoundName name, float travelDistance, float blurRadius, float lifeTime)
    {
        this.name = name;
        this.travelDistance = travelDistance;
        this.blurRadius = blurRadius;
        this.lifeTime = lifeTime;
    }

    public enum SoundName
    {
        Footstep,
        Task
        //ADD MORE SOUNDS HERE
    }
}
public static class SoundTypes
{
    public static readonly SoundType Footstep =
        new SoundType(SoundType.SoundName.Footstep, 5f, 1f, 1.5f);

    public static readonly SoundType Task =
        new SoundType(SoundType.SoundName.Task, 12f, 2.5f, 5f);
    //ADD MORE SOUNDS HERE
}
