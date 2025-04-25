using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchAnimatorEvents : MonoBehaviour
{
    [SerializeField] BoxCollider _punchCollider;

    private void Awake()
    {
        _punchCollider.enabled = false;
    }

    public void InitPunch()
    {
        _punchCollider.enabled = true;
        Debug.Log("Bla");
    }

    public void EndPunch()
    {
        _punchCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Colision");
    }
}
