using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public enum SpawnDirection
    {
        Left = -1,
        Right = 1
    }

    [Header("Prefab Lists")]
    [SerializeField] private List<GameObject> _cartonPrefabs;
    [SerializeField] private List<GameObject> _ironPrefabs;
    [SerializeField] private List<GameObject> _barrelPrefabs;

    [Header("Spawn Chances (0-100)")]
    [SerializeField][Range(0, 100)] private int _cartonChance = 60;
    [SerializeField][Range(0, 100)] private int _ironChance = 30;
    [SerializeField][Range(0, 100)] private int _barrelChance = 10;

    [SerializeField] private int _initialGarbageAmount;
    [SerializeField] private SpawnDirection _spawnDirection;
    private Animator _animator;

    private float _xBorder = 30;
    private float _yMinBorder = 40;
    private float _yMaxBorder = 340;

    private float _minTimeToResp = 0.3f;
    private float _maxTimeToResp = 0.6f;

    private float _minReboundForce = 1.0f;
    private float _maxReboundForce = 13000.0f;
    private float _maxTorqueForce = 20;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
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
        SpawnGrbage();
    }

    private void LoadInitialGarbage()
    {
        for (int i = 0; i <= _initialGarbageAmount; i++)
        {
            var randomPos = new Vector3(
                Random.Range(_xBorder, -_xBorder),
                Random.Range(_yMinBorder, _yMaxBorder),
                0);
            var garbagePrefab = GetRandomGarbagePrefab();
            Instantiate(garbagePrefab, randomPos, garbagePrefab.transform.rotation);
        }
    }

    private void SpawnGrbage() => StartCoroutine(SpawnGrbageCoroutine());

    private IEnumerator SpawnGrbageCoroutine()
    {
        while (true)
        {
            float timeToNextObstacle = Random.Range(_minTimeToResp, _maxTimeToResp);
            yield return new WaitForSeconds(timeToNextObstacle);

            _animator.Play("Garbage Disposal", 0, 0f);

            var garbagePrefab = GetRandomGarbagePrefab();

            if (garbagePrefab != null)
            {
                var instance = Instantiate(garbagePrefab, transform.position, garbagePrefab.transform.rotation);
                ImpulseToGarbage(instance);
            }
        }
    }

    private GameObject GetRandomGarbagePrefab()
    {
        int random = Random.Range(0, 100);

        if (random < _cartonChance && _cartonPrefabs.Count > 0)
        {
            return _cartonPrefabs[Random.Range(0, _cartonPrefabs.Count)];
        }
        else if (random < _cartonChance + _ironChance && _ironPrefabs.Count > 0)
        {
            return _ironPrefabs[Random.Range(0, _ironPrefabs.Count)];
        }
        else if (_barrelPrefabs.Count > 0)
        {
            return _barrelPrefabs[Random.Range(0, _barrelPrefabs.Count)];
        }
        else
        {
            Debug.LogError("The list of prefabs is empty");
            return null;
        }
    }

    private void ImpulseToGarbage(GameObject instance)
    {
        var rb = instance.GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2((float)_spawnDirection, 0) *
            Random.Range(_minReboundForce, _maxReboundForce), ForceMode2D.Impulse);

        rb.AddTorque(Random.Range(-_maxTorqueForce, _maxTorqueForce), ForceMode2D.Impulse);
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        int total = _cartonChance + _ironChance + _barrelChance;
        if (total != 100)
        {
            Debug.LogError($"The sum of the chances must be equal to 100, now: {total}");
        }
    }
#endif
}
