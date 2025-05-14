using System.Collections;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    [SerializeField] float _radius;
    [SerializeField] float _force;
    [SerializeField] bool _isExploded;

    private byte _damage = 3;
    private float _delayBeforeExplosion = 1;
    private float _minTimeToSelfExplosion = 1;
    private float _maxTimeToSelfExplosion = 35;
    private bool _hasExploded;

    private void Start()
    {
        SelfExplosion();
    }

    private void Update()
    {
        if (_isExploded)
            Explode();
    }

    private void SelfExplosion() => StartCoroutine(SelfExplosionCoroutine());

    private IEnumerator SelfExplosionCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(_minTimeToSelfExplosion, _maxTimeToSelfExplosion));
        Explode();
    }

    public void Explode()
    {
        if (_hasExploded) return;
        _hasExploded = true;
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeExplosion);

        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D collider in overlappedColliders)
            ApplyExplosionEffects(collider);

        Destroy(gameObject);
    }

    private void ApplyExplosionEffects(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            float distance = Mathf.Max(Vector2.Distance(rb.position, transform.position), 0.01f);
            //float explosionForce = _force / distance;
            rb.AddForce(direction * _force);
        }

        if (collider.TryGetComponent(out Health health))
            health.TakeDamage(_damage);

        if (collider.TryGetComponent(out Explosion explosion))
            explosion.Explode();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Explode();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}
