using UnityEngine;

public class PCInput : IPlayerInput
{
    public float GetHorizontal() => Input.GetAxis("Horizontal");
    public bool IsJumpInput() => Input.GetButtonDown("Jump");
    public bool IsJumpInputReleased() => Input.GetButtonUp("Jump");
}