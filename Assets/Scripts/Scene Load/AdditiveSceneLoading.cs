using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoading : MonoBehaviour
{
    [SerializeField] string _sceneToLoad = "CaveLevel";

    [SerializeField] GameObject _caveDivisor;

    Collider _collider;

    void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Player>())
        {
            _collider.enabled = false;

            AsyncOperation handler = SceneManager.LoadSceneAsync(_sceneToLoad, LoadSceneMode.Additive);

            
            handler.completed += StartFadeTransition;
        }
    }

    void StartFadeTransition(AsyncOperation operation)
    {
        StartCoroutine(FadeDivisor());
    }

    IEnumerator FadeDivisor()
    {
        var material = _caveDivisor.GetComponent<Renderer>().material;

        var initialColor = material.color;

        var endColor = initialColor;
        endColor.a = 0;

        float ticks = 0;

        while (ticks < 1)
        {
            ticks += Time.deltaTime;

            var newColor = Color.Lerp(initialColor, endColor, ticks);

            material.color = newColor;

            yield return null;
        }
    }


}
