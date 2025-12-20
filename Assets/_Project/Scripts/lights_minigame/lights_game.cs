
using UnityEngine;
using UnityEngine.UI;

public class lights_game : MonoBehaviour
{
    public GameObject minigameCanvas;
    public Button[] lights;

    public float failCooldown = 1f;

    int safeIndex;
    bool active = false;
    public static bool onCooldown = false;
    
    
    private ChristmasLightsPickUp currentPickup;
    private int pickupAmount;

    void OnEnable()
    {
        if (onCooldown)
        {
            gameObject.SetActive(false);
            return;
        }

      
        currentPickup = null;

       
        Time.timeScale = 0f;

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
                MiniGameManager.Instance.OnMiniGameComplete(pickupAmount);
                Destroy(currentPickup.gameObject);
                currentPickup = null;
            }
        }
        else
        {
            Debug.Log("Sparks");
            onCooldown = true;
            Invoke(nameof(ResetCooldown), failCooldown);
           
            currentPickup = null;
        }

        Close();
    }
    
   
    public void SetPickup(ChristmasLightsPickUp pickup, int amount)
    {
        currentPickup = pickup;
        pickupAmount = amount;
    }

    void Close()
    {
       
        Time.timeScale = 1f;
        minigameCanvas.SetActive(false);
    }

    void ResetCooldown()
    {
        onCooldown = false;
    }
}
