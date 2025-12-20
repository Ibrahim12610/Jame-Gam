using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    public PlayerMovement movement;
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

    public void SetPlayerTransform(Transform t)
    {
        transform.position = t.position;
    }
}
