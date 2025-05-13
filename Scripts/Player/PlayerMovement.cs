using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private byte _maxJumpCount;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _jumpForce;
    private Rigidbody2D _rb;
    private float _yVelReleasedMod = 5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Move(float horizontalInput)
    {
        _rb.velocity = new Vector2(horizontalInput * _moveSpeed, _rb.velocity.y);
    }

    public void Jump(bool isOnGround, ref byte currentJumpCount)
    {
        if (isOnGround)
        {
            currentJumpCount = 0;
            MakeJump();
        }
        else if (currentJumpCount < _maxJumpCount)
        {
            MakeJump();
            currentJumpCount++;
        }
    }

    public void Climb(float horizontalInput, float faceDirection)
    {
        _rb.gravityScale = 0;
        _rb.velocity = new Vector2(_rb.velocity.x, horizontalInput * _moveSpeed * faceDirection);
    }

    public void ExitClimb()
    {
        _rb.gravityScale = 4;
    }

    public void HandleJumpRelease()
    {
        if (_rb.velocity.y > 0)
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y / _yVelReleasedMod);
    }

    private void MakeJump() => _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);

}
