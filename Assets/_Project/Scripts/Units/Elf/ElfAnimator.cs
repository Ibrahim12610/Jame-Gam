using System;
using UnityEngine;
using UnityEngine.AI;

public class ElfAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMoveDirection = Vector2.down;
    private NavMeshAgent _agent;
    private ElfMovement _movement;
    
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
        _agent = GetComponentInParent<NavMeshAgent>();
        _movement = GetComponentInParent<ElfMovement>();
    }
    
    void Update()
    {
        Vector2 velocity = _agent.velocity;

        Vector2 facingDirection =
            (_movement != null && _movement.HasFacingOverride)
                ? _movement.FacingOverride
                : _agent.desiredVelocity;

        UpdateAnimations(velocity, facingDirection);
    }

    void UpdateAnimations(Vector2 velocity, Vector2 desiredDirection)
    {
        bool isMoving = velocity.magnitude > 0.1f;
        
        if (desiredDirection.magnitude > 0.1f)
            _lastMoveDirection = desiredDirection.normalized;

        Vector2 facingDirection = _lastMoveDirection;
        
        if (_spriteRenderer != null)
        {
            if (Mathf.Abs(facingDirection.x) > Mathf.Abs(facingDirection.y))
                _spriteRenderer.flipX = facingDirection.x < 0;
        }

        string animationState = GetAnimationState(isMoving, facingDirection);
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
                stateName = isMoving ? State_walk_forward : State_idle_forward;
            else
                stateName = isMoving ? State_walk_back : State_idle_back;
        }
        else
            stateName = isMoving ? State_walk_side : State_idle_side;
        
        return stateName;
    }
}
