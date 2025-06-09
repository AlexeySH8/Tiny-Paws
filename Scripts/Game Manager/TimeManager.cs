using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static TimeManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        ResumeGame();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameOver += PauseGame;
        GameManager.Instance.OnGameFinish += PauseGame;
        GameManager.Instance.OnGamePause += PauseGame;
        GameManager.Instance.OnGameResume += ResumeGame;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= PauseGame;
        GameManager.Instance.OnGameFinish -= PauseGame;
        GameManager.Instance.OnGamePause -= PauseGame;
        GameManager.Instance.OnGameResume -= ResumeGame;
    }

    public void PauseGame() => Time.timeScale = 0;

    public void ResumeGame() => Time.timeScale = 1;
}
