
using TMPro;
using UnityEngine;

public class MiniGameIndicatorController : MonoBehaviour, IMiniGame
{
    [SerializeField] private TMP_Text tmpText;
    
    private void OnEnable()
    {
        MiniGameManager.OnChristmasLightsUpdated += HandleIndicatorChange;
    }

    private void OnDisable()
    {
        MiniGameManager.OnChristmasLightsUpdated -= HandleIndicatorChange;
    }

    private void Start()
    {
        HandleIndicatorChange();
    }

    private void HandleIndicatorChange()
    {
        tmpText.text = $"Remaining Objectives: {MiniGameManager.Instance.GetMiniGameTotal()}";
    }
}