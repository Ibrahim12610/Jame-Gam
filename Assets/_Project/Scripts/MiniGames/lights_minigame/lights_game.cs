using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class lights_game : MiniGame
{
    public GameObject minigameCanvas;
    public Button[] lights;

    public float failCooldown = 1f;

    int safeIndex;
    bool active = false;
    public static bool onCooldown = false;
    
    
    private MinigamePickup currentPickup;
    private int pickupAmount;
    
    
    void OnEnable()
    {
        currentPickup = null;
        
        active = true;
        safeIndex = Random.Range(0, lights.Length);

        for (int i = 0; i < lights.Length; i++)
        {
            int index = i;
            lights[i].onClick.RemoveAllListeners();
            lights[i].onClick.AddListener(() => OnLightClicked(index));
        }
    }

    void OnLightClicked(int index)
    {
        if (!active) return;
        active = false;

        if (index == safeIndex)
        {
            Debug.Log("Clean steal");
           
            if (currentPickup != null)
            {
                currentPickup = null;
            }
            RaiseSuccess();
        }
        else
        {
            Debug.Log("Sparks");
            onCooldown = true;
            Invoke(nameof(ResetCooldown), failCooldown);
            RaiseFail();
            currentPickup = null;
        }

        Close();
    }
    
   
    public void SetPickup(MinigamePickup pickup, int amount)
    {
        currentPickup = pickup;
        pickupAmount = amount;
    }

    void Close()
    {
       
        minigameCanvas.SetActive(false);
        Destroy(gameObject);
    }

    void ResetCooldown()
    {
        onCooldown = false;
    }
}
