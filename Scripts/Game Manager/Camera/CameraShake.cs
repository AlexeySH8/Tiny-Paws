using System.Collections;
using System.Linq;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public Vector3 ShakeOffset {  get; private set; }

    [Header("Camera Max Shake")]
    [SerializeField] private float _maxShakeDuration;
    [SerializeField] private float _maxMagnitude;
    [SerializeField] private float _thresholdToMaxShaking;
    [Header("Camera Min Shake")]
    [SerializeField] private float _minShakeDuration;
    [SerializeField] private float _minMagnitude;
    [SerializeField] private float _thresholdToMinShaking;

    private Coroutine _shakeCoroutine;
    private PlayerHealth _playerHealth;

    public void Init(PlayerHealth playerHealth)
    {
        _playerHealth = playerHealth;
    }

    private void Start()
    {
        if (_playerHealth == null)
            Debug.LogError("Component _playerHealth is null");
        ShakeOffset = Vector3.zero;
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        _playerHealth.OnPlayerHPChanged += Shake;
    }

    private void OnDisable()
    {
        _playerHealth.OnPlayerHPChanged -= Shake;
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
            elapsed += Time.unscaledDeltaTime;

            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;
            ShakeOffset = new Vector3(x, y, 0);
            yield return null;
        }
        ShakeOffset = Vector3.zero;
    }
}
