using System;
using UnityEngine;

public class EscapeIndicator : MonoBehaviour
{
    [SerializeField] private GameObject escapePanel;

    private void Awake()
    {
        escapePanel.SetActive(false);
    }

    private void OnEnable()
    {
        MiniGameManager.AllMiniGameComplete += HandleEscapeSequenceActivation;
    }

    private void OnDisable()
    {
        MiniGameManager.AllMiniGameComplete -= HandleEscapeSequenceActivation;
    }

    private void HandleEscapeSequenceActivation()
    {
        escapePanel.SetActive(true);
    }
}
