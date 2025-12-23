using UnityEngine;
using UnityEngine.UI;
using System;

public class Lockpick_minigame : MiniGame
{
    [Header("UI")]
    [SerializeField] private Image successArc;
    [SerializeField] private RectTransform needle;
    [SerializeField] private GameObject canvas;

    [Header("Gameplay")]
    [SerializeField] private float needleSpeed = 180f;
    [SerializeField, Range(0.05f, 0.9f)] private float startArcSize = 0.6f;
    [SerializeField] private float arcShrinkPerRound = 0.08f;
    [SerializeField] private int roundsToWin = 5;

    [Header("Feel")]
    [SerializeField] private float forgiveness = 0.02f;

    public Action<bool> OnComplete;

    float needleAngle;
    int currentRound;
    bool active;

    void OnEnable()
    {
        active = true;
        currentRound = 0;
        needleAngle = 0f;

        // Activate canvas when minigame starts
        if (canvas != null)
        {
            canvas.SetActive(true);
        }

        needle.pivot = new Vector2(0.5f, 0f);

        StartRound();
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (!active) return;

        needleAngle = (needleAngle + needleSpeed * Time.unscaledDeltaTime) % 360f;
        needle.localRotation = Quaternion.Euler(0f, 0f, -needleAngle);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckSkill();
        }

        // Optional cancel
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Fail();
        }
    }

    void StartRound()
    {
        float arcSize = Mathf.Max(0.05f, startArcSize - currentRound * arcShrinkPerRound);

        successArc.type = Image.Type.Filled;
        successArc.fillMethod = Image.FillMethod.Radial360;
        successArc.fillClockwise = true;

        // 0 = Top, 1 = Right, 2 = Bottom, 3 = Left
        successArc.fillOrigin = UnityEngine.Random.Range(0, 4);

        // Filled = fail, unfilled = success
        successArc.fillAmount = 1f - arcSize;
        successArc.transform.localRotation = Quaternion.identity;

        Debug.Log(
            $"[Lockpick] Round {currentRound + 1}/{roundsToWin} | " +
            $"Origin {(successArc.fillOrigin * 90f)}° | " +
            $"Success {(arcSize * 360f):F0}°"
        );
    }

    void CheckSkill()
    {
        // Needle rotates clockwise visually → invert
        float needleNorm = 1f - ((needleAngle % 360f) / 360f);

        // Convert origin to normalized space
        float originNorm = successArc.fillOrigin / 4f;

        // Relative position in fill space
        float relative = needleNorm - originNorm;
        if (relative < 0f) relative += 1f;

        bool success = relative > (successArc.fillAmount - forgiveness);

        Debug.Log(
            $"[Lockpick] Needle {needleAngle:F1}° | " +
            $"Relative {relative:F3} | " +
            $"Fill {successArc.fillAmount:F3} | SUCCESS {success}"
        );

        if (success)
        {
            currentRound++;
            if (currentRound >= roundsToWin)
            {
                Win();
            }
            else
            {
                StartRound();
            }
        }
        else
        {
            Fail();
        }
    }

    void Win()
    {
        Debug.Log("[Lockpick] SUCCESS");
        
        // Close the canvas when winning
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        Destroy(gameObject);
        
        Complete(true);
    }

    void Fail()
    {
        Debug.Log("[Lockpick] FAILED");
        
        // Close the canvas on fail
        if (canvas != null)
        {
            canvas.SetActive(false);
        }
        Destroy(gameObject);
        
        Complete(false);
    }

    void Complete(bool success)
    {
        active = false;

        OnComplete?.Invoke(success);
    }
}
