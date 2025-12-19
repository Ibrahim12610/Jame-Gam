using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    //Components

    Rigidbody2D rb;
    [Header("Animations")]
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private const string State_idle_forward= "forward_idle";
    private const string State_idle_side= "side_idle";
    private const string State_idle_back= "back_idle";
    private const string State_walk_forward= "forward_walk";
    private const string State_walk_side= "side_walk";
    private const string State_walk_back = "walk_back";

    [Header("Settings")]
    [SerializeField] float moveSpeed;

    
    private Vector2 lastMoveDirection = Vector2.down;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
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

        
        UpdateAnimations(moveVector);
    }

    void UpdateAnimations(Vector2 moveVector)
    {
        
        if (moveVector.magnitude > 0.1f)
        {
            lastMoveDirection = moveVector.normalized;
        }

        
        bool isMoving = moveVector.magnitude > 0.1f;
        Vector2 currentDirection = isMoving ? moveVector.normalized : lastMoveDirection;

       
        if (spriteRenderer != null)
        {
            float absX = Mathf.Abs(currentDirection.x);
            float absY = Mathf.Abs(currentDirection.y);
            
           
            if (absX > absY)
            {
                
                spriteRenderer.flipX = currentDirection.x < 0;
            }
        }

        string animationState = GetAnimationState(isMoving, currentDirection);

        animator.Play(animationState);
    }

    string GetAnimationState(bool isMoving, Vector2 direction)
    {
       
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        string stateName;

        if (absY > absX)
        {
            
            if (direction.y < 0)
            {
               
                stateName = isMoving ? State_walk_forward : State_idle_forward;
            }
            else
            {
                
                stateName = isMoving ? State_walk_back : State_idle_back;
            }
        }
        else
        {
            
            stateName = isMoving ? State_walk_side : State_idle_side;
        }

        return stateName;
    }
}
