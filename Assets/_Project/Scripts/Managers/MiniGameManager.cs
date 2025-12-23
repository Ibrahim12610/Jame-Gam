using System;
using UnityEngine;

public class MiniGameManager : MonoBehaviour
{
    public static MiniGameManager Instance;
    [SerializeField] private int miniGameAmount;

    public static event Action OnChristmasLightsUpdated;
    public static event Action AllMiniGameComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        OnMiniGameComplete(0);
    }

    public void OnMiniGameComplete(int amount)
    {
        miniGameAmount -= amount;
        OnChristmasLightsUpdated?.Invoke();

        if (miniGameAmount <= 0)
        {
            AllMiniGameComplete?.Invoke();
        }
    }

    public int GetMiniGameTotal()
    {
        return miniGameAmount;
    }

}


