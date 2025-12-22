using System;
using UnityEngine;

public class ExitDoorController : MonoBehaviour
{
    public GameObject exitDoor;
    public GameObject waypoint;

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
        waypoint.SetActive(true);
    }
}
