using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] MobileInput _mobileInput;

    public PlayerState CurrentState { get; private set; }

    private IPlayerInput _input;
    private PlayerVisual _visual;
    private PlayerMovement _movement;
    private PlayerGroundChecker _groundChecker;
    private PlayerStateResolver _stateResolver;
    private PlayerLadderClimber _ladderClimber;
    private Coroutine _disableMovementCoroutine;
    private float _horizontalInput;
    private bool _isJumpRequested;
    private bool _canMove;

    private void Awake()
    {
        if (Application.isMobilePlatform)
            _input = _mobileInput;
        else
            _input = new PCInput();

        _movement = GetComponent<PlayerMovement>();
        _groundChecker = GetComponent<PlayerGroundChecker>();
        _stateResolver = GetComponent<PlayerStateResolver>();
        _visual = GetComponentInChildren<PlayerVisual>();
        _ladderClimber = GetComponent<PlayerLadderClimber>();

        _ladderClimber.Init(_movement, _visual);
        _visual.Init(this);
        _groundChecker.Init(_movement);
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += EnableMovement;
        GameManager.Instance.OnGameOver += DisableMovement;
        GameManager.Instance.OnGameFinish += DisableMovement;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= EnableMovement;
        GameManager.Instance.OnGameOver -= DisableMovement;
        GameManager.Instance.OnGameFinish -= DisableMovement;
    }

    private void Update()
    {
        if (_canMove)
        {
            _horizontalInput = _input.GetHorizontal();
            _visual.FlipFace(_horizontalInput);

            if (_input.IsJumpInput())
                _isJumpRequested = true;

            if (_input.IsJumpInputReleased())
                _movement.HandleJumpRelease();
        }
        CurrentState = _stateResolver.GetCurrentState(IsOnGround(), 
            _ladderClimber.IsOnLadder, _horizontalInput);
    }

    private void FixedUpdate()
    {
        if (!_canMove) return;

        if (_isJumpRequested)
        {
            _isJumpRequested = false;
            _ladderClimber.ExitLadder();
            _movement.Jump(IsOnGround());
        }
        if (_ladderClimber.IsOnLadder)
            _ladderClimber.Climb(_horizontalInput);
        else
            _movement.Move(_horizontalInput);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            _ladderClimber.EnterLadder(_horizontalInput);

        if (collision.CompareTag("Dish"))
            GameManager.Instance.FinishGame();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
            _ladderClimber.ExitLadder();
    }

    public void DisableMovementTemporarily(float duration)
    {
        if (_disableMovementCoroutine != null)
            StopCoroutine(_disableMovementCoroutine);

        _disableMovementCoroutine = StartCoroutine(DisableMovementTemporarilyCoroutine(duration));
    }

    private IEnumerator DisableMovementTemporarilyCoroutine(float duration)
    {
        DisableMovement();
        yield return new WaitForSeconds(duration);
        EnableMovement();
    }

    public bool IsOnGround() => _groundChecker.CheckGrounded();

    public void EnableMovement() => _canMove = true;

    public void DisableMovement() => _canMove = false;
}