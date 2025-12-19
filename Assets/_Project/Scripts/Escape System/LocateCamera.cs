using System;
using UnityEngine;

public class LocateCamera : MonoBehaviour
{
    private Canvas _canvas;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
    }

    private void Start()
    {
        _canvas.worldCamera = Camera.main;  
    }
}
