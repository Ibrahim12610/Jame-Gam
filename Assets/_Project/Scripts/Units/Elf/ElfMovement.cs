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
    [SerializeField] private AudioClip notifySound;
    
    private Transform _player;
    private NavMeshAgent _agent;
    private AudioSource _audioSource;
    private Coroutine _fadeRoutine;
    private ElfAnimator _animator;
    
    private int _currentPatrolIndex;
    private bool _isWaiting;
    private float _waitTimer;
    private bool _isPaused;
    private bool _playNotifySound = true;
    
    public Vector2 FacingOverride { get; private set; }
    public bool HasFacingOverride { get; private set; }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _audioSource = GetComponent<AudioSource>();
        _animator = GetComponentInChildren<ElfAnimator>();
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
            UpdateFacingTowardsPlayer();
            
            if (_playNotifySound)
            {
                _playNotifySound = false;
                StartNotifyAudio();
            }
            
            return;
        }

        _playNotifySound = true;
        StopNotifyAudio();
        
        HasFacingOverride = false;
        HandlePatrolLogic();
    }
    
    private void UpdateFacingTowardsPlayer()
    {
        Transform player = PlayerManager.Instance.GetPlayerTransform();
        if (player == null)
            return;

        Vector2 directionToPlayer =
            (player.position - transform.position);

        if (directionToPlayer.sqrMagnitude < 0.001f)
            return;

        FacingOverride = directionToPlayer.normalized;
        HasFacingOverride = true;

        _agent.velocity = Vector3.zero;
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
    
    
    private void StartNotifyAudio()
    {
        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
            _fadeRoutine = null;
        }

        _audioSource.volume = 1f;
        _audioSource.clip = notifySound;
        _audioSource.loop = true;

        if (!_audioSource.isPlaying)
            _audioSource.Play();
    }

    private void StopNotifyAudio()
    {
        if (_audioSource.isPlaying)
        {
            _fadeRoutine = StartCoroutine(
                GameUtility.FadeOutAndStop(_audioSource, 0.25f)
            );
        }
    }
}
