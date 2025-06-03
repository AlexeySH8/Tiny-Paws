using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    public static CircleTransition Instance;
    [SerializeField] public string TargetTag;

    private const string RADIUS = "_Radius";
    private const string CENTER_X = "_CenterX";
    private const string CENTER_Y = "_CenterY";
    [SerializeField] private float _closeDuration;
    [SerializeField] private float _openDuration;
    private Canvas _canvas;
    private Image _blackScreen;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        _canvas = GetComponent<Canvas>();
        _blackScreen = GetComponentInChildren<Image>();
    }

    public IEnumerator SceneTransition(float duration = 0)
    {
        yield return CloseBlackScreenCoroutine();
        yield return new WaitForSeconds(duration);
        OpenBlackScreen();
    }

    private IEnumerator CloseBlackScreenCoroutine()
    {
        DrawBlackScreen();
        yield return StartCoroutine(Transition(_closeDuration, 1f, 0f, false));
    }

    private void OpenBlackScreen() => StartCoroutine(OpenBlackScreenCoroutine());

    private IEnumerator OpenBlackScreenCoroutine()
    {
        yield return StartCoroutine(Transition(_openDuration, 0f, 1f, true));
        HideBlackScreen();
    }

    private IEnumerator Transition(float duration, float startRadius,
        float endRadius, bool isMenu)
    {
        SetTransitionCenter(isMenu);
        float elapsed = 0;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            var time = elapsed / duration;
            var radius = Mathf.Lerp(startRadius, endRadius, time);
            _blackScreen.material.SetFloat(RADIUS, radius);
            yield return null;
        }
    }

    private void SetTransitionCenter(bool isMenu)
    {
        float targetUVx = 0.5f;
        float targetUVy = 0.5f;

        if (!isMenu)
        {
            var target = GameObject.FindWithTag(TargetTag);
            if (!target) Debug.LogError($"TargetTag {TargetTag} not found or empty");

            Vector3 targetScreenPos = Camera.main
            .WorldToScreenPoint(target.transform.position);

            targetUVx = targetScreenPos.x / Screen.width;
            targetUVy = targetScreenPos.y / Screen.height;
        }

        var mat = _blackScreen.material;
        _blackScreen.material.SetFloat(CENTER_X, targetUVx);
        _blackScreen.material.SetFloat(CENTER_Y, targetUVy);
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

        _blackScreen.rectTransform.sizeDelta = new Vector2(squareValue, squareValue);
    }

    private void HideBlackScreen() => _blackScreen.rectTransform.sizeDelta = Vector2.zero;
}
