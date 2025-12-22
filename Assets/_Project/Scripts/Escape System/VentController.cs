using UnityEngine;
using UnityEngine.UI;

public class VentController : MonoBehaviour
{
    [SerializeField] private GameObject otherVent;
    [SerializeField] private GameObject sliderGameObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float ventingSpeed = 1f;
    [SerializeField] private AudioClip escapeSound;
    
    private AudioSource _audioSource;
    private Coroutine _fadeRoutine;
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
        sliderGameObject.SetActive(_isPlayerOnCollider);

        bool isHolding = Input.GetKey(KeyCode.E) && _isPlayerOnCollider;

        if (isHolding)
        {
            HandleEscapeHold();
        }
        else
        {
            HandleEscapeRelease();
        }

        if (_totalTime >= 1f)
        {
            CompleteVenting();
        }
    }
    
    private void HandleEscapeHold()
    {
        _totalTime += Time.deltaTime * ventingSpeed;

        if (_isEscaping) return;
        
        _isEscaping = true;
        StartEscapeAudio();
    }
    
    private void HandleEscapeRelease()
    {
        if (_isEscaping)
        {
            _isEscaping = false;
            StopEscapeAudio();
        }

        _totalTime = 0f;
    }
    
    private void StartEscapeAudio()
    {
        if (_fadeRoutine != null)
        {
            StopCoroutine(_fadeRoutine);
            _fadeRoutine = null;
        }
        
        _audioSource.volume = 1f;

        if (_audioSource.isPlaying) return;
        
        _audioSource.clip = escapeSound;
        _audioSource.loop = true;
        _audioSource.Play();
    }
    
    private void StopEscapeAudio()
    {
        if (_audioSource.isPlaying)
        {
            _fadeRoutine = StartCoroutine(
                GameUtility.FadeOutAndStop(_audioSource, 0.25f)
            );
        }
    }

    private void CompleteVenting()
    {
        StopEscapeAudio();
        _totalTime = 0f;
        PlayerManager.Instance.SetPlayerTransform(otherVent.transform);
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
