using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] public float moveSpeed;
    [SerializeField] float crouchSpeedMultiplier = 0.5f;
    [SerializeField] float footstepSoundSignalDelay;
    
    private Vector2 _lastMoveDirection = Vector2.down;
    public bool isCrouching = false;
    private float _originalMoveSpeed;

    private Rigidbody2D rb;
    float lastFootstepTimestamp;

    [HideInInspector] public bool disableMovement = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        _originalMoveSpeed = moveSpeed;
    }

    void Update()
    {
        if (disableMovement) return;
        
        Vector2 moveVector = Vector2.zero;

        if (PlayerManager.Instance.IsInImpulse()) return;

        if (Input.GetKey(KeyCode.W))
            moveVector += Vector2.up;
        if (Input.GetKey(KeyCode.A))
            moveVector += Vector2.left;
        if (Input.GetKey(KeyCode.S))
            moveVector += Vector2.down;
        if (Input.GetKey(KeyCode.D))
            moveVector += Vector2.right;

        moveVector.Normalize();

        float currentSpeed = isCrouching ? _originalMoveSpeed * crouchSpeedMultiplier : _originalMoveSpeed;
        moveVector *= currentSpeed;

        rb.linearVelocity = moveVector;
        
        if (moveVector.magnitude > 0 &&
            Time.time >= lastFootstepTimestamp + footstepSoundSignalDelay)
            CreateFootstepSound();

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (isCrouching)
            {
                UnCrouch();
            }
            else
            {
                Crouch();
            }
        }
    }
    
    void CreateFootstepSound()
    {
        lastFootstepTimestamp = Time.time;
        EnemyAI[] listeners = PlayerManager.Instance.soundListeners;
        SoundSignal signal =
            new SoundSignal(SoundType.Footstep, transform.position, Time.time);
        DrawCircle(transform.position, signal.type.travelDistance);
        foreach (EnemyAI listener in listeners)
            listener.HearSound(signal);
    }

    //DEBUG
    Vector2 center = Vector2.zero;
    float radius = 0;
    public void DrawCircle(Vector2 center, float radius)
    {
        this.center = center;
        this.radius = radius;
    }
    private void OnDrawGizmos()
    {
        if (center != Vector2.zero && radius != 0)
        {
            Gizmos.color = Color.grey;
            Gizmos.DrawWireSphere(center, radius);
        }
    }

    void Crouch()
    {
        isCrouching = true;
    }

    void UnCrouch()
    {
        isCrouching = false;
    }
}
