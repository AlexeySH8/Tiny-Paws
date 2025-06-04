using UnityEngine;

public class MobileInput : MonoBehaviour, IPlayerInput
{
    private bool _leftHeld;
    private bool _rightHeld;
    private bool _jumpPressed;
    private bool _jumpReleased;

    public void OnLeftDown() => _leftHeld = true;
    public void OnLeftUp() => _leftHeld = false;

    public void OnRightDown() => _rightHeld = true;
    public void OnRightUp() => _rightHeld = false;

    public void OnJumpDown() => _jumpPressed = true;
    public void OnJumpUp() => _jumpReleased = true;

    public float GetHorizontal()
    {
        if (_leftHeld) return -1f;
        if (_rightHeld) return 1f;
        return 0f;
    }

    public bool IsJumpInput()
    {
        bool result = _jumpPressed;
        _jumpPressed = false;
        return result;
    }

    public bool IsJumpInputReleased()
    {
        bool result = _jumpReleased;
        _jumpReleased = false;
        return result;
    }
}
