using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLadderClimber : MonoBehaviour
{
    private PlayerVisual _visual;
    private PlayerMovement _movement;
    private bool _isOnLadder;
    private float _ladderDirection;

    public bool IsOnLadder => _isOnLadder;

    public void Init(PlayerMovement movement, PlayerVisual visual)
    {
        _movement = movement;
        _visual = visual;
    }

    public void EnterLadder(float inputDirection)
    {
        if (inputDirection == 0)
            return;
        _isOnLadder = true;
        _ladderDirection = Mathf.Sign(inputDirection);
        _visual.EnterClimb(_ladderDirection);
    }

    public void ExitLadder()
    {
        _isOnLadder = false;
        _movement.ExitClimb();
        _visual.ExitClimb();
    }

    public void Climb(float input)
    {
        _movement.Climb(input, _ladderDirection);
    }
}
