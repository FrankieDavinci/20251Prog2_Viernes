using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[DisallowMultipleComponent]
[RequireComponent(typeof(Canvas))]
public class LoadSceneHandler : MonoBehaviour
{
    Canvas _loadCanvas;
    
    [SerializeField] string _sceneName;
    [SerializeField] Image _loadingBar;

    private void Awake()
    {
        _loadCanvas = GetComponent<Canvas>();
        _loadCanvas.enabled = false;
    }

    public void StartLoading()
    {
        _loadCanvas.enabled = true;
        _loadingBar.fillAmount = 0;
        StartCoroutine(StartLoadingScene());
    }

    IEnumerator StartLoadingScene()
    {
        AsyncOperation handler = SceneManager.LoadSceneAsync(_sceneName);

        handler.allowSceneActivation = false;

        //Time.timeScale = 0;

        //Mientras la nueva escena no se haya terminado de cargar
        while (handler.progress < 0.9f)
        {
            _loadingBar.fillAmount = handler.progress;
            yield return null;
        }


        _loadingBar.fillAmount = 1;

        yield return new WaitForSeconds(2);

        handler.allowSceneActivation = true;
    }
}
