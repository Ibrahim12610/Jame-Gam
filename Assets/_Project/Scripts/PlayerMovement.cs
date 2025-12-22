using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //Components
    PlayerManager playerManager;
    Rigidbody2D rb;

    [Header("Settings")]
    public float moveSpeed;
    [SerializeField] float footstepSoundSignalDelay;
    
    Vector2 lastMoveDirection = Vector2.down;
    float lastFootstepTimestamp;
    

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerManager = GetComponent<PlayerManager>();
    }
    
    void Update()
    {
        Vector2 moveVector = Vector2.zero;

        if (Input.GetKey(KeyCode.W))
            moveVector += Vector2.up;
        if (Input.GetKey(KeyCode.A))
            moveVector += Vector2.left;
        if (Input.GetKey(KeyCode.S))
            moveVector += Vector2.down;
        if (Input.GetKey(KeyCode.D))
            moveVector += Vector2.right;

        moveVector.Normalize();
        moveVector *= moveSpeed;

        rb.linearVelocity = moveVector;

        if (moveVector.magnitude > 0 &&
            Time.time >= lastFootstepTimestamp + footstepSoundSignalDelay)
            CreateFootstepSound();
    }
    void CreateFootstepSound()
    {
        lastFootstepTimestamp = Time.time;
        EnemyAI[] listeners = playerManager.soundListeners;
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
}
