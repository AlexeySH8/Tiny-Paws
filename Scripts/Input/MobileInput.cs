using UnityEngine;
using UnityEngine.UI;

public class MobileInput : MonoBehaviour, IPlayerInput
{
    private float _sensivity = 3f;
    private float _horizontalInput;
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
        if (_rightHeld)
            _horizontalInput += _sensivity * Time.deltaTime;
        else if (_leftHeld)
            _horizontalInput -= _sensivity * Time.deltaTime;
        else
            _horizontalInput = Mathf.MoveTowards(_horizontalInput, 0, _sensivity * Time.deltaTime);

        _horizontalInput = Mathf.Clamp(_horizontalInput, -1f, 1f);
        return _horizontalInput;
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
