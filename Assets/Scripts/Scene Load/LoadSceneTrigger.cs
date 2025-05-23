using UnityEngine;

[DisallowMultipleComponent]
public class LoadSceneTrigger : MonoBehaviour
{
    [SerializeField] LoadSceneHandler _loadHandler;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8)
        {
            GetComponent<Collider>().enabled = false;
            _loadHandler.StartLoading();
        }
    }
}
