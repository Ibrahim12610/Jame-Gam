using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float moveSpeed;
    [SerializeField] float crouchSpeedMultiplier = 0.5f; 
    
    private Vector2 lastMoveDirection = Vector2.down;
    bool isCrouching = false;
    float originalMoveSpeed;
    
    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        originalMoveSpeed = moveSpeed; 
    }
    
    void Update()
    {
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
        
        float currentSpeed = isCrouching ? originalMoveSpeed * crouchSpeedMultiplier : originalMoveSpeed;
        moveVector *= currentSpeed;

        rb.linearVelocity = moveVector;
        
        if (Input.GetKeyDown(KeyCode.LeftShift) )
        {
            if(isCrouching){
                UnCrouch();
            }
            else{
                Crouch();
            }
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
