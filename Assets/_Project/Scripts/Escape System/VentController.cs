using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class VentController : MonoBehaviour
{
    [SerializeField] private GameObject otherVent;
    [SerializeField] private GameObject sliderGameObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float ventingSpeed = 1f;
    [SerializeField] private AudioClip escapeSound;
    
    private AudioSource _audioSource;
    private float _totalTime = 0f;
    private bool _isPlayerOnCollider = false;
    private bool _isEscaping = false;
    
    private void Awake()
    {
        _audioSource =  GetComponent<AudioSource>();
    }
    
    private void Update()
    {
        UpdateSlider();

        if (_isPlayerOnCollider)
        {
            sliderGameObject.SetActive(true);
        }
        else
        {
            sliderGameObject.SetActive(false);
        }

        if (Input.GetKey(KeyCode.E) && _isPlayerOnCollider)
        {
            _totalTime += Time.deltaTime * ventingSpeed;
            if (!_isEscaping)
            {
                _audioSource.PlayOneShot(escapeSound);
            }
            _isEscaping = true;
        }
        else
        {
            _isEscaping = false;
            _audioSource.Stop();
            _totalTime = 0f;
        }

        if (_totalTime >= 1f)
        {
            PlayerManager.Instance.SetPlayerTransform(otherVent.transform);
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
