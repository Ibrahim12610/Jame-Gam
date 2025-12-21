using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ElfMovement : MonoBehaviour
{
    [Header("Patrol Points")] 
    private readonly List<Transform> _patrolPoints = new();
    
    [Header("Patrol Settings")]
    [SerializeField] private float arriveDistance = 0.2f;
    [SerializeField] private GameObject patrolSection;
    [SerializeField] private float waitPerPatrolPoint = 0f;
    
    private Transform _player;
    private NavMeshAgent _agent;
    
    private int _currentPatrolIndex;
    private bool _isWaiting;
    private float _waitTimer;
    private bool _isPaused;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _agent.updateRotation = false;
        _agent.updateUpAxis = false;
        _agent.autoRepath = false;

        CachePatrolPoints();
    }

    private void CachePatrolPoints()
    {
        var patrolParent = patrolSection.transform;
        
        _patrolPoints.Clear();

        for (int i = 0; i < patrolParent.childCount; i++)
        {
            _patrolPoints.Add(patrolParent.GetChild(i));
        }
    }
    
    private void OnEnable()
    {
        ElfDetectionController.OnElfDetectedPlayer += PausePatrol;
        ElfDetectionController.OnElfNoLongerDetectedPlayer += ResumePatrol;
    }

    private void OnDisable()
    {
        ElfDetectionController.OnElfDetectedPlayer -= PausePatrol;
        ElfDetectionController.OnElfNoLongerDetectedPlayer -= ResumePatrol;
    }

    private void Start()
    {
        if (_patrolPoints.Count > 0)
        {
            SetNextPatrolPoint();
        }
    }

    private void Update()
    {
        if (!_agent.enabled || _patrolPoints.Count == 0)
            return;
        
        if (!_agent.isOnNavMesh)
        {
            Debug.LogWarning("Attempting to return to navmesh");
            TryReturnToNavMesh();
            return;
        }

        if (_isPaused)
        {
            Debug.Log("Attempting to head towards player");
            _agent.SetDestination(PlayerManager.Instance.GetPlayerTransform().position);
        }

        HandlePatrolLogic();
    }

    private void HandlePatrolLogic()
    {
        if (_isPaused || !_agent.enabled || _patrolPoints.Count == 0)
            return;
        
        if (!_isWaiting && _agent.remainingDistance <= arriveDistance)
        {
            if (waitPerPatrolPoint > 0f)
            {
                _isWaiting = true;
                _waitTimer = waitPerPatrolPoint;
                _agent.isStopped = true;
            }
            else
            {
                SetNextPatrolPoint();
            }

            return;
        }
        
        if (_isWaiting)
        {
            _waitTimer -= Time.deltaTime;

            if (_waitTimer <= 0f)
            {
                _isWaiting = false;
                _agent.isStopped = false;
                SetNextPatrolPoint();
            }
        }
    }
    
    private bool TryReturnToNavMesh(float searchRadius = 1f)
    {
        if (_agent.isOnNavMesh)
            return true;

        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, searchRadius, NavMesh.AllAreas))
        {
            _agent.SetDestination(hit.position);
            return false;
        }

        return false;
    }
    
    private void SetNextPatrolPoint()
    {
        if (!_agent.enabled || !_agent.isOnNavMesh || _patrolPoints.Count == 0)
            return;

        var target = _patrolPoints[_currentPatrolIndex];
        if (target != null)
            _agent.SetDestination(target.position);

        _currentPatrolIndex = (_currentPatrolIndex + 1) % _patrolPoints.Count;
    }
    
    private void PausePatrol()
    {
        _isPaused = true;
        _isWaiting = false;

        if (_agent.enabled && _agent.isOnNavMesh)
            _agent.isStopped = false;
    }

    private void ResumePatrol()
    {
        _isPaused = false;

        if (!_agent.enabled || !_agent.isOnNavMesh)
            return;

        _agent.isStopped = false;

        if (!_agent.hasPath)
            SetNextPatrolPoint();
    }
}
