using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingPlatform : MonoBehaviour
{
    [SerializeField] private float _fadeTime = .3f;
    [SerializeField] private float _interval = 5;
    [SerializeField] private float _spawnTime = 3;

    private bool _isActive;

    private Collider _col;
    private Material _material;

    private void Start()
    {
        _col = GetComponent<Collider>();
        _material = GetComponent<Renderer>().material;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out Player player) && !_isActive)
        {
            //CORRUTINA - Coroutine
            StartCoroutine(FadeCoroutine());
        }
    }

    IEnumerator FadeCoroutine()
    {
        _isActive = true;

        float timer = 0;

        var currentColor = _material.color;
        
        while (timer < 1)
        {
            timer += Time.deltaTime * _fadeTime;

            currentColor.a = Mathf.Lerp(1, 0, timer);

            _material.color = currentColor;
            
            yield return null;
        }

        currentColor.a = 0;
        _material.color = currentColor;
        _col.enabled = false;
        
        NavMeshManager.UpdateSurface();
        
        yield return new WaitForSeconds(_interval);

        timer = 0;
        
        while (timer < 1)
        {
            timer += Time.deltaTime * _fadeTime;

            currentColor.a = Mathf.Lerp(0, 1, timer);

            _material.color = currentColor;
            
            yield return null;
        }
        
        currentColor.a = 1;
        _material.color = currentColor;
        _col.enabled = true;

        _isActive = false;
        
        NavMeshManager.UpdateSurface();
    }
}
