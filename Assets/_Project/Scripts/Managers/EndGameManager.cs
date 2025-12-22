using System;
using UnityEngine;

public class EndGameManager : MonoBehaviour
{
    public static EndGameManager Instance;
    public static event Action ActivateEndGameSequence;

    [SerializeField] private AudioSource otherBackGroundMusic;
    
    private AudioSource _endGameMusic;
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        
        _endGameMusic =  GetComponent<AudioSource>();
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
        if(otherBackGroundMusic)
            otherBackGroundMusic.Stop(); // TODO: Fade Later
        
        _endGameMusic.Play();
    }
    
}
