using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] float _durationSceneTransition;
    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGamePause;
    public event Action OnGameResume;

    private void Awake()
    {
        #region Initialization
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        #endregion
    }

    public void GameStart() => StartCoroutine(GameStartCoroutine());

    private IEnumerator GameStartCoroutine()
    {
        UIManager.Instance.HideHomePage();
        yield return TimelineManager.Instance.OpeningCutscene();
        OnGameStart?.Invoke();
    }

    public void GameOver() => StartCoroutine(GameOverCoroutine());

    private IEnumerator GameOverCoroutine()
    {
        OnGameOver?.Invoke();
        yield return CircleTransition.Instance.SceneTransition(); // OpenBlackScreen() called in parallel
        GameRestart();
    }

    public void FinishGame() => StartCoroutine(FinishGameCoroutine());

    private IEnumerator FinishGameCoroutine()
    {
        OnGameOver?.Invoke();
        yield return CircleTransition.Instance.CloseBlackScreenCoroutine();
        yield return TimelineManager.Instance.FinishCutsceneCoroutine();
        CircleTransition.Instance.OpenBlackScreen();
        GameRestart();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GamePause() => OnGamePause?.Invoke();

    public void GameResume() => OnGameResume?.Invoke();
}
