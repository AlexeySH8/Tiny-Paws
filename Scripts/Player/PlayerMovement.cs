using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private byte _maxJumpCount;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    private float _yVelReleasedMod = 2f;
    private byte _currentJumpCount;
    private Rigidbody2D _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalInput)
    {
        _rb.velocity = new Vector2(horizontalInput * _moveSpeed, _rb.velocity.y);
        if (horizontalInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(horizontalInput), 1, 1);
    }  

    public void Jump(bool isOnGround)
    {
        if (isOnGround)
        {
            _currentJumpCount = 0;
            MakeJump();
        }
        else if (_currentJumpCount < _maxJumpCount)
        {
            MakeJump();
            _currentJumpCount++;
        }
    }

    public void HandleJumpRelease()
    {
        if (_rb.velocity.y > 0)
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y / _yVelReleasedMod);
    }

    private void MakeJump() => _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
}
