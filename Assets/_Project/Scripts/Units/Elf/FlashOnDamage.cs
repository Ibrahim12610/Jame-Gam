using System.Collections;
using UnityEngine;

public class FlashOnDamage : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private Color flashColor = Color.white;

    Material _material;
    static readonly int FlashAmount = Shader.PropertyToID("_FlashAmount");
    static readonly int FlashColor = Shader.PropertyToID("_FlashColor");

    private void Awake()
    {
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _material = _spriteRenderer.material;

        _material.SetFloat(FlashAmount, 0);
        _material.SetColor(FlashColor, flashColor);
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine());
    }

    private IEnumerator FlashRoutine()
    {
        _material.SetFloat(FlashAmount, 1);
        yield return new WaitForSeconds(flashDuration);
        _material.SetFloat(FlashAmount, 0);
    }
}
