using UnityEngine;

public class GarbageImpulse : MonoBehaviour
{
    [SerializeField] private float _minReboundForce = 1.0f;
    [SerializeField] private float _maxReboundForce = 13000.0f;
    [SerializeField] private float _maxTorqueForce = 20;

    public void ApplyImpulseToGarbage(GameObject instance)
    {
        var rb = instance.GetComponent<Rigidbody2D>();

        rb.AddForce(new Vector2(-1f, 0) *
            Random.Range(_minReboundForce, _maxReboundForce), ForceMode2D.Impulse);

        rb.AddTorque(Random.Range(-_maxTorqueForce, _maxTorqueForce), ForceMode2D.Impulse);
    }
}
