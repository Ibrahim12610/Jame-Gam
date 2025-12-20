using System;
using System.Collections;
using UnityEngine;

public class EndGameSceneController : MonoBehaviour
{
    private void Awake()
    {
        StartCoroutine(HandleSceneChange());
    }

    private IEnumerator HandleSceneChange()
    {
        yield return new WaitForSeconds(3f);
        SceneChangeManager.Instance.LoadNextStage("CreditScene");
    }
}
