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
    }

    private void Start()
    {
        var punchEvents = _animator.GetBehaviour<PunchEventsBehavior>();

        if (punchEvents)
        {
            punchEvents.SetCollider(_punchCollider);
        }
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

        _animator.SetFloat(_xAxiId, _direction.x);
        _animator.SetFloat(_zAxiId, _direction.z);
    }


    private void FixedUpdate()
    {
        Movement();

        if (_jumpPressed)
        {
            Jump();
            _jumpPressed = false;
        }
    }

    void Jump()
    {
        _rb.AddForce(Vector3.up * _jumpForce, ForceMode.VelocityChange);
        _isGrounded = false;
    }

    void Movement()
    {
        if (_direction.sqrMagnitude > 1)
        {
            _direction.Normalize();
        }

        //transform.position += _direction * (_speed * Time.fixedDeltaTime);

        //_rb.velocity = 
        //_rb.AddForce(_direction * _speed, ForceMode.Acceleration);
        _rb.MovePosition(transform.position + _direction * (_speed * Time.fixedDeltaTime));
    }

    private void OnCollisionEnter(Collision collision)
    {
        _isGrounded = true;
    }
    
}
