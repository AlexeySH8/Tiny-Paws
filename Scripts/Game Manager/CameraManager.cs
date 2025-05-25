using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Follow Player")]
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private GameObject _player;
    private float _xBoundary = 14.0f;
    private float _minYBoundary = 0f;
    private float _maxYBoundary = 340f;
    private Vector3 _initialObservPos = new Vector3(0f, 5f, -33f);
    private Vector3 _velocity = Vector3.zero;
    private float _zPosition;
    private float _yOffset;
    private bool _isObserved;

    [Header("Camera Max Shake")]
    [SerializeField] private float _maxShakeDuration;
    [SerializeField] private float _maxMagnitude;
    [SerializeField] private float _thresholdToMaxShaking;
    [Header("Camera Min Shake")]
    [SerializeField] private float _minShakeDuration;
    [SerializeField] private float _minMagnitude;
    [SerializeField] private float _thresholdToMinShaking;
    private Vector3 _shakeOffset = Vector3.zero;
    private Coroutine _shakeCoroutine;
    private PlayerHealth _playerHealth;

    private void Start()
    {
        _playerHealth = _player.GetComponent<PlayerHealth>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _playerHealth.OnPlayerTakeDamage += Shake;
        GameManager.Instance.OnGameStart += StartObservePlayer;
    }

    private void OnDisable()
    {
        _playerHealth.OnPlayerTakeDamage -= Shake;
        GameManager.Instance.OnGameStart -= StartObservePlayer;
    }

    private void LateUpdate()
    {
        if (_isObserved)
            ObservePlayer();
    }

    private void ObservePlayer()
    {
        var targetX = Mathf.Clamp(_player.transform.position.x,
            -_xBoundary, _xBoundary);

        var targetY = Mathf.Clamp(_player.transform.position.y + _yOffset,
            _minYBoundary, _maxYBoundary);

        Vector3 targetPosition = new Vector3(targetX, targetY, _zPosition);
        var smoothedPosition = Vector3.SmoothDamp(
            transform.position, targetPosition, ref _velocity, _smoothTime);

        transform.position = smoothedPosition + _shakeOffset;
    }

    private void StartObservePlayer()
    {
        transform.position = _initialObservPos;
        _yOffset = transform.position.y;
        _zPosition = transform.position.z;
        _isObserved = true;
    }

    private void Shake(int currentHP, int damageTaken)
    {
        if (_shakeCoroutine != null)
            StopCoroutine(_shakeCoroutine);

        if (damageTaken <= _thresholdToMinShaking)
            _shakeCoroutine = StartCoroutine(ShakeCoroutine(_minShakeDuration, _minMagnitude));
        else if (damageTaken >= _thresholdToMaxShaking)
            _shakeCoroutine = StartCoroutine(ShakeCoroutine(_maxShakeDuration, _maxMagnitude));
    }

    private IEnumerator ShakeCoroutine(float duration, float magnitude)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            _shakeOffset = new Vector3(x, y, 0);

            yield return null;
        }
        _shakeOffset = Vector3.zero;
    }
}
