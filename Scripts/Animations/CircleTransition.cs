using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    private const string RADIUS = "_Radius";
    private const string CENTER_X = "_CenterX";
    private const string CENTER_Y = "_CenterY";
    [SerializeField] GameObject _target;
    [SerializeField] private float _closeDuration;
    [SerializeField] private float _openDuration;
    private Canvas _canvas;
    private Image _blackScreen;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        _blackScreen = GetComponentInChildren<Image>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
            CloseBlackScreen();
        if (Input.GetKeyDown(KeyCode.T))
            StartCoroutine(OpenBlackScreen());
    }

    private void CloseBlackScreen()
    {
        DrawBlackScreen();
        StartCoroutine(Transition(_closeDuration, 1f, 0f));
    }

    private IEnumerator OpenBlackScreen()
    {
        yield return StartCoroutine(Transition(_openDuration, 0f, 1f));
        HideBlackScreen();
    }

    private IEnumerator Transition(float duration, float startRadius, float endRadius)
    {
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            var time = elapsed / duration;
            var radius = Mathf.Lerp(startRadius, endRadius, time);
            _blackScreen.material.SetFloat(RADIUS, radius);
            yield return null;
        }
    }

    private void DrawBlackScreen()
    {
        var screenWidth = Screen.width;
        var screenHeight = Screen.height;

        var canvasRect = _canvas.GetComponent<RectTransform>().rect;
        float canvasWidth = canvasRect.width;
        float canvasHeight = canvasRect.height;

        float squareValue = 0f;

        if (canvasWidth > canvasHeight)
            squareValue = canvasWidth;
        else squareValue = canvasHeight;


        var targetScreenPos = Camera.main.WorldToScreenPoint(_target.transform.position);
        float targetUVx = targetScreenPos.x / Screen.width;
        float targetUVy = targetScreenPos.y / Screen.height;

        var mat = _blackScreen.material;
        _blackScreen.material.SetFloat(CENTER_X, targetUVx);
        _blackScreen.material.SetFloat(CENTER_Y, targetUVy);

        _blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }

    private void HideBlackScreen() => _blackScreen.rectTransform.sizeDelta = Vector2.zero;
}
