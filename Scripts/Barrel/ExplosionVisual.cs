using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ExplosionVisual : MonoBehaviour
{
    [SerializeField] GameObject _shockWave;
    private float _animationDuration;
    private Material _material;
    private Coroutine _shockWaveCoroutine;
    private static int _waveDistanceFromCenter = Shader.PropertyToID("_WaveDistanceFromCenter");

    private void Awake()
    {
        _material = _shockWave.GetComponent<SpriteRenderer>().material;
    }

    void Start()
    {
        _animationDuration = GetAnimationDuration();
        DestroyAfterAnimation();
        CallShockWave();
    }

    private void CallShockWave() => _shockWaveCoroutine = StartCoroutine(CallShockWaveCoroutine(-0.1f, 1f));

    private IEnumerator CallShockWaveCoroutine(float startPos, float endPos)
    {
        _material.SetFloat(_waveDistanceFromCenter, startPos);
        float lerpedAmount = 0f;
        float elipsedTime = 0f;
        while (elipsedTime < _animationDuration)
        {
            elipsedTime += Time.deltaTime;
            lerpedAmount = Mathf.Lerp(startPos, endPos, (elipsedTime / _animationDuration));
            _material.SetFloat(_waveDistanceFromCenter, lerpedAmount);
            yield return null;
        }
    }

    private void DestroyAfterAnimation() => Destroy(gameObject, _animationDuration);

    private float GetAnimationDuration()
    {
        var animator = GetComponent<Animator>();
        var clip = animator.runtimeAnimatorController
            .animationClips
            .FirstOrDefault(c => c.name == "Explosion");

        if (clip == null)
        {
            Debug.LogWarning("Animation clip 'Explosion' not found.");
            Destroy(gameObject);
        }

        return clip.length;
    }
}
