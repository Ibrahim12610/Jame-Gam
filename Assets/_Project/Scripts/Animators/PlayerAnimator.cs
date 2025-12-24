using System;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;
    private Vector2 _lastMoveDirection = Vector2.down;
    private PlayerMovement _playerMovement;
    private bool _isCrouching;
    private bool _isDead;

    [SerializeField] private float attackDuration = 0.35f;

    private bool _isAttacking;
    private float _attackTimer;

    private const string State_idle_forward = "forward_idle";
    private const string State_idle_side = "side_idle";
    private const string State_idle_back = "back_idle";
    private const string State_walk_forward = "forward_walk";
    private const string State_walk_side = "side_walk";
    private const string State_walk_back = "walk_back";

    private const string State_attack_forward = "forward_attack";
    private const string State_attack_side = "side_attack";
    private const string State_attack_back = "back_attack";

    private const string State_crouch_back = "crouch_back";
    private const string State_crouch_front = "crouch_front";
    private const string State_crouch_side = "crouch_side";
    private const string State_crouch_side_walk = "crouch_side_walk";
    private const string State_crouch_front_walk = "crouch_front_walk";
    private const string State_crouch_back_walk = "crouch_back_walk";
    
    private const string State_death_forward = "forward_death";
    private const string State_death_side = "sideways_death";
    private const string State_death_back = "backward_death";

    public bool disableAnimator = false;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _playerMovement = GetComponent<PlayerMovement>();
        _isCrouching = _playerMovement.isCrouching;

    }
    void Update()
    {
        if (disableAnimator || _isDead) return;
        
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
        bool isCrouching = _playerMovement.isCrouching;

        if (!isCrouching)
        {
            if (absY > absX)
            {
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
                return isMoving ? State_walk_side : State_idle_side;
            }
        }
        else
        {
            if (absY > absX)
            {
                if (direction.y < 0)
                {
                    return isMoving ? State_crouch_front_walk : State_crouch_front;
                }
                else
                {
                    return isMoving ? State_crouch_back_walk : State_crouch_back;
                }
            }
            else
            {
                return isMoving ? State_crouch_side_walk : State_crouch_side;
            }
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

        _spriteRenderer.flipX = direction.x < 0;
    }
    
    public void TriggerDeath()
    {
        if (_isDead) return;

        _isDead = true;
        _isAttacking = false;

        string deathState = GetDeathState(_lastMoveDirection);

        _animator.Play(deathState, 0, 0f);
        
        float clipLength = GetAnimationLength(deathState);
        Invoke(nameof(FreezeAnimator), clipLength);
    }
    
    string GetDeathState(Vector2 direction)
    {
        float absX = Mathf.Abs(direction.x);
        float absY = Mathf.Abs(direction.y);

        if (absY > absX)
        {
            return direction.y < 0
                ? State_death_forward
                : State_death_back;
        }

        return State_death_side;
    }
    
    void FreezeAnimator()
    {
        _animator.speed = 0f;
    }
    
    float GetAnimationLength(string stateName)
    {
        var controller = _animator.runtimeAnimatorController;

        foreach (var clip in controller.animationClips)
        {
            if (clip.name == stateName)
                return clip.length;
        }

        return 0.5f;
    }
}
