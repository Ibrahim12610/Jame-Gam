using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    [HideInInspector] public PlayerMovement movement;
    [HideInInspector] public EnemyAI[] soundListeners;
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        movement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        FindSoundListenersInScene();
    }

    public void SetPlayerTransform(Transform t)
    {
        transform.position = t.position;
    }

    //RUN THIS WHEN THE GAME BEGINS
    public void FindSoundListenersInScene()
    {
        soundListeners = FindObjectsByType<EnemyAI>(FindObjectsSortMode.None);
    }
    
}
