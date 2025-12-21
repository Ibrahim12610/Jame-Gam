using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMoveDirection = Vector2.down;
    
    [SerializeField] private float attackDuration = 0.35f;

    private bool _isAttacking;
    private float _attackTimer;
    
    private const string State_idle_forward= "forward_idle";
    private const string State_idle_side= "side_idle";
    private const string State_idle_back= "back_idle";
    private const string State_walk_forward= "forward_walk";
    private const string State_walk_side= "side_walk";
    private const string State_walk_back = "walk_back";
    
    private const string State_attack_forward = "forward_attack";
    private const string State_attack_side    = "side_attack";
    private const string State_attack_back    = "back_attack";
    
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        Vector2 moveVector = Vector2.zero;

        if (!_isAttacking)
        {
            if (Input.GetKey(KeyCode.W)) moveVector += Vector2.up;
            if (Input.GetKey(KeyCode.A)) moveVector += Vector2.left;
            if (Input.GetKey(KeyCode.S)) moveVector += Vector2.down;
            if (Input.GetKey(KeyCode.D)) moveVector += Vector2.right;
        }

        UpdateAttackTimer();
        UpdateAnimations(moveVector);
    }
    
    public void TriggerAttack(Vector2? forcedDirection = null)
    {
        if (_isAttacking) return;

        _isAttacking = true;
        _attackTimer = attackDuration;

        if (forcedDirection.HasValue && forcedDirection.Value.sqrMagnitude > 0.01f)
        {
            _lastMoveDirection = forcedDirection.Value.normalized;
        }
    }
    
    void StartAttack()
    {
        _isAttacking = true;
        _attackTimer = attackDuration;
    }

    void UpdateAttackTimer()
    {
        if (!_isAttacking) return;

        _attackTimer -= Time.deltaTime;
        if (_attackTimer <= 0f)
        {
            _isAttacking = false;
        }
    }

    void UpdateAnimations(Vector2 moveVector)
    {
        if (moveVector.magnitude > 0.1f)
        {
            _lastMoveDirection = moveVector.normalized;
        }

        Vector2 direction = _isAttacking
            ? _lastMoveDirection
            : (moveVector.magnitude > 0.1f ? moveVector.normalized : _lastMoveDirection);

        UpdateSpriteFlip(direction);

        string state = _isAttacking
            ? GetAttackState(direction)
            : GetMovementState(moveVector.magnitude > 0.1f, direction);

        _animator.Play(state);
    }
    
    string GetMovementState(bool isMoving, Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absY > absX)
        {
            // Vertical
            if (direction.y < 0)
            {
                return isMoving ? State_walk_forward : State_idle_forward;
            }
            else
            {
                return isMoving ? State_walk_back : State_idle_back;
            }
        }
        else
        {
            // Horizontal
            return isMoving ? State_walk_side : State_idle_side;
        }
    }

    string GetAttackState(Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absY > absX)
        {
            return direction.y < 0
                ? State_attack_forward
                : State_attack_back;
        }

        return State_attack_side;
    }
    
    void UpdateSpriteFlip(Vector2 direction)
    {
        if (_spriteRenderer == null) return;

        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            _spriteRenderer.flipX = direction.x < 0;
        }
    }
}
