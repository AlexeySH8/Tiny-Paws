using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSound : MonoBehaviour
{
    [SerializeField] private Transform _playerPos;
    [SerializeField] private AudioSource _factorySource; // 0 - 80 Y axis
    [SerializeField] private AudioSource _citySource; // 70 - 260 Y axis
    [SerializeField] private AudioSource _skySource; // 250 - 350 Y axis
    [SerializeField] private AudioSource _rainSource;

    private float _zoneWidth = 40f;
    private float _endFactory = 80f;
    private float _startCity = 70f;
    private float _endCity = 300f;
    private float _startSky = 250f;

    private float _maxFactoryVolume;
    private float _maxCityVolume;
    private float _maxSkyVolume;
    private bool _canUpdateVolume;
    private bool _isRaining;

    private void Awake()
    {
        SetMaxVolume();
        DisableUpdateVolume();
    }

    private void Start()
    {
        _citySource.Play();
        SubscribeToEvents();
    }

    private void Update()
    {
        if (_canUpdateVolume)
            UpdateVolumes();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += EnableUpdateVolume;
        GameManager.Instance.OnGameStart += PlaySources;
        GameManager.Instance.OnGamePause += PauseSources;
        GameManager.Instance.OnGameResume += PlaySources;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= EnableUpdateVolume;
        GameManager.Instance.OnGameStart -= PlaySources;
        GameManager.Instance.OnGamePause -= PauseSources;
        GameManager.Instance.OnGameResume -= PlaySources;
    }

    private void UpdateVolumes()
    {
        float y = _playerPos.position.y;

        float factoryDisctance = y - _zoneWidth;
        _factorySource.volume = Mathf.Clamp01(1f -
            Mathf.Clamp01(factoryDisctance / _endFactory)) * _maxFactoryVolume;

        if (y < _startCity || y > _endCity)
            _citySource.volume = 0f;
        else if (y <= (_startCity + _zoneWidth))
            _citySource.volume = Mathf.Lerp(0, _maxCityVolume, (y - _startCity) / _zoneWidth);
        else if (y <= (_endCity - _zoneWidth))
            _citySource.volume = _maxCityVolume;
        else
            _citySource.volume = Mathf.Lerp(_maxCityVolume, 0f, (y - (_endCity - _zoneWidth)) / _zoneWidth);

        _skySource.volume = Mathf.Clamp01((y - _startSky) / 50f) * _maxSkyVolume;
    }

    private void SetMaxVolume()
    {
        _maxFactoryVolume = _factorySource.volume;
        _maxCityVolume = _citySource.volume;
        _maxSkyVolume = _skySource.volume;
    }

    private void PlaySources()
    {
        _factorySource.Play();
        _citySource.Play();
        _skySource.Play();
        if (_isRaining)
            _rainSource.Play();
    }

    private void PauseSources()
    {
        _factorySource.Pause();
        _citySource.Pause();
        _skySource.Pause();
        _rainSource.Pause();
    }

    public void StartRain()
    {
        _isRaining = true;
        _rainSource.Play();
    }

    public void StopRain()
    {
        _isRaining = false;
        _rainSource.Stop();
    }

    private void EnableUpdateVolume() => _canUpdateVolume = true;

    private void DisableUpdateVolume() => _canUpdateVolume = false;
}
