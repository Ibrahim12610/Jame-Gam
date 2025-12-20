
using System;
using TMPro;
using UnityEngine;

public class MiniGameIndicatorController : MonoBehaviour, IMiniGame
{
    [SerializeField] private TMP_Text tmpText;
    [SerializeField] private GameObject miniGamePanel;

    private void Awake()
    {
        miniGamePanel.SetActive(true);
    }

    private void OnEnable()
    {
        MiniGameManager.OnChristmasLightsUpdated += HandleIndicatorChange;
        MiniGameManager.AllMiniGameComplete += HandleEscapeSequenceActivation;
    }

    private void OnDisable()
    {
        MiniGameManager.OnChristmasLightsUpdated -= HandleIndicatorChange;
        MiniGameManager.AllMiniGameComplete -= HandleEscapeSequenceActivation;
    }

    private void Start()
    {
        HandleIndicatorChange();
    }

    private void HandleIndicatorChange()
    {
        tmpText.text = $"Remaining Objectives: {MiniGameManager.Instance.GetMiniGameTotal()}";
    }
    
    private void HandleEscapeSequenceActivation()
    {
        miniGamePanel.SetActive(false);
    }
}