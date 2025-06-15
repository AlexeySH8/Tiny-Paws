using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    const int TotalChance = 100;

    [SerializeField] private List<GarbageType> _garbageTypes;

    [SerializeField] private float _xBorder = 30;
    [SerializeField] private float _yMinBorder = 40;
    [SerializeField] private float _yMaxBorder = 340;

    [SerializeField] private float _minTimeToRespawn = 0.4f;
    [SerializeField] private float _maxTimeToRespawn = 0.7f;

    [SerializeField] private int _initialGarbageAmount;

    private Animator _animator;
    private GarbageImpulse _garbageImpulse;
    private ObjectPool _objectPool;

    private void Awake()
    {
#if UNITY_EDITOR
        CheckTotalChance();
#endif
        _animator = GetComponentInParent<Animator>();
        _garbageImpulse = GetComponent<GarbageImpulse>();
        _objectPool = GetComponentInParent<ObjectPool>();
        _objectPool.Init(_garbageTypes);
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += StartSpawnGarbage;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= StartSpawnGarbage;
    }

    private void StartSpawnGarbage()
    {
        LoadInitialGarbage();
        SpawnGarbage();
    }

    private void LoadInitialGarbage()
    {
        for (int i = 0; i < _initialGarbageAmount; i++)
        {
            var randomPos = new Vector3(
                Random.Range(-_xBorder, _xBorder),
                Random.Range(_yMinBorder, _yMaxBorder),
                0);
            var garbagePrefab = GetRandomGarbagePrefab();
            _objectPool.Get(garbagePrefab, randomPos, garbagePrefab.transform.rotation);
        }
    }

    private void SpawnGarbage() => StartCoroutine(SpawnGarbageCoroutine());

    private IEnumerator SpawnGarbageCoroutine()
    {
        while (true)
        {
            float timeToNextObstacle = Random.Range(_minTimeToRespawn, _maxTimeToRespawn);
            yield return new WaitForSeconds(timeToNextObstacle);

            _animator.Play("Garbage Disposal", 0, 0f);

            var garbagePrefab = GetRandomGarbagePrefab();

            if (garbagePrefab != null)
            {
                var instance = _objectPool.Get(garbagePrefab, transform.position,garbagePrefab.transform.rotation);
                _garbageImpulse.ApplyImpulseToGarbage(instance);
            }
        }
    }

    private GameObject GetRandomGarbagePrefab()
    {
        int random = Random.Range(0, TotalChance);
        int cumulative = 0;

        foreach (var type in _garbageTypes)
        {
            cumulative += type.Chance;
            if (random < cumulative && type.Prefabs.Count > 0)
                return type.Prefabs[Random.Range(0, type.Prefabs.Count)];
        }
        Debug.LogWarning("No garbage prefab selected");
        return null;
    }

#if UNITY_EDITOR
    private void CheckTotalChance()
    {
        int totalChance = 0;
        foreach (var type in _garbageTypes)
            totalChance += type.Chance;
        if (totalChance != TotalChance)
            Debug.LogError($"Total spawn chance must be 100. Current: {totalChance}");
    }
#endif
}