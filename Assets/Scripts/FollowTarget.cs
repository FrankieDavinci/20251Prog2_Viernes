using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Transform _targetTransform;

    Vector3 _offset;

    public void SetTarget(Transform target)
    {
        _targetTransform = target;

        _offset = transform.position - _targetTransform.position;
    }

    private void LateUpdate()
    {
        if (!_targetTransform) return;

        transform.position = _offset + _targetTransform.position;
    }
}
