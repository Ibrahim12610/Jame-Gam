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
        //TODO: A fade audio to indicate closer of the UI
        Destroy(gameObject);
    }

    void Win()
    {
        Debug.Log("Cookie minigame won!");
        RaiseSuccess();
        Close();
    }
    
    void Fail()
    {
        onCooldown = true;
        Invoke(nameof(ResetCooldown), failCooldown);
        RaiseFail();
        Close();
        Debug.Log("Cookie minigame failed!");
    }
    void ResetCooldown()
    {
        onCooldown = false;
    }
    
}
