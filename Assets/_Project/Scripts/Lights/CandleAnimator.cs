using UnityEngine;
using UnityEngine.UI;
public class CandleAnimator : MonoBehaviour
{
    [SerializeField] private Sprite[] sprites;
    private int currentSpriteIndex = 0;
    [SerializeField] private float timeBetweenAnimations = 0.4f;
    private float _timer = 0f;

    private SpriteRenderer _currSprite;

    // Update is called once per frame

    void Awake()
    {
        _currSprite = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= timeBetweenAnimations)
        {
            switchSprite();
            _timer = 0;
        }
    }

    void switchSprite()
    {
        currentSpriteIndex++;
        if (currentSpriteIndex > sprites.Length - 1)
        {
            currentSpriteIndex = 0;
        }
        _currSprite.sprite = sprites[currentSpriteIndex];
    }
}
