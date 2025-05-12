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
        _animator.SetBool("IsRunning", _playerController.CurrentState == PlayerState.Running);
        _animator.SetBool("IsJumping", _playerController.CurrentState == PlayerState.Jumping);
        _animator.SetBool("IsFalling", _playerController.CurrentState == PlayerState.Falling);
        _animator.SetBool("IsOnGround", _playerController.IsOnGround());
    }
}
