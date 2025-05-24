using System.Collections;
using UnityEngine;

public class ExplosionBarrel : BaseBarrel
{
    [SerializeField] private float _force;
    [SerializeField] private GameObject _explosionEffectPref;

    private float _delayBeforeExplosion = 1;
    private float _offsetForPlayer = 3f;
    private float _disableMovementDuration = 0.5f;
    private Animator _animator;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public override void BarrelEffect() => Explode();

    public override void ReactToDeath() => Explode();

    public void Explode()
    {
        if (hasActivated) return;
        hasActivated = true;
        _animator.SetBool("HasActivated", hasActivated);
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeExplosion);

        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D collider in overlappedColliders)
            ApplyExplosionEffects(collider);

        Instantiate(_explosionEffectPref, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void ApplyExplosionEffects(Collider2D collider)
    {
        if (collider.TryGetComponent(out Rigidbody2D rb))
        {
            Vector2 direction = (collider.transform.position - transform.position).normalized;
            var explosionForce = direction * _force;
            if (rb.TryGetComponent(out PlayerController player))
            {
                explosionForce /= _offsetForPlayer;
                player.DisableMovementTemporarily(_disableMovementDuration);
            }
            rb.AddForce(explosionForce);
        }

        if (collider.TryGetComponent(out BaseHealth health))
            health.TakeDamage(damage);
    }
}
