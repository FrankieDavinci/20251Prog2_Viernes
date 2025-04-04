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

    [Header("Keys")]
    [SerializeField] KeyCode _jumpKey = KeyCode.Space;

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
    }

    void Update()
    {
        if (Input.GetKeyDown(_jumpKey) && _isGrounded)
        {
            _jumpPressed = true;
        }

        _direction.x = Input.GetAxis("Horizontal");
        _direction.z = Input.GetAxis("Vertical");

        _animator.SetFloat("xAxi", _direction.x);
        _animator.SetFloat("zAxi", _direction.z);
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
