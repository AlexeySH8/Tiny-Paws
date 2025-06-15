using System.Collections;
using UnityEngine;

public abstract class BaseBarrel : BaseHealth
{
    [SerializeField] protected float _radius;
    [SerializeField] protected int _damage;
    protected abstract AudioClip _soundEffect { get; }
    private AudioSource _sfxSource;
    protected bool _hasActivated;
    private float _minSelfExplodeTime = 1;
    private float _maxSelfExplodeTime = 40;

    protected virtual void Start()
    {
        SetSFX();
        SelfActivated();
    }

    protected abstract void BarrelEffect();

    public abstract void ResetState();

    private void SelfActivated() => StartCoroutine(SelfActivatedCoroutine());

    private IEnumerator SelfActivatedCoroutine()
    {
        yield return new WaitForSeconds(Random.Range(_minSelfExplodeTime, _maxSelfExplodeTime));
        BarrelEffect();
    }

    private void SetSFX()
    {
        _sfxSource = GetComponent<AudioSource>();
        if (_soundEffect == null)
        {
            Debug.LogError($"{GetType().Name} on {gameObject.name} sound effect is not set");
            return;
        }
        _sfxSource = GetComponent<AudioSource>();
        _sfxSource.clip = _soundEffect;
    }

    protected float PlaySFX()
    {
        _sfxSource.Play();
        return _sfxSource.clip.length;
    }

    protected void StopSFX()
    {
        _sfxSource.Stop();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            BarrelEffect();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, _radius);
    }
}