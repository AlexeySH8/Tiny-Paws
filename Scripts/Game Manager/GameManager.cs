using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnGamePause;
    public event Action OnGameResume;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void GameStart() => OnGameStart?.Invoke();

    public void GameOver()
    {
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        OnGameOver?.Invoke();
        yield return CircleTransition.Instance.SceneTransition(); // OpenBlackScreen() called in parallel
        GameRestart();
    }

    public void GameRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GamePause() => OnGamePause?.Invoke();

    public void GameResume() => OnGameResume?.Invoke();
}
