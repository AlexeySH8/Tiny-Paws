using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private Dictionary<GameObject, List<GameObject>> _objectPools =
        new Dictionary<GameObject, List<GameObject>>();

    public void Init(List<GarbageType> garbageTypes)
    {
        foreach (var garbageType in garbageTypes)
            foreach (var garbage in garbageType.Prefabs)
                _objectPools.Add(garbage, new List<GameObject>());
    }

    public GameObject Get(GameObject prefab, Vector3 spawnPos, Quaternion rotation)
    {
        if (_objectPools.TryGetValue(prefab, out var garbagePool))
        {
            var garbage = garbagePool.FirstOrDefault(el => !el.activeSelf);
            if (garbage == null)
                garbage = Create(prefab, spawnPos, rotation);
            garbage.SetActive(true);
            return garbage;
        }
        else
        {
            Debug.LogError($"{prefab} is not in the object pool");
            return null;
        }
    }

    public void Release(GameObject garbage)
    {
        if (garbage.TryGetComponent<BaseBarrel>(out var barrel))
            barrel.ResetState();

        if (garbage.TryGetComponent<Rigidbody2D>(out var rb))
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        garbage.SetActive(false);
        garbage.transform.position = transform.position;
    }

    private GameObject Create(GameObject prefab, Vector3 spawnPos, Quaternion rotation)
    {
        var garbage = Instantiate(prefab, spawnPos, rotation);
        garbage.GetComponent<IPoolHandler>()?.Init(this);
        _objectPools[prefab].Add(garbage);
        return garbage;
    }
}
