using System;
using UnityEngine;
using UnityEngine.UI;

public class SliderInstructionController : MonoBehaviour
{
    [SerializeField] private Sprite keyNormal;
    [SerializeField] private Sprite keyPressed;
    
    
    private Image _image;

    private void Awake()
    {
        _image = GetComponent<Image>();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.E))
        {
            _image.sprite = keyPressed;
        }
        else
        {
            _image.sprite = keyNormal;
        }
    }
}
