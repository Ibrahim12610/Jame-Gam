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

        if (_isPaused)
        {
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
    
    private void SetNextPatrolPoint()
    {
        var target = _patrolPoints[_currentPatrolIndex];
        if(target != null)
            _agent.SetDestination(target.position);
        
        _currentPatrolIndex++;
        if (_currentPatrolIndex >= _patrolPoints.Count)
            _currentPatrolIndex = 0;
    }
    
    private void PausePatrol()
    {
        _isPaused = true;
        _isWaiting = false;
        _agent.isStopped = false;
    }

    private void ResumePatrol()
    {
        _isPaused = false;
        _agent.isStopped = false;
        if (!_agent.hasPath)
            SetNextPatrolPoint();
    }
}
