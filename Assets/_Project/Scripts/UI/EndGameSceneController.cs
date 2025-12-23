using System;
using System.Collections;
using UnityEngine;

public class EndGameSceneController : MonoBehaviour
{
    [SerializeField] private float duration;
    
    private void Awake()
    {
        StartCoroutine(HandleSceneChange());
    }

    private IEnumerator HandleSceneChange()
    {
        yield return new WaitForSeconds(duration);
        SceneChangeManager.Instance.LoadNextStage("Main Menu2");
    }
}
