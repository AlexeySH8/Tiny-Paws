using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    public LayerMask Ground;
    [SerializeField] private float _fallDamageThreshold;
    [SerializeField] private GameObject _groundCheck;

    private PlayerHealth _health;
    private PlayerMovement _movement;
    private float _groundCheckRadius;
    private float _yPreviousPos;

    private void Awake()
    {
        _health = GetComponent<PlayerHealth>();
        _groundCheckRadius = _groundCheck.GetComponent<CircleCollider2D>().radius;
        _yPreviousPos = transform.position.y;
    }

    public void Init(PlayerMovement movement)
    {
        _movement = movement;
    }

    public bool CheckGrounded()
    {
        var result = Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, Ground);
        if (result)
        {
            var height = _yPreviousPos - transform.position.y;
            if (height > _fallDamageThreshold)
                _health.FallDamage((int)Mathf.Floor(height / _fallDamageThreshold));
            _movement.ResetJumpCount();
            _yPreviousPos = transform.position.y;
        }
        return result;
    }
}
