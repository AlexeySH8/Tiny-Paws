using System.Collections;
using TMPro;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Follow Player")]
    [SerializeField] private float _smoothTime = 0.1f;
    [SerializeField] private GameObject _player;
    private float _xBoundary = 14.0f;
    private float _minYBoundary = 0f;
    private float _maxYBoundary = 340f;
    private Vector3 _velocity = Vector3.zero;
    private Vector3 _initialObservPos;
    private float _yOffset;
    private bool _isObserved;
    private CameraShake _shake;
    private CameraSetting _setting;

    private void Awake()
    {
        _setting = GetComponent<CameraSetting>();
        _shake = GetComponent<CameraShake>();
        _shake.SetPlayerHealth(_player.GetComponent<PlayerHealth>());
    }

    private void Start()
    {
        _initialObservPos = new Vector3(0f, 5f, _setting.ZPosition);
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += StartObservePlayer;
    }

    private void OnDisable()
    {
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

        Vector3 targetPosition = new Vector3(targetX, targetY, _setting.ZPosition);
        var smoothedPosition = Vector3.SmoothDamp(
            transform.position, targetPosition, ref _velocity, _smoothTime);

        transform.position = smoothedPosition + _shake.ShakeOffset;
    }

    private void StartObservePlayer()
    {
        transform.position = _initialObservPos;
        _yOffset = transform.position.y;
        _isObserved = true;
    }
}
