using System;
using UnityEngine;

public class ExitDoorTriggerController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D otherCollider)
    {
        PlayerManager.Instance.HandleDestroy();
        SceneChangeManager.Instance.LoadNextStage("Main Menu");
    }
    
    //Exit Sequence:
    //Need to Hold E to exit -> after complete will destroy player + Start scene change
}
