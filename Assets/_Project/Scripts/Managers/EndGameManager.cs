using System;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;

    public static event Action ActivateEndGameSequence;

    public GameObject exit;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void OnEnable()
    {
        MiniGameManager.AllMiniGameComplete += HandleEndGameActivation;
    }

    private void OnDisable()
    {
        MiniGameManager.AllMiniGameComplete -= HandleEndGameActivation;
    }

    private void HandleEndGameActivation()
    {
        ActivateEndGameSequence?.Invoke();
        exit.SetActive(true);
    }
    
}
