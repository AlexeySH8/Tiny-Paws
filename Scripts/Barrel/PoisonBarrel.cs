using System.Collections;
using UnityEngine;

public class PoisonBarrel : BaseBarrel
{
    [SerializeField] private GameObject _poisonEffect;
    [SerializeField] private float _poisonInterval = 1.0f;

    protected override AudioClip _soundEffect => SFX.Instance.PoisonLeak;

    protected override void BarrelEffect() => PoisonLeak();

    protected override void ReactToDeath() => PoisonLeak();

    private void PoisonLeak()
    {
        if (_hasActivated) return;
        _hasActivated = true;
        PlaySFX();
        Poison();
        _poisonEffect.SetActive(true);
    }

    private void Poison() => StartCoroutine(PoisonCoroutine());

    private IEnumerator PoisonCoroutine()
    {
        while (_hasActivated)
        {
            yield return new WaitForSeconds(_poisonInterval);
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, _radius);
            foreach (Collider2D collider in overlappedColliders)
            {
                if (collider.TryGetComponent(out BaseHealth health))
                    health.TakeDamage(_damage);
            }
        }
    }

    public override void ResetState()
    {
        ResetHealth();
        _hasActivated = false;
        StopSFX();
        _poisonEffect.SetActive(false);
        StopAllCoroutines();
    }
}
