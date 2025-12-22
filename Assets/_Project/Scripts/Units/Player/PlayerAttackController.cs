using System.Collections;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private CircleCollider2D impulseCollider;
    [SerializeField] private float colliderOffsetDistance = 0.4f;
    
    [SerializeField] private float damageDelay = 0f;
    [SerializeField] private float attackDuration = .1f;
    [SerializeField] private AudioClip attackSound;
    
    [HideInInspector] public bool isAttacking = false;
    [HideInInspector] public bool disableAttack = false;
    
    private CircleCollider2D _directionalCollider;
    private AudioSource _audioSource;
    private Vector2 _moveInput;
    private Vector2 _facingDirection = Vector2.down;
    private PlayerAnimator _playerAnimator;
    
    
    
    private void Awake()
    {
        attackPoint.SetActive(false);
        _directionalCollider = attackPoint.GetComponent<CircleCollider2D>();
        _audioSource = GetComponent<AudioSource>();
        _playerAnimator = GetComponentInParent<PlayerAnimator>();
    }

    private void Update()
    {
        if (disableAttack) return;
        
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            StopAllCoroutines();
            isAttacking = true;
            _audioSource.PlayOneShot(attackSound);
            _playerAnimator.TriggerAttack();
            StartCoroutine(HandleAttackPointActivation());
        }

        _moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        );

        if (_moveInput.sqrMagnitude > 0.01f)
            _facingDirection = _moveInput.normalized;
        
        UpdateDirectionalCollider();
    }

    private IEnumerator HandleAttackPointActivation()
    {
        if (damageDelay > 0f)
            yield return new WaitForSeconds(damageDelay);

        attackPoint.SetActive(true);
        
        yield return new WaitForSeconds(attackDuration);

        attackPoint.SetActive(false);
        isAttacking = false;
    }
    
    private void UpdateDirectionalCollider()
    {
        if (_directionalCollider == null)
            return;

        Vector2 dir = _facingDirection.normalized;
        
        _directionalCollider.offset = dir * colliderOffsetDistance;
        impulseCollider.offset = dir * colliderOffsetDistance;
    }

    private Vector2 GetCardinalDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }
}




