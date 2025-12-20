using System;
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        //hold e to continue seqence
        //send to main menu
        SceneChangeManager.Instance.LoadNextStage("Main Menu");
    }
}
