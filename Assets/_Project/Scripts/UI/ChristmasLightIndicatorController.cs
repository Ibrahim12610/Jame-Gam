using System;
using _Project.Scripts.Managers;
using TMPro;
using UnityEngine;

public class ChristmasLightIndicatorController : MonoBehaviour
{
    [SerializeField] private TMP_Text tmpText;
    
    private void OnEnable()
    {
        ChristmasLightManager.OnChristmasLightsUpdated += HandleIndicatorChange;
        HandleIndicatorChange();
    }

    private void OnDisable()
    {
        ChristmasLightManager.OnChristmasLightsUpdated += HandleIndicatorChange;
    }

    private void HandleIndicatorChange()
    {
        tmpText.text = $"Remaining Lights: {ChristmasLightManager.Instance.GetChristmasLightAmount()}";
    }
}
