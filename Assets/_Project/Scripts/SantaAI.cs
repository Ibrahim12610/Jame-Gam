using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SantaAI : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private LayerMask visionLayer;
    [SerializeField] private LayerMask playerLayer;
    public Transform[] taskPatrolPoints;
    [HideInInspector] public MonoBehaviour monoBehaviour;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public PlayerManager player;
    Animator animator;

    [Header("Move Settings")]
    public float walkSpeed;
    public float inspectSpeed;
    public float chaseSpeed;
    public Vector2 minWorldBounds;
    public Vector2 maxWorldBounds;
    [Tooltip("Each time santa picks a new patrol " +
        "point this is the probability he will pick a task point")]
    public float chanceToPatrolTask = .1f;
    public float chancePerSecondToObserve = .1f;

    public bool hasKilledPlayer = false;

    [Header("Sight Settings")]
    [SerializeField] int rays;
    [SerializeField] float rayOffset;
    [SerializeField] float distance;
    [SerializeField] float angle;
    [SerializeField] bool debugShowSight;

    [HideInInspector] public bool isChasing;
    [HideInInspector] public Vector2 target;
    public List<SoundSignal> soundStack;
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
        soundStack = new List<SoundSignal>();
        //StartCoroutine(ObserveRoutine());
    }
    private void Update()
    {
        if (hasKilledPlayer)
        {
            canSeePlayer = false;
            animator.SetBool("canSeePlayer", false);
            return;
        }
        
        // canSeePlayer = CanSeeLayer(LayerMask.NameToLayer("PlayerRaycast"));
        canSeePlayer = CanSeeLayer(playerLayer);
        animator.SetBool("canSeePlayer", canSeePlayer);
    }
    //-----Sound stack stuff-----
    public void PushSound(SoundSignal signal)
    {
        soundStack.Add(signal);

        // Sort: highest priority first, newest first
        soundStack.Sort((a, b) =>
        {
            int p = b.type.priority.CompareTo(a.type.priority);
            if (p != 0) return p;
            return b.timeStamp.CompareTo(a.timeStamp);
        });
    }
    public void ClearSoundsOfType(SoundType.SoundName name)
    {
        soundStack.RemoveAll(s => s.type.name == name);
    }
    // end of sound stack stuff
    IEnumerator ObserveRoutine()
    {
        //WORK IN PROGRESS
        while (true)
        {
            yield return new WaitForSeconds(1);
            if (animator.GetCurrentAnimatorStateInfo(1).IsTag("Patrol"))
                Debug.Log("ASDD");
        }
    }
    public void HearSound(SoundSignal sound)
    {
        if (Vector2.Distance(transform.position, sound.originalPos) <= sound.type.travelDistance)
            PushSound(sound);
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
                distance,
                visionLayer
            );

            if (hit.collider != null)
            {
                if (((1 << hit.transform.gameObject.layer) & layer) != 0)
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
        if (direction == Vector2.zero)
            return;
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
    public bool IsPathValid(Vector2 targetPos)
    {
        if (!IsPositionWalkable(targetPos))
            return false;

        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPos, path);
        if (path.status != NavMeshPathStatus.PathComplete)
            return false;
        return true;
    }
    bool IsPositionWalkable(Vector2 position)
    {
        int notWalkableArea = NavMesh.GetAreaFromName("Not Walkable");

        // Build a mask that excludes Not Walkable
        int walkableMask = NavMesh.AllAreas;
        if (notWalkableArea != -1)
            walkableMask &= ~(1 << notWalkableArea);

        // Only succeeds if position is ON the NavMesh AND in an allowed area
        return NavMesh.SamplePosition(position, out _, 0.75f, walkableMask);
    }
}
public class SoundSignal
{
    public SoundType type;
    public Vector2 originalPos;
    public float timeStamp;

    public SoundSignal(SoundType type, Vector2 originalPos, float timeStamp)
    {
        this.type = type;
        this.originalPos = originalPos;
        this.timeStamp = timeStamp;
    }
}
public struct SoundType
{
    public SoundName name;
    public float travelDistance;
    public float blurRadius; //The range where enemies will check where this sound occured
    public float lifeTime; //The time until this sound is declared stale and discarded
    public int priority;

    public SoundType
        (SoundName name, float travelDistance, float blurRadius, float lifeTime, int priority)
    {
        this.name = name;
        this.travelDistance = travelDistance;
        this.blurRadius = blurRadius;
        this.lifeTime = lifeTime;
        this.priority = priority;
    }

    public enum SoundName
    {
        Footstep,
        Task,
        ElfBell
        //ADD MORE SOUNDS HERE
    }

    public static readonly SoundType Footstep =
        new SoundType(SoundName.Footstep, 4.5f, 1f, .5f, 4);

    public static readonly SoundType Task =
        new SoundType(SoundName.Task, 12f, 12f, 5f, 1);

    public static readonly SoundType ElfBell =
        new SoundType(SoundName.ElfBell, 150, 0, 5, 2);
    //ADD MORE SOUNDS HERE
}
