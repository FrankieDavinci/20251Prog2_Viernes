using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Cursor")]
    [SerializeField] CursorLockMode _lockMode = CursorLockMode.Locked;
    [SerializeField] bool _visible = false;

    [Header("Physics")]
    [SerializeField] float _detectionRadius = .1f;
    [SerializeField] float _hitOffset = .25f;

    Ray _camRay;
    RaycastHit _hitInfo;

    [Header("Settings")]
    [SerializeField] float _mouseSensitivity = 500f;
    [SerializeField] float _minDistance = .25f;
    [SerializeField] float _maxDistance = 5;
    [SerializeField] float _minRotation = -45;
    [SerializeField] float _maxRotation = 80;

    bool _isCamBlocked;
    float _mouseY;

    Vector3 _direction;
    Vector3 _camPosition;

    Camera _camera;

    Transform _target;


    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = _visible;

        _camera = GetComponentInChildren<Camera>(true);
    }

    void Start()
    {
        _target = PlayerReference.Instance.player.CameraTarget;

        transform.forward = _target.forward;

        _mouseY = transform.eulerAngles.x;
    }

    private void FixedUpdate()
    {
        _camRay = new Ray(transform.position, -transform.forward);

        _isCamBlocked = Physics.SphereCast(_camRay, _detectionRadius, out _hitInfo, _maxDistance);
    }

    private void LateUpdate()
    {
        var x = Input.GetAxisRaw("Mouse X");
        var y = Input.GetAxisRaw("Mouse Y");

        UpdateCameraRotation(x,y);
        UpdateSpringArm();
    }

    void UpdateCameraRotation(float x, float y)
    {
        transform.position = _target.position;

        if (x == 0 && y == 0) return;

        if (y != 0)
        {
            _mouseY += y * _mouseSensitivity * Time.deltaTime;

            _mouseY = Mathf.Clamp(_mouseY, _minRotation, _maxRotation);
        }

        var mouseX = x * _mouseSensitivity * Time.deltaTime;

        //Rotacion hacia arriba / abajo
        transform.rotation = Quaternion.Euler(-_mouseY, transform.eulerAngles.y, 0);
        
        //Rotacion hacia los costados
        transform.Rotate(Vector3.up, mouseX);
    }

    void UpdateSpringArm()
    {
        _direction = -transform.forward;

        if (_isCamBlocked)
        {
            Vector3 dir = (_hitInfo.point - transform.position) + (_hitInfo.normal * _hitOffset);
            

            if (dir.sqrMagnitude <= Mathf.Pow(_minDistance, 2))
            {
                _camPosition = transform.position + _direction * _minDistance;
            }
            else
            {
                _camPosition = transform.position + dir;
            }
        }
        else
        {
            _camPosition = transform.position + _direction * _maxDistance;
        }

        _camera.transform.position = _camPosition;
        _camera.transform.LookAt(transform.position);
    }
}
