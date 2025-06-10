using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    public float FaceDirection { get; private set; }
    public float FaceLadderDirection { get; private set; }

    private PlayerController _playerController;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void Init(PlayerController controller)
    {
        _playerController = controller;
    }

    private void Update()
    {
        _animator.SetBool("IsRunning", _playerController.CurrentState == PlayerState.Running);
        _animator.SetBool("IsJumping", _playerController.CurrentState == PlayerState.Jumping);
        _animator.SetBool("IsFalling", _playerController.CurrentState == PlayerState.Falling);
        _animator.SetBool("IsOnGround", _playerController.IsOnGround());
    }

    public void EnterClimb(float faceDirection)
    {
        FaceLadderDirection = faceDirection;
        transform.localRotation = Quaternion.Euler(0f, 0f, 90f * faceDirection);
    }

    public void ExitClimb()
    {
        transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
    }

    public void FlipFace(float horizontalInput)
    {
        if (horizontalInput != 0)
        {
            float newDirection = Mathf.Sign(horizontalInput);
            if (FaceDirection == newDirection) return;
            FaceDirection = newDirection;
            transform.localScale = new Vector3(FaceDirection, 1, 1);
        }
    }
}
