using System.Collections;
using UnityEngine;

public class ScaleTransition : MonoBehaviour
{
    [SerializeField] float _duration = 0.2f;
    [SerializeField] float _delay;
    [SerializeField] private AnimationCurve _curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    private Vector3 _targetScale;

    private void Awake()
    {
        _targetScale = transform.localScale;
    }

    private void OnEnable()
    {
        transform.localScale = Vector3.zero;
        float elapsed = 0;
        while (elapsed < _delay)
            elapsed += Time.deltaTime;
        StartCoroutine(ScaleInWithDelay());
    }

    private IEnumerator ScaleInWithDelay()
    {
        float elapsed = 0;
        while (elapsed < _delay)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        StartCoroutine(ScaleIn());
    }

    private IEnumerator ScaleIn()
    {
        float elapsed = 0;
        while (elapsed < _duration)
        {
            elapsed += Time.deltaTime;
            float time = Mathf.Clamp01(elapsed / _duration);
            float curveValue = _curve.Evaluate(time);
            transform.localScale = Vector3.Lerp(Vector3.zero, _targetScale, time);
            yield return null;
        }
        transform.localScale = _targetScale;
    }
}
