using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public int CurrentJumpCount { get; private set; }

    [SerializeField] private byte _maxJumpCount;
    [SerializeField] private float _moveSpeed; // 25
    [SerializeField] private float _jumpForce; // 30
    private Rigidbody2D _rb;
    private float _originalGravityScale;
    private float _yVelReleasedMod = 5f;

    private void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _originalGravityScale = _rb.gravityScale;
    }

    public void Move(float horizontalInput)
    {
        _rb.velocity = new Vector2(horizontalInput * _moveSpeed, _rb.velocity.y);
    }

    public void Jump(bool isOnGround)
    {
        if (isOnGround)
        {
            ResetJumpCount();
            SFX.Instance.PlayPlayerJump(CurrentJumpCount);
            MakeJump();
        }
        else if (CurrentJumpCount < _maxJumpCount)
        {
            CurrentJumpCount++;
            SFX.Instance.PlayPlayerJump(CurrentJumpCount);
            MakeJump();
        }
    }

    public void Climb(float horizontalInput, float faceDirection)
    {
        _rb.gravityScale = 0;
        _rb.velocity = new Vector2(_rb.velocity.x, horizontalInput * _moveSpeed * faceDirection);
    }

    public void ExitClimb()
    {
        _rb.gravityScale = _originalGravityScale;
    }

    public void HandleJumpRelease()
    {
        if (_rb.velocity.y > 0)
            _rb.velocity = new Vector2(_rb.velocity.x, _rb.velocity.y / _yVelReleasedMod);
    }

    private void MakeJump()
    {
        _rb.velocity = new Vector2(_rb.velocity.x, _jumpForce);
    }

    public void ResetJumpCount() => CurrentJumpCount = 0;
}