using UnityEngine;
using UnityEngine.UI;

public class Lockpick_minigame : MiniGame
{
    [Header("UI")]
    public GameObject canvas;
    public Image successArc;     
    public RectTransform needle;

    [Header("Gameplay")]
    public float needleSpeed = 180f;
    public float startArcSize = 0.6f;
    public float arcShrink = 0.1f;
    public int roundsToWin = 3;
    public float failCooldown = 1f;

    float angle;
    float arcStart;
    float arcSize;
    int round;
    bool active;

    public static bool onCooldown;

    void OnEnable()
    {
        canvas.SetActive(true);

        round = 0;
        angle = 0f;
        active = true;

        StartRound();
    }

    void Update()
    {
        //if (!active) return;

        angle += needleSpeed * Time.unscaledDeltaTime;
        angle %= 360f;

        needle.localRotation = Quaternion.Euler(0, 0, -angle);

        if (Input.GetKeyDown(KeyCode.Space))
            Check();
    }

    void StartRound()
    {
        arcSize = Mathf.Max(0.05f, startArcSize - round * arcShrink);
        arcStart = Random.value;

        successArc.fillAmount = arcSize;
       
    }

    void Check()
    {
        float needlePos = angle / 360f;
        bool success = InArc(needlePos);

        if (success)
        {
            round++;
            if (round >= roundsToWin)
                Win();
            else
                StartRound();
        }
        else
        {
            Fail();
        }
    }

    bool InArc(float value)
    {
        float end = arcStart + arcSize;

        if (end <= 1f)
            return value >= arcStart && value <= end;

        return value >= arcStart || value <= end - 1f;
    }

    void Win()
    {
        RaiseSuccess();
        Debug.Log("LOCKPICK SUCCESS");
        Close();
    }

    void Fail()
    {
        onCooldown = true;
        Invoke(nameof(ResetCooldown), failCooldown);
        RaiseFail();
        Debug.Log("LOCKPICK FAILED");
        Close();
    }

    void Close()
    {
        active = false;
       
        canvas.SetActive(false);
        Destroy(gameObject);
    }

    void ResetCooldown()
    {
        onCooldown = false;
    }
}