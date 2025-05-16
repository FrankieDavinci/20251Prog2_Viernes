using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    Rigidbody _rb;
    [SerializeField] Animator _animator;

    [Header("Stats")]
    [SerializeField] float _speed;
    [SerializeField] float _jumpForce;
    [SerializeField] Transform _cameraTarget;
    public Transform CameraTarget { get { return _cameraTarget; } }

    [Header("Animation Parameters")]
    [SerializeField] string _isMovingParameter;
    [SerializeField] string _punchParameter;
    [SerializeField] string _xAxiParameter;
    [SerializeField] string _zAxiParameter;
    int _isMovingId;
    int _punchId;
    int _xAxiId;
    int _zAxiId;

    [Header("Keys")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;
    [SerializeField] KeyCode _punchKey = KeyCode.Mouse0;

    [Header("Punch")]
    [SerializeField] BoxCollider _punchCollider;

    [Header("Ground Detection")]
    [SerializeField] float _jumpRayDistance = .2f;
    [SerializeField] LayerMask _jumpMask;
    
    [Header("Wall Detection")]
    [SerializeField] float _wallRayDistance = .75f;
    [SerializeField] LayerMask _wallMask;

    Transform _camTransform;

    Vector3 _direction;

    bool _jumpPressed;

    bool _isGrounded = false;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.constraints = RigidbodyConstraints.FreezeRotation;

        //ctrl + K + C y ctrl + K + U
        //_rb.constraints = RigidbodyConstraints.FreezeRotationY | 
        //                    RigidbodyConstraints.FreezeRotationX;
        _direction = Vector3.zero;

        _isMovingId = Animator.StringToHash(_isMovingParameter);
        _punchId = Animator.StringToHash(_punchParameter);
        _xAxiId = Animator.StringToHash(_xAxiParameter);
        _zAxiId = Animator.StringToHash(_zAxiParameter);

        // PlayerReference.player = this;
    }

    private void OnEnable()
    {
        var camera = Camera.main;

        if (camera != null)
        {
            if (camera.TryGetComponent(out FollowTarget followTarget))
            {
                followTarget.SetTarget(transform);
            }
        }

        PlayerReference.Instance.player = this;
        
    }

    private void Start()
    {
        var punchEvents = _animator.GetBehaviour<PunchEventsBehavior>();

        if (punchEvents)
        {
            punchEvents.SetCollider(_punchCollider);
        }

        _camTransform = Camera.main.transform;
    }

    void Update()
    {
        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            _jumpPressed = true;
        }

        if (Input.GetKeyDown(_punchKey) && _isGrounded)
        {
            _animator.SetTrigger(_punchId);
        }

        var isMoving = _direction.x != 0 || _direction.z != 0;

        _animator.SetBool(_isMovingId, isMoving);

        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

    }


    private void FixedUpdate()
    {
        if (IsDirectionBlocked(_direction))
        {
            _direction = Vector3.zero;
            RotateBasedOnCamera();
        }
        else
        {
            Movement();
        }

        _animator.SetFloat(_xAxiId, _direction.x);
        _animator.SetFloat(_zAxiId, _direction.z);

        if (_jumpPressed)
        {
            Jump();
            _jumpPressed = false;
        }

        _isGrounded = GroundDetection();
        _animator.SetBool("Grounded", _isGrounded);
    }

    bool IsDirectionBlocked(Vector3 direction)
    {
        var rayDir = (transform.right * direction.x + transform.forward * direction.z);
        var wallRay = new Ray(transform.position, rayDir);

        Debug.DrawLine(transform.position, transform.position + rayDir * 2, Color.red, 1);

        return Physics.Raycast(wallRay, _wallRayDistance, _wallMask);
    }

    bool GroundDetection()
    {
        var jumpOffset = transform.position + Vector3.up * _jumpRayDistance/2;

        var jumpRay = new Ray(jumpOffset, -Vector3.up);

        //Debug.DrawLine(jumpOffset, jumpOffset - Vector3.up * _jumpRayDistance, Color.red, 1);

        return Physics.SphereCast(jumpRay, 0.19f , _jumpRayDistance, _jumpMask);
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
    }

    void Movement()
    {
        var camForward = _camTransform.forward;
        var camRight = _camTransform.right;

        camForward.y = 0;
        camRight.y = 0;

        UpdateForward(camForward);

        var direction = (camRight * _direction.x + camForward * _direction.z);

        //if (_direction.sqrMagnitude > 1)
        //{
        //    _direction.Normalize();
        //}

        //transform.position += _direction * (_speed * Time.fixedDeltaTime);

        //_rb.velocity = 
        //_rb.AddForce(_direction * _speed, ForceMode.Acceleration);
        _rb.MovePosition(transform.position + Vector3.ClampMagnitude(direction, 1) * (_speed * Time.fixedDeltaTime));
    }

    void UpdateForward(Vector3 direction)
    {
        transform.forward = direction;
    }

    void RotateBasedOnCamera()
    {
        var camForward = _camTransform.forward;
        camForward.y = 0;

        UpdateForward(camForward);
    }

    // private void OnDestroy()
    // {
    //     PlayerReference.player = null;
    // }
}
