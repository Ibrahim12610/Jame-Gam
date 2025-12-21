using System;
using UnityEngine;
using UnityEngine.Events;

public class ElfHealthController : MonoBehaviour
{
    [SerializeField] private float totalElfHealth = 100f;
    [SerializeField] private float currentElfHealth = 100f;
    
    public UnityEvent onElfKilled;
    public UnityEvent onElfDamaged;

    public void TakeDamage(float damage)
    {
        currentElfHealth -= damage;
        onElfDamaged.Invoke();
        Debug.Log("Attacking Elf");
        
        if (currentElfHealth <= 0)
        {
            Debug.Log("Elf Killed");
            onElfKilled.Invoke();
        }
    }
}
