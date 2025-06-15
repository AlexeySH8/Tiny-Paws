using System.Collections;
using UnityEngine;

public class ScaleTransition : MonoBehaviour
{
    [SerializeField] float _duration = 0.05f;
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
        StartCoroutine(ScaleInWithDelay());
    }

    private IEnumerator ScaleInWithDelay()
    {
        float elapsed = 0;
        while (elapsed < _delay)
        {
            elapsed += Time.unscaledDeltaTime;
            yield return null;
        }
        StartCoroutine(ScaleIn());
    }

    private IEnumerator ScaleIn()
    {
        yield return new WaitForSecondsRealtime(_delay);

        float elapsed = 0;
        while (elapsed < _duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float time = Mathf.Clamp01(elapsed / _duration);
            float curveValue = _curve.Evaluate(time);
            transform.localScale = Vector3.Lerp(Vector3.zero, _targetScale, curveValue);
            yield return null;
        }
        transform.localScale = _targetScale;
    }
}
