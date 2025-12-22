using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;
    
    private ImpulseController _impulseController;
    private PlayerAttackController _playerAttackController;
    public PlayerMovement _playerMovement;
    private PlayerAnimator _playerAnimator;

    [HideInInspector] public EnemyAI[] soundListeners;
    
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
        soundListeners = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
    }
    
    public bool IsInImpulse() => _impulseController.IsInImpulse();

    public void DisablePlayerAttack() =>
        _playerAttackController.disableAttack = true;

    public void EnablePlayerAttack() =>
        _playerAttackController.disableAttack = false;

    public void DisablePlayerMovement() =>
        _playerMovement.disableMovement = true;
    
    public void EnablePlayerMovement() =>
        _playerMovement.disableMovement = false;
    
    public void DisablePlayerAnimator() =>
        _playerAnimator.disableAnimator = true;
    
    public void EnablePlayerAnimator() =>
        _playerAnimator.disableAnimator = false;

}
