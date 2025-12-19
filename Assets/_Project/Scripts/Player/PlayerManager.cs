using System;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetPlayerTransform(Transform t)
    {
        transform.position = t.position;
    }
}
