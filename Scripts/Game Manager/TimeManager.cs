using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
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
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += ResumeGame;
        GameManager.Instance.OnGamePause += PauseGame;
        GameManager.Instance.OnGameResume += ResumeGame;
    }
    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= ResumeGame;
        GameManager.Instance.OnGamePause -= PauseGame;
        GameManager.Instance.OnGameResume -= ResumeGame;
    }

    public void PauseGame() => Time.timeScale = 0;

    public void ResumeGame() => Time.timeScale = 1;
}
