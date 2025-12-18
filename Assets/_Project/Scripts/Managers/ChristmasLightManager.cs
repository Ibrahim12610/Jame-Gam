using System;
using UnityEngine;

namespace _Project.Scripts.Managers
{
    public class ChristmasLightManager : MonoBehaviour
    {
        public static ChristmasLightManager Instance;
        [SerializeField] private int christmasLightsAmount;
        
        public static event Action OnChristmasLightsUpdated;
        
        private void Awake()
        {
            Instance = this;
        }
        
        public void OnChristmasLightCollected(int amount)
        {
            christmasLightsAmount -= amount;
            OnChristmasLightsUpdated?.Invoke();
        }

        public void SetChristmasLightAmount(int amount)
        {
            christmasLightsAmount = amount;
        }

        public int GetChristmasLightAmount()
        {
            return christmasLightsAmount;
        }
        
    }
}
