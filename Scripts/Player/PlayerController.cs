using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public bool IsOnGround { get; private set; }
    public LayerMask Ground;

    [SerializeField] private Transform _groundCheck;
    [SerializeField] private float _checkRadius = 0.5f;
    private IPlayerInput _input;
    private PlayerMovement _movement;
    private float _horizontalInput;
    private bool _isJumpRequested;
    private bool _canMove;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        // var mobileInput = FindObjectOfType<MobileInput>();

#if UNITY_ANDROID || UNITY_IOS
    _input = mobileInput;
#else
        // mobileInput?.gameObject.SetActive(false);
        _input = new PCInput();
#endif      
        _movement = GetComponent<PlayerMovement>();
        EnableMovement();
    }

    private void Update()
    {
        CheckingGround();
        _horizontalInput = _input.GetHorizontal();
        if (_input.IsJumpInput())
            _isJumpRequested = true;
        if (_input.IsJumpInputReleased())
            _movement.HandleJumpRelease();
    }

    void FixedUpdate()
    {
        if (_canMove)
        {
            if (_isJumpRequested)
            {
                _isJumpRequested = false;
                _movement.Jump(IsOnGround);
            }
            _movement.Move(_horizontalInput);
        }
    }

    private void CheckingGround() =>
        IsOnGround = Physics2D.OverlapCircle(_groundCheck.position, _checkRadius, Ground);

    private void EnableMovement() => _canMove = true;

    private void DisableMovement() => _canMove = false;
}
