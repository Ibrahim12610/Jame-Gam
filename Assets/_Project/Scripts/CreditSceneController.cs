using System.Collections;
using UnityEngine;

public class CreditSceneController : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(HandleSceneChange());
    }

    private IEnumerator HandleSceneChange()
    {
        yield return new WaitForSeconds(5f);
        SceneChangeManager.Instance.LoadNextStage("Main Menu2");
    }
}
