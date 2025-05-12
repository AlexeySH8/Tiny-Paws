using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawner : MonoBehaviour
{
    public enum SpawnDirection
    {
        Left = -1,
        Right = 1
    }

    [SerializeField] private List<GameObject> _garbagePrefabs;
    [SerializeField] private SpawnDirection _spawnDirection;
    private Animator _animator;

    private float _minTimeToResp = 0.8f;
    private float _maxTimeToResp = 1.8f;

    private float _minReboundForce = 1.0f;
    private float _maxReboundForce = 8000.0f;
    private float _maxTorqueForce = 10;

    private void Start()
    {
        _animator = GetComponentInParent<Animator>();
        SpawnGrbage();
    }

    private void SpawnGrbage() => StartCoroutine(SpawnGrbageCoroutine());

    private IEnumerator SpawnGrbageCoroutine()
    {
        while (true)
        {
            float timeToNextObstacle = Random.Range(_minTimeToResp, _maxTimeToResp);
            yield return new WaitForSeconds(timeToNextObstacle);
            _animator.Play("Garbage Disposal", 0, 0f);
            var garbagePrefab = _garbagePrefabs[Random.Range(0, _garbagePrefabs.Count)];
            var instance = Instantiate(garbagePrefab, transform.position, garbagePrefab.transform.rotation);
            ImpulseToGrbage(instance);
        }
    }

    private void ImpulseToGrbage(GameObject instance)
    {
        var rb = instance.GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2((float)_spawnDirection, 0) *
            Random.Range(_minReboundForce, _maxReboundForce), ForceMode2D.Impulse);

        rb.AddTorque(Random.Range(-_maxTorqueForce, _maxTorqueForce), ForceMode2D.Impulse);
    }
}
