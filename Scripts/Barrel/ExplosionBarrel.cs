using System.Collections;
using UnityEngine;

public class ExplosionBarrel : BaseBarrel, IPoolHandler
{
    [SerializeField] private float _force;
    [SerializeField] private GameObject _explosionEffectPref;

    private float _delayBeforeExplosion = 1;
    private float _offsetForPlayer = 3f;
    private float _disableMovementDuration = 0.5f;
    private Animator _animator;
    private ObjectPool _objectPool;

    protected override void Start()
    {
        base.Start();
        _animator = GetComponent<Animator>();
    }

    public void Init(ObjectPool pool)
    {
        _objectPool = pool;
    }

    protected override AudioClip _soundEffect => SFX.Instance.Explosion;

    protected override void BarrelEffect() => Explode();

    protected override void ReactToDeath() => Explode();

    public void Explode()
    {
        if (_hasActivated) return;
        _hasActivated = true;
        _animator.SetBool("HasActivated", _hasActivated);
        StartCoroutine(ExplodeCoroutine());
    }

    private IEnumerator ExplodeCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeExplosion);

        Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, _radius);

        foreach (Collider2D collider in overlappedColliders)
            ApplyExplosionEffects(collider);

        var durationSFX = PlaySFX();
        Instantiate(_explosionEffectPref, transform.position, Quaternion.identity);
        GetComponent<SpriteRenderer>().enabled = false;
        yield return new WaitForSeconds(durationSFX);
        _objectPool.Release(gameObject);
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
            health.TakeDamage(_damage);
    }

    public override void ResetState()
    {
        ResetHealth();
        _hasActivated = false;
        GetComponent<SpriteRenderer>().enabled = true;
        _animator.SetBool("HasActivated", _hasActivated);
        StopAllCoroutines();
    }
}
