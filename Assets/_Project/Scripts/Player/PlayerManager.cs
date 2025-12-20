using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SetPlayerTransform(Transform t)
    {
        transform.position = t.position;
    }

    public void HandleDestroy()
    {
        Destroy(gameObject);
    }
}
