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
    [Range(0.05f, 0.9f)]
    public float startArcSize = 0.6f;
    public float arcShrink = 0.1f;
    public int roundsToWin = 3;
    public float failCooldown = 1f;

    [Header("Feel (Optional)")]
    [Tooltip("Extra forgiveness added to success arc (normalized 0–1)")]
    public float forgiveness = 0.02f;

    float needleAngle;   // 0–360 (0 = top)
    int round;
    float angle;

    public static bool onCooldown;

    void OnEnable()
    {
        canvas.SetActive(true);

        round = 0;
        angle = 0f;

        needle.pivot = new Vector2(0.5f, 0f);

        StartRound();
    }

    void Update()
    {
        //if (!active) return;

        needleAngle = (needleAngle + needleSpeed * Time.unscaledDeltaTime) % 360f;
        needle.localRotation = Quaternion.Euler(0f, 0f, -needleAngle);

        if (Input.GetKeyDown(KeyCode.Space))
            Check();
    }

    void StartRound()
    {
        float arcSize = Mathf.Max(0.05f, startArcSize - round * arcShrink);

        // Unity radial fill: success is UNFILLED portion
        successArc.type = Image.Type.Filled;
        successArc.fillMethod = Image.FillMethod.Radial360;
        successArc.fillClockwise = true;

        // 0 = Top, 1 = Right, 2 = Bottom, 3 = Left
        successArc.fillOrigin = Random.Range(0, 4);

        successArc.fillAmount = 1f - arcSize;
        successArc.transform.localRotation = Quaternion.identity;

        Debug.Log(
            $"Round {round + 1} | " +
            $"Origin {(successArc.fillOrigin * 90f):F0}° | " +
            $"Success Size {(arcSize * 360f):F0}°"
        );
    }

    void Check()
    {
            // Normalize needle angle
            // Needle rotates CLOCKWISE, so invert
        float needleNorm = 1f - ((needleAngle % 360f) / 360f);

        // Radial360 has 4 origins: 0=Top,1=Right,2=Bottom,3=Left
        float originNorm = successArc.fillOrigin / 4f;

        // Convert needle into fill-origin space
        float relative = needleNorm - originNorm;
        if (relative < 0f) relative += 1f;

        // Filled = fail, Unfilled = success
        bool success = relative > (successArc.fillAmount - forgiveness);

        Debug.Log(
            $"Needle {needleAngle:F1}° | " +
            $"NeedleNorm {needleNorm:F3} | " +
            $"Origin {originNorm:F3} | " +
            $"Relative {relative:F3} | " +
            $"FillAmount {successArc.fillAmount:F3} | SUCCESS {success}"
        );

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

    void Win()
    {
        RaiseSuccess();
        Debug.Log("LOCKPICK SUCCESS");
        Close();
    }

    void Fail()
    {
        Debug.Log("LOCKPICK FAILED");
        onCooldown = true;
        Invoke(nameof(ResetCooldown), failCooldown);
        RaiseFail();
        Debug.Log("LOCKPICK FAILED");
        Close();
    }

    void Close()
    {
       
        canvas.SetActive(false);
        Destroy(gameObject);
    }

    void ResetCooldown()
    {
        onCooldown = false;
    }
}