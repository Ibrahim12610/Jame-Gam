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
    private bool _isAlerted;
    
    private const string IdleForward= "elf_forward_idle";
    private const string IdleSide= "elf_sideways_idle";
    private const string IdleBack= "elf_backwards_idle";
    private const string WalkForward= "elf_forward_walk";
    private const string WalkSide= "elf_sideways_walk";
    private const string WalkBack = "elf_backward_walk";
    
    private const string BellForward= "elf_forward_bell";
    private const string BellSide= "elf_sideways_bell";
    private const string BellBack = "elf_backward_bell";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _agent = GetComponentInParent<NavMeshAgent>();
        _movement = GetComponentInParent<ElfMovement>();
    }
    
    private void OnEnable()
    {
        ElfDetectionController.OnElfDetectedPlayer += OnDetectedPlayer;
        ElfDetectionController.OnElfNoLongerDetectedPlayer += OnLostPlayer;
    }

    private void OnDisable()
    {
        ElfDetectionController.OnElfDetectedPlayer -= OnDetectedPlayer;
        ElfDetectionController.OnElfNoLongerDetectedPlayer -= OnLostPlayer;
    }
    
    void Update()
    {
        Vector2 velocity = _agent != null ? _agent.velocity : Vector2.zero;

        Vector2 desiredDirection =
            (_movement != null && _movement.HasFacingOverride)
                ? _movement.FacingOverride
                : (_agent != null ? _agent.desiredVelocity : Vector2.zero);

        UpdateAnimations(velocity, desiredDirection);
    }
    
    private void OnDetectedPlayer()
    {
        _isAlerted = true;
        
        if (_movement != null && _movement.HasFacingOverride)
        {
            _lastMoveDirection = _movement.FacingOverride;
        }
    }

    private void OnLostPlayer()
    {
        _isAlerted = false;
    }

    void UpdateAnimations(Vector2 velocity, Vector2 desiredDirection)
    {
        bool isMoving = velocity.magnitude > 0.1f && !_isAlerted;

        if (desiredDirection.magnitude > 0.1f)
            _lastMoveDirection = desiredDirection.normalized;

        Vector2 facingDirection = _lastMoveDirection;

        UpdateSpriteFlip(facingDirection);

        string state = _isAlerted
            ? GetBellState(facingDirection)
            : GetMovementState(isMoving, facingDirection);

        _animator.Play(state);
    }

    string GetBellState(Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absY > absX)
            return direction.y < 0 ? BellForward : BellBack;

        return BellSide;
    }
    
    string GetMovementState(bool isMoving, Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absY > absX)
        {
            return direction.y < 0
                ? (isMoving ? WalkForward : IdleForward)
                : (isMoving ? WalkBack : IdleBack);
        }

        return isMoving ? WalkSide : IdleSide;
    }
    
    void UpdateSpriteFlip(Vector2 direction)
    {
        if (_spriteRenderer == null)
            return;
        
        _spriteRenderer.flipX = direction.x < 0;
    }
}
