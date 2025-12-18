using System;
using _Project.Scripts.Managers;
using UnityEngine;

public class ChristmasLightsPickUp : MonoBehaviour
{
    [SerializeField] private int amount;
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        if (!otherCollider.CompareTag("Player")) return;

        ChristmasLightManager.Instance.OnChristmasLightCollected(amount);
        Destroy(gameObject);
    }
}
