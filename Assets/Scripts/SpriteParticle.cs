using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteParticle : MonoBehaviour {
    public float lifetime = 0.5f;
    private SpriteRenderer spriteRenderer;
    private SpritePool spritePool;
    private float _lifetime = 0.5f;
    private readonly Color transparent = new Color(255,255,255,0);
    public bool animateAlpha;

    // Awake는 스크립트 인스턴스가 로드되는 중에 호출됩니다.
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spritePool = GetComponent<SpritePool>();
    }

    private void OnEnable()
    {
        _lifetime = lifetime;
        if (animateAlpha)
            spriteRenderer.color = Color.white;
    }

    private void Update()
    {
        if (animateAlpha)
            spriteRenderer.color = Color.Lerp(Color.white, transparent, _lifetime / lifetime);
        if (_lifetime <= 0)
            spritePool.DestroyPool();
        else 
            _lifetime -= Time.deltaTime;
    }
}
