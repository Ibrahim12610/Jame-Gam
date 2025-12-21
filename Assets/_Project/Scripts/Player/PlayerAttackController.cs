using System;
using System.Collections;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.AI;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] private GameObject attackPoint;
    [SerializeField] private CircleCollider2D impulseCollider;
    [SerializeField] private float colliderOffsetDistance = 0.4f;
    
    [SerializeField] private float damageDelay = 0f;
    [SerializeField] private float attackDuration = .1f;
    
    
    private Vector2 _moveInput;
    private Vector2 _facingDirection = Vector2.down;
    private CircleCollider2D _directionalCollider;
    
    [HideInInspector] public bool isAttacking = false;
    
    private void Awake()
    {
        attackPoint.SetActive(false);
        _directionalCollider = attackPoint.GetComponent<CircleCollider2D>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isAttacking)
        {
            StopAllCoroutines();
            isAttacking = true;
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

        Vector2 cardinalDir = GetCardinalDirection(_facingDirection);
        
        _directionalCollider.offset = cardinalDir * colliderOffsetDistance;
        impulseCollider.offset = cardinalDir * colliderOffsetDistance;
    }

    private Vector2 GetCardinalDirection(Vector2 dir)
    {
        if (Mathf.Abs(dir.x) > Mathf.Abs(dir.y))
            return new Vector2(Mathf.Sign(dir.x), 0);
        else
            return new Vector2(0, Mathf.Sign(dir.y));
    }
}




