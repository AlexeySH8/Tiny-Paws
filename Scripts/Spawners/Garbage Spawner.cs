using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    [SerializeField] private List<GarbageType> _garbageTypes;
    [Header("Spawn Settings")]
    [SerializeField] private float _xBorder = 30;
    [SerializeField] private float _yMinBorder = 40;
    [SerializeField] private float _yMaxBorder = 340;

    [SerializeField] private float _minTimeToRespawn = 0.4f;
    [SerializeField] private float _maxTimeToRespawn = 0.7f;

    [SerializeField] private float _minReboundForce = 1.0f;
    [SerializeField] private float _maxReboundForce = 13000.0f;
    [SerializeField] private float _maxTorqueForce = 20;

    [SerializeField] private int _initialGarbageAmount;
    private Animator _animator;

    private void Awake()
    {
#if UNITY_EDITOR
        CheckTotalChance();
#endif
        _animator = GetComponentInParent<Animator>();
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
            Instantiate(garbagePrefab, randomPos, garbagePrefab.transform.rotation);
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
                var instance = Instantiate(garbagePrefab, transform.position, garbagePrefab.transform.rotation);
                ApplyImpulseToGarbage(instance);
            }
        }
    }

    private GameObject GetRandomGarbagePrefab()
    {
        int random = Random.Range(0, 100);
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

    private void ApplyImpulseToGarbage(GameObject instance)
    {
        var rb = instance.GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(-1f, 0) *
            Random.Range(_minReboundForce, _maxReboundForce), ForceMode2D.Impulse);

        rb.AddTorque(Random.Range(-_maxTorqueForce, _maxTorqueForce), ForceMode2D.Impulse);
    }

#if UNITY_EDITOR
    private void CheckTotalChance()
    {
        int totalChance = 0;
        foreach (var type in _garbageTypes)
            totalChance += type.Chance;
        if (totalChance != 100)
            Debug.LogError($"Total spawn chance must be 100. Current: {totalChance}");
    }
#endif
}
