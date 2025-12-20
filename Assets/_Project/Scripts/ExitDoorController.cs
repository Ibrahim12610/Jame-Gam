using System;
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    public GameObject exitDoor;

    private void OnEnable()
    {
        EndGameManager.ActivateEndGameSequence += HandleDoorActivation;
    }

    private void OnDisable()
    {
        EndGameManager.ActivateEndGameSequence -= HandleDoorActivation;
    }

    private void HandleDoorActivation()
    {
        exitDoor.SetActive(true);
    }
}
