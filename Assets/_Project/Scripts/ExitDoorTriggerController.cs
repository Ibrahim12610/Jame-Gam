using System;
using UnityEngine;
using UnityEngine.UI;

public class ExitDoorTriggerController : MonoBehaviour
{
    [SerializeField] private GameObject sliderGameObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float exitSpeed = 1f;
    [SerializeField] private AudioClip exitSound;
    
    private float _totalTime = 0f;
    private bool _isPlayerOnCollider = false;
    private AudioSource _audioSource;
    private bool _isExiting= false;

    private void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        UpdateSlider();

        if (_isPlayerOnCollider && PlayerManager.Instance)
        {
            sliderGameObject.SetActive(true);
        }
        else
        {
            sliderGameObject.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E) && _isPlayerOnCollider)
        {
            _totalTime += Time.deltaTime * exitSpeed;
            
            if (!_isExiting)
            {
                _audioSource.PlayOneShot(exitSound);
            }
            _isExiting = true;
        }
        else
        {
            _isExiting = false;
            _audioSource.Stop();
            _totalTime = 0f;
        }

        if (_totalTime > 1f)
        {
            PlayerManager.Instance.HandleDestroy();
            SceneChangeManager.Instance.LoadNextStage("EndSplashScreenScene");
        }
        
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
