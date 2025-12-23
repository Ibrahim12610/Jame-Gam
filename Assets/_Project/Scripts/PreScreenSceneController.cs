using System.Collections;
using UnityEngine;

public class PreScreenSceneController : MonoBehaviour
{
    [SerializeField] private float timeToWait;
    private void Awake()
    {
        StartCoroutine(HandleSceneChange());
    }

    private IEnumerator HandleSceneChange()
    {
        yield return new WaitForSeconds(timeToWait);
        SceneChangeManager.Instance.LoadNextStage("FinalizedMap2");
    }
}
