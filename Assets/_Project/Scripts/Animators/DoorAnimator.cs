using System;
using System.Collections;
using UnityEngine;

public class DoorAnimator : MonoBehaviour
{
    [SerializeField] private ExitDoorTriggerController exitDoorTriggerController;
    
    private Animator _animator;

    private bool _isOpen;
    private bool _isAnimating;

    private const string DoorOpenState = "DoorOpening";
    private const string DoorCloseState = "DoorClosing";

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!exitDoorTriggerController)
            return;

        // Player STARTED holding E → open door
        if (exitDoorTriggerController.isExiting && !_isOpen)
        {
            TryOpen();
        }
        // Player RELEASED E → close door
        else if (!exitDoorTriggerController.isExiting && _isOpen)
        {
            TryClose();
        }
    }


    private void TryOpen()
    {
        if (_isAnimating) return;

        _isAnimating = true;
        _isOpen = true;
        _animator.Play(DoorOpenState, 0, 0f);
    }

    private void TryClose()
    {
        if (_isAnimating) return;

        _isAnimating = true;
        _isOpen = false;
        _animator.Play(DoorCloseState, 0, 0f);
    }


    public void OnDoorAnimationFinished()
    {
        _isAnimating = false;
    }
}

