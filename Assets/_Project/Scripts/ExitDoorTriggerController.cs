using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitDoorTriggerController : MonoBehaviour
{
    [SerializeField] private GameObject sliderGameObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float exitSpeed = 1f;
    [SerializeField] private AudioClip exitSound;
    
    private float _totalTime = 1f;
    private bool _isPlayerOnCollider = false;
    private bool _isExiting = false;

    private AudioSource _audioSource;
    private Coroutine _fadeRoutine;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateSlider();
        sliderGameObject.SetActive(_isPlayerOnCollider && PlayerManager.Instance);

        bool isHolding = Input.GetKey(KeyCode.E) && _isPlayerOnCollider;

        if (isHolding)
        {
            HandleExitHold();
        }
        else
        {
            HandleExitRelease();
        }

        if (_totalTime >= 1f)
        {
            CompleteExit();
        }
    }
    
    private void HandleExitHold()
    {
        _totalTime += Time.deltaTime * exitSpeed;

        if (_isExiting) return;
        
        _isExiting = true;
        StartExitAudio();
    }
    
    
    private void HandleExitRelease()
    {
        if (_isExiting)
        {
            _isExiting = false;
            StopExitAudio();
        }

        _totalTime = 0f;
    }
    
    private void StartExitAudio()
    {
        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
            _fadeRoutine = null;
        }

        _audioSource.volume = 1f;

        if (_audioSource.isPlaying) return;
        
        _audioSource.clip = exitSound;
        _audioSource.Play();
    }
    
    private void StopExitAudio()
    {
        if (_audioSource.isPlaying)
        {
            _fadeRoutine = StartCoroutine(
                GameUtility.FadeOutAndStop(_audioSource, 0.25f)
            );
        }
    }
    
    private void CompleteExit()
    {
        StopExitAudio();
        _totalTime = 0f;

        PlayerManager.Instance.HandleDestroy();
        SceneChangeManager.Instance.LoadNextStage("EndSplashScreenScene");
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        _isPlayerOnCollider = true;
    }
    
    private void OnTriggerStay2D(Collider2D other)
    {
        _isPlayerOnCollider = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _isPlayerOnCollider = false;
    }

    private void UpdateSlider()
    {
        slider.value = _totalTime;
    }
}
