using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private Transform _playerPos;
    [SerializeField] private AudioSource _factorySource;
    [SerializeField] private AudioSource _citySource;
    [SerializeField] private AudioSource _skySource;
    [SerializeField] private AudioSource _rainSource;

    private float _maxFactoryVolume;
    private float _maxCityVolume;
    private float _maxSkyVolume;

    void Start()
    {
        SetMaxVolume();
        StartPlaySources();
    }

    void Update()
    {
        float y = _playerPos.position.y;

        _factorySource.volume = Mathf.Clamp01(_maxFactoryVolume - Mathf.Abs(y - 50f) / 50f);
        _citySource.volume = Mathf.Clamp01(_maxCityVolume - Mathf.Abs(y - 150f) / 50f);
        _skySource.volume = Mathf.Clamp01(_maxSkyVolume - Mathf.Abs(y - 250f) / 50f);
    }

    private void SetMaxVolume()
    {
        _maxFactoryVolume = _factorySource.volume;
        _maxCityVolume = _citySource.volume;
        _maxSkyVolume = _skySource.volume;
    }

    private void StartPlaySources()
    {
        _factorySource.Play();
        _citySource.Play();
        _skySource.Play();
    }

    public void StartRain() => _rainSource.Play();
}
