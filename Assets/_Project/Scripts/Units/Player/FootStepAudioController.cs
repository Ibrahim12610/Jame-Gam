using UnityEngine;

public class FootStepAudioController : MonoBehaviour
{
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip crouchClip;

    private AudioSource _audioSource;
    private Rigidbody2D _rb;
    private PlayerMovement _movement;

    private float _stepTimer;

    void Awake()
    {
        _rb = GetComponentInParent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _movement = GetComponentInParent<PlayerMovement>();
        
        _audioSource.loop = true;
        _audioSource.playOnAwake = false;
    }


    void Update()
    {
        bool isMoving = _rb.linearVelocity.sqrMagnitude > 0.01f;

        if (!isMoving || _movement.disableMovement)
        {
            StopFootsteps();
            return;
        }

        var targetClip = _movement.isCrouching ? crouchClip : walkClip;

        if (_audioSource.clip != targetClip)
        {
            _audioSource.clip = targetClip;
            _audioSource.Play();
        }
        else if (!_audioSource.isPlaying)
        {
            _audioSource.Play();
        }
    }

    void StopFootsteps()
    {
        if (_audioSource.isPlaying)
            _audioSource.Stop();
    }
}

