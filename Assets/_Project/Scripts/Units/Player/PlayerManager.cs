using System;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    private ImpulseController _impulseController;
    private PlayerAttackController _playerAttackController;
    public PlayerMovement _playerMovement;
    private PlayerAnimator _playerAnimator;
    
    private bool _pauseMenuActive = false;
    private bool _popUpActive = false;

    [HideInInspector] public SantaAI santaListener;

    public UnityEvent onKillSequence;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _impulseController = GetComponent<ImpulseController>();
        _playerAttackController = GetComponentInChildren<PlayerAttackController>();
        _playerMovement = GetComponent<PlayerMovement>();
        _playerAnimator = GetComponent<PlayerAnimator>();
    }
    private void Start()
    {
        FindSoundListenersInScene();
    }
    
    public void SetPlayerTransform(Transform t)
    {
        transform.position = t.position;
    }

    public void HandleDestroy()
    {
        Destroy(gameObject);
    }

    public Transform GetPlayerTransform()
    {
        return gameObject.transform;
    }
    
    //RUN THIS WHEN THE GAME BEGINS
    public void FindSoundListenersInScene()
    {
        santaListener = FindFirstObjectByType<SantaAI>();
    }
    
    public bool IsInImpulse() => _impulseController.IsInImpulse();
    
    public void DisablePlayerLogic(string origin)
    {
        if (origin == "PauseMenu") _pauseMenuActive = true;
        if (origin == "PopUp") _popUpActive = true;

        _playerAttackController.disableAttack = true;
        _playerMovement.disableMovement = true;
        _playerAnimator.disableAnimator = true;
    }

    public void EnablePlayerLogic(string origin)
    {
        if(origin == "PauseMenu") _pauseMenuActive = false;
        if(origin == "PopUp") _popUpActive = false;

        if (!_popUpActive && !_pauseMenuActive)
        {
            _playerAttackController.disableAttack = false;
            _playerMovement.disableMovement = false;
            _playerAnimator.disableAnimator = false;
        }
    }

    public void HandleonKillSequence()
    {
        onKillSequence.Invoke();
    }
}
