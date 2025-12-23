using UnityEngine;
using UnityEngine.AI;

public class SantaAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private NavMeshAgent _agent;

    private Vector2 _lastMoveDirection = Vector2.down;
    
    private const string IdleForward = "santa_forward_idle";
    private const string IdleSide    = "santa_sideways_idle";
    private const string IdleBack    = "santa_backward_idle";

    private const string WalkForward = "santa_forward_walk";
    private const string WalkSide    = "santa_sideways_walk";
    private const string WalkBack    = "santa_backward_walk";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _agent = GetComponentInParent<NavMeshAgent>();
    }
    
    private void Update()
    {
        if (_agent == null)
            return;

        Vector2 velocity = _agent.velocity;
        Vector2 desiredDirection = GetDesiredDirection();
        
        UpdateAnimations(velocity, desiredDirection);
    }
    
    private Vector2 GetDesiredDirection()
    {
        if (_agent.desiredVelocity.sqrMagnitude > 0.01f)
            return _agent.desiredVelocity.normalized;

        return _lastMoveDirection;
    }
    
    private void UpdateAnimations(Vector2 velocity, Vector2 desiredDirection)
    {
        bool isMoving = velocity.magnitude > 0.1f;

        if (desiredDirection.magnitude > 0.1f)
            _lastMoveDirection = desiredDirection;

        UpdateSpriteFlip(_lastMoveDirection);

        string state = GetMovementState(isMoving, _lastMoveDirection);

        _animator.Play(state);
    }

    private string GetMovementState(bool isMoving, Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absY > absX)
        {
            if (direction.y < 0)
                return isMoving ? WalkForward : IdleForward;
            else
                return isMoving ? WalkBack : IdleBack;
        }

        return isMoving ? WalkSide : IdleSide;
    }
    
    private void UpdateSpriteFlip(Vector2 direction)
    {
        if (_spriteRenderer == null)
            return;

        _spriteRenderer.flipX = direction.x < 0;
    }
}
