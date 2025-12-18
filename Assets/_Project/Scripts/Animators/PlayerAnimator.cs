using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMoveDirection = Vector2.down;
    
    private const string State_idle_forward= "forward_idle";
    private const string State_idle_side= "side_idle";
    private const string State_idle_back= "back_idle";
    private const string State_walk_forward= "forward_walk";
    private const string State_walk_side= "side_walk";
    private const string State_walk_back = "walk_back";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
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
        
        UpdateAnimations(moveVector);
    }

    void UpdateAnimations(Vector2 moveVector)
    {
        
        if (moveVector.magnitude > 0.1f)
        {
            _lastMoveDirection = moveVector.normalized;
        }

        
        bool isMoving = moveVector.magnitude > 0.1f;
        Vector2 currentDirection = isMoving ? moveVector.normalized : _lastMoveDirection;

       
        if (_spriteRenderer != null)
        {
            float absX = Mathf.Abs(currentDirection.x);
            float absY = Mathf.Abs(currentDirection.y);
            
           
            if (absX > absY)
            {
                _spriteRenderer.flipX = currentDirection.x < 0;
            }
        }

        string animationState = GetAnimationState(isMoving, currentDirection);
        
        _animator.Play(animationState);
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
