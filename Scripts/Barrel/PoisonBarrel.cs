using System.Collections;
using UnityEngine;

public class PoisonBarrel : BaseBarrel
{
    [SerializeField] private GameObject _poisonEffectPref;
    [SerializeField] private float _poisonInterval = 1.0f;

    public override void BarrelEffect() => PoisonLeak();

    public override void ReactToDeath() => PoisonLeak();

    private void PoisonLeak()
    {
        if (hasActivated) return;
        hasActivated = true;
        Poison();
        Instantiate(_poisonEffectPref, transform);
    }    

    private void Poison() => StartCoroutine(PoisonCoroutine());

    private IEnumerator PoisonCoroutine()
    {
        while (hasActivated)
        {
            yield return new WaitForSeconds(_poisonInterval);
            Collider2D[] overlappedColliders = Physics2D.OverlapCircleAll(transform.position, radius);
            foreach (Collider2D collider in overlappedColliders)
            {
                if (collider.TryGetComponent(out BaseHealth health))
                    health.TakeDamage(damage);
            }
        }
    }   
}
