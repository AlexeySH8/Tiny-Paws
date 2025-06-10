using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateResolver : MonoBehaviour
{
    private const float MinVelocityThreshold = 0.2f;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public PlayerState GetCurrentState(bool isOnGround, bool isLadder, float horizontalInput)
    {
        PlayerState state = PlayerState.Idle;
        if (isOnGround)
        {
            if (Mathf.Abs(horizontalInput) > MinVelocityThreshold || isLadder)
                state = PlayerState.Running;
            else
                state = PlayerState.Idle;
        }
        else
        {
            if (_rb.velocity.y > MinVelocityThreshold)
                state = PlayerState.Jumping;
            else if (_rb.velocity.y < -MinVelocityThreshold)
                state = PlayerState.Falling;
        }
        return state;
    }
}
