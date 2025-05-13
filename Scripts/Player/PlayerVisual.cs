using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    private PlayerController _playerController;
    private Animator _animator;

    private void Awake()
    {
        _playerController = GetComponentInParent<PlayerController>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        FlipFace();
        _animator.SetBool("IsRunning", _playerController.CurrentState == PlayerState.Running);
        _animator.SetBool("IsJumping", _playerController.CurrentState == PlayerState.Jumping);
        _animator.SetBool("IsFalling", _playerController.CurrentState == PlayerState.Falling);
        _animator.SetBool("IsOnGround", _playerController.IsOnGround());
    }

    public void EnterClimb(float faceDirection)
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 90f * faceDirection);
    }

    public void ExitClimb()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    private void FlipFace()
    {
        if (_playerController.FaceDirection != 0)
            transform.localScale = new Vector3(_playerController.FaceDirection, 1, 1);
    }
}
