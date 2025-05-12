using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public PlayerState CurrentState { get; private set; }
    public LayerMask Ground;

    [SerializeField] private GameObject _groundCheck;

    private float _minProcessedValue = 0.1f;
    private IPlayerInput _input;
    private PlayerMovement _movement;
    private Rigidbody2D _rb;
    private float _groundCheckRadius;
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
        #region Input
        // var mobileInput = FindObjectOfType<MobileInput>();

#if UNITY_ANDROID || UNITY_IOS
    _input = mobileInput;
#else
        // mobileInput?.gameObject.SetActive(false);
        _input = new PCInput();
#endif
        #endregion
        _rb = GetComponent<Rigidbody2D>();
        _movement = GetComponent<PlayerMovement>();
        _groundCheckRadius = _groundCheck.GetComponent<CircleCollider2D>().radius;
        EnableMovement();
    }

    private void Update()
    {
        _horizontalInput = _input.GetHorizontal();
        Flip();

        if (_input.IsJumpInput())
            _isJumpRequested = true;

        if (_input.IsJumpInputReleased())
            _movement.HandleJumpRelease();

        UpdateState();
    }

    private void FixedUpdate()
    {
        if (_canMove)
        {
            if (_isJumpRequested)
            {
                _isJumpRequested = false;
                _movement.Jump(IsOnGround());
            }
            _movement.Move(_horizontalInput);
        }
    }

    private void UpdateState()
    {
        if (!IsOnGround())
        {
            if (_rb.velocity.y > _minProcessedValue)
                CurrentState = PlayerState.Jumping;
            else if (_rb.velocity.y < -_minProcessedValue)
                CurrentState = PlayerState.Falling;
        }
        else
        {
            if (Mathf.Abs(_horizontalInput) > _minProcessedValue)
                CurrentState = PlayerState.Running;
            else
                CurrentState = PlayerState.Idle;
        }
    }

    private void Flip()
    {
        if (_horizontalInput != 0)
            transform.localScale = new Vector3(Mathf.Sign(_horizontalInput), 1, 1);
    }

    public bool IsOnGround() => Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, Ground);

    private void EnableMovement() => _canMove = true;

    private void DisableMovement() => _canMove = false;
}
