using System;
using UnityEngine;
using UnityEngine.UI;

public class cookie_minigame : MiniGame
{
    public static bool onCooldown;
    public float failCooldown = 1f;
    public GameObject canvas;
    bool active;
    int cookiesRemaining;

    void Awake()
    {
        cookiesRemaining = GetComponentsInChildren<cookies>().Length;
    }

    void OnEnable()
    {
        canvas.SetActive(true);
        active = true;
        
        // Reset cookie count
        cookiesRemaining = GetComponentsInChildren<cookies>().Length;
    }
    
    public void CookiePlaced()
    {
        if (!active) return;
        cookiesRemaining--;
        if (cookiesRemaining <= 0)
        {
            Win();
        }
    }

    void Update()
    {
        if (!active) return;
    }

    void Close()
    {
        active = false;
        Destroy(gameObject);
    }

    void Win()
    {
        RaiseSuccess();
        Debug.Log("Cookie minigame won!");
        Close();
    }
    
    void Fail()
    {
        onCooldown = true;
        Invoke(nameof(ResetCooldown), failCooldown);
        RaiseFail();
        Debug.Log("Cookie minigame failed!");
        Close();
    }
    void ResetCooldown()
    {
        onCooldown = false;
    }
    
}
