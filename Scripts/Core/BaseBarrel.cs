using System.Collections;
using UnityEngine;

public abstract class BaseBarrel : BaseHealth
{
    [SerializeField] protected float radius;
    [SerializeField] protected int damage;
    protected bool hasActivated;
    private float _minSelfExplodeTime = 1;
    private float _maxSelfExplodeTime = 40;

    protected virtual void Start()
    {
        SelfActivated();
    }

    private void SelfActivated() => StartCoroutine(SelfActivatedCoroutine());

    private IEnumerator SelfActivatedCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(_minSelfExplodeTime, _maxSelfExplodeTime));
        BarrelEffect();
    }

    public abstract void BarrelEffect();

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            BarrelEffect();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}