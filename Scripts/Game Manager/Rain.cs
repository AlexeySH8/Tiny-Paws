using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Rain : MonoBehaviour
{
    [Header("Target Settings")]
    [SerializeField] private GameObject _targetToFollow;

    [Header("Rain Timing")]
    [SerializeField] private float _minTimeToRain;
    [SerializeField] private float _maxTimeToRain;
    [SerializeField] private float _rainDuration;

    [Header("Color Change")]
    [SerializeField] private GameObject _rainVolumeObj;
    [SerializeField] private float _delayBeforeColorChange;
    [SerializeField] private float _colorChangeDuration;
    [SerializeField] private float _targetWeight; // saturation of blue hue

    private ParticleSystem _rainParticleSystem;
    private Volume _rainVolume;
    private bool _isRaining;
    private float _yOffset;

    private void Start()
    {
        _yOffset = transform.position.y;
        _rainVolume = _rainVolumeObj.GetComponent<Volume>();
        _rainParticleSystem = GetComponent<ParticleSystem>();
        _rainVolume.weight = 0;
        StartCoroutine(Raining());
    }

    private void Update()
    {
        FollowTarget();
    }

    private IEnumerator Raining()
    {
        while (true)
        {
            var waitBeforeRain = Random.Range(_minTimeToRain, _maxTimeToRain);
            yield return new WaitForSeconds(waitBeforeRain);
            yield return StartCoroutine(StartRain());
        }
    }

    private IEnumerator StartRain()
    {
        _rainParticleSystem.Play();
        yield return StartCoroutine(ChangeColor(_rainVolume.weight, _targetWeight, true));
        yield return new WaitForSeconds(_rainDuration);
        _rainParticleSystem.Stop();
        yield return StartCoroutine(ChangeColor(_rainVolume.weight, 0, false));
    }

    private IEnumerator ChangeColor(float startWeight, float endWeight, bool hasDelay)
    {
        if (hasDelay)
            yield return new WaitForSeconds(_delayBeforeColorChange);
        float lerpedAmount = 0f;
        float elapsedTime = 0;
        while (elapsedTime < _colorChangeDuration)
        {
            elapsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startWeight, endWeight, (elapsedTime / _colorChangeDuration));
            _rainVolume.weight = lerpedAmount;
            yield return null;
        }
    }

    private void FollowTarget()
    {
        Vector3 position = transform.position;
        position.y = _targetToFollow.transform.position.y + _yOffset;
        transform.position = position;
    }
}
