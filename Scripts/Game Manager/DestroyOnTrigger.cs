using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    public LayerMask Garbage;
    [SerializeField] ObjectPool _objectPool;

    private void Awake()
    {
        _objectPool = FindAnyObjectByType<ObjectPool>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (((1 << other.gameObject.layer) & Garbage.value) != 0)
            _objectPool.Release(other.gameObject);
    }
}
