using UnityEngine;
using UnityEngine.UI;

public class ExitDoorSliderController : MonoBehaviour
{
    [SerializeField] private GameObject sliderGameObject;
    [SerializeField] private Slider slider;
    [SerializeField] private float exitSpeed = 1f;
    
    private float _totalTime = 0f;

    private bool _isPlayerOnCollider = false;

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
            _totalTime += Time.deltaTime * exitSpeed;
        }
        else
        {
            _totalTime = 0f;
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
