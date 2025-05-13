using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }

    public PlayerState CurrentState { get; private set; }
    public float FaceDirection { get; private set; }
    public LayerMask Ground;

    [SerializeField] private GameObject _groundCheck;

    private float _minProcessedValue = 0.1f;
    private IPlayerInput _input;
    private PlayerMovement _movement;
    private PlayerVisual _visual;
    private Rigidbody2D _rb;
    private float _faceLadderDirection;
    private float _horizontalInput;
    private byte _currentJumpCount;
    private float _groundCheckRadius;
    private bool _isJumpRequested;
    private bool _isLadder;
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
        _visual = GetComponentInChildren<PlayerVisual>();
        _groundCheckRadius = _groundCheck.GetComponent<CircleCollider2D>().radius;
        EnableMovement();
    }

    private void Update()
    {
        _horizontalInput = _input.GetHorizontal();
        if (_horizontalInput != 0)
            FaceDirection = Mathf.Sign(_horizontalInput);

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
                _isLadder = false;
                _movement.Jump(IsOnGround(), ref _currentJumpCount);
                Debug.Log(_currentJumpCount);
            }
            if (_isLadder)
                _movement.Climb(_horizontalInput, _faceLadderDirection);
            else
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

    public bool IsOnGround()
    {
        var result = Physics2D.OverlapCircle(_groundCheck.transform.position, _groundCheckRadius, Ground);
        if (result)
            _currentJumpCount = 0;
        return result;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            EnterClimb();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            ExitClimb();
    }

    private void EnterClimb()
    {
        _isLadder = true;
        _visual.EnterClimb(FaceDirection);
        _faceLadderDirection = FaceDirection;
    }

    private void ExitClimb()
    {
        _isLadder = false;
        _movement.ExitClimb();
        _visual.ExitClimb();
    }

    private void EnableMovement() => _canMove = true;

    private void DisableMovement() => _canMove = false;

}
