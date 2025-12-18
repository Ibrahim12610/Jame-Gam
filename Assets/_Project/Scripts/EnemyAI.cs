using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    //Components
    [HideInInspector] public NavMeshAgent agent;
    Animator animator;

    [Header("Sight Settings")]
    [SerializeField] int rays;
    [SerializeField] float rayOffset;
    [SerializeField] float distance;
    [SerializeField] float angle;
    [SerializeField] bool debugShowSight;

    //Private Vars
    bool canSeePlayer = false;

    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    private void Update()
    {
        canSeePlayer = CanSeeLayer(LayerMask.NameToLayer("Player"));
        animator.SetBool("canSeePlayer", canSeePlayer);
    }
    bool CanSeeLayer(LayerMask layer)
    {
        bool canSee = false;
        float angleStep = angle / (rays - 1);
        float startAngle = -angle / 2f;

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
}