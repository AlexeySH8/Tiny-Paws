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

    private CameraShake _shake;

    [Header("Start Animation")]
    [SerializeField] private float _offsetAnimation;
    [SerializeField] private float _speedStartAnim;
    private bool _movingUp;
    private float _yMinBorder;
    private float _yMaxBorder;

    private void Awake()
    {
        _shake = GetComponent<CameraShake>();
        _shake.SetPlayerHealth(_player.GetComponent<PlayerHealth>());
        float currentPos = transform.position.y;
        _yMinBorder = currentPos - _offsetAnimation;
        _yMaxBorder = currentPos + _offsetAnimation;
    }

    private void Start()
    {
        SubscribeToEvents();
        StartCameraAnimation();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += GameStart;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStart;
    }

    private void LateUpdate()
    {
        if (_isObserved)
            ObservePlayer();
    }

    private void GameStart()
    {
        StartObservePlayer();
        StopCameraAnimation();
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

        transform.position = smoothedPosition + _shake.ShakeOffset;
    }

    private void StartObservePlayer()
    {
        transform.position = _initialObservPos;
        _yOffset = transform.position.y;
        _zPosition = transform.position.z;
        _isObserved = true;
    }

    private void StartCameraAnimation() => StartCoroutine(CameraAnimation());

    private void StopCameraAnimation() => StopCoroutine(CameraAnimation());

    private IEnumerator CameraAnimation()
    {
        while (true)
        {
            if (transform.position.y >= _yMaxBorder)
                _movingUp = false;
            else if (transform.position.y <= _yMinBorder)
                _movingUp = true;
            float direction = _movingUp ? 1f : -1f;
            transform.position += Vector3.up * direction * _speedStartAnim * Time.deltaTime;
            yield return null;
        }
    }
}
