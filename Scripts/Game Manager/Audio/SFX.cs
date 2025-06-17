using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFX : MonoBehaviour
{
    public static SFX Instance { get; private set; }

    [Header("Player")]
    [SerializeField] private AudioClip[] _jumpSounds;
    [SerializeField] AudioClip _takeDamage;
    [SerializeField] AudioClip _catPurring;

    [Header("Barrel")]
    [SerializeField] AudioClip _explosion;
    [SerializeField] AudioClip _poisonLeak;

    [Header("Game")]
    [SerializeField] AudioClip _gameOver;
    [SerializeField] AudioClip _gameFinish;
    [SerializeField] AudioClip _clickButton;

    private AudioSource _sfxSource;
    private PlayerHealth _playerHealth;


    public AudioClip Explosion { get; private set; }
    public AudioClip PoisonLeak { get; private set; }

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
        Explosion = _explosion;
        PoisonLeak = _poisonLeak;
        _sfxSource = GetComponent<AudioSource>();
        SetSoundToButtons();
    }

    private void Start()
    {
        _playerHealth = GameObject
            .FindWithTag("Player")
            .GetComponent<PlayerHealth>();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameOver += PlayGameOver;
        GameManager.Instance.OnGameFinish += PlayGameFinish;
        _playerHealth.OnPlayerHPChanged += PlayPlayerTakeDamage;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameOver -= PlayGameOver;
        GameManager.Instance.OnGameFinish -= PlayGameFinish;
        _playerHealth.OnPlayerHPChanged -= PlayPlayerTakeDamage;
    }

    #region Player

    public void PlayPlayerJump(int currentJumpCount)
    {
        AudioClip sound = currentJumpCount < _jumpSounds.Length ?
            _jumpSounds[currentJumpCount] : _jumpSounds[_jumpSounds.Length - 1];
        _sfxSource.PlayOneShot(sound);
    }

    private void PlayPlayerTakeDamage(int currentHP, int takenDamage)
    {
        _sfxSource.PlayOneShot(_takeDamage);
    }

    #endregion

    #region Game

    private void PlayGameOver() => _sfxSource.PlayOneShot(_gameOver);

    private void PlayGameFinish()
    {
        _sfxSource.PlayOneShot(_gameFinish);
        _sfxSource.PlayOneShot(_catPurring);
    }

    private void SetSoundToButtons()
    {
        foreach (var button in FindObjectsOfType<Button>(true))
            button.onClick.AddListener(PlayClickButton);
    }

    private void PlayClickButton() => _sfxSource.PlayOneShot(_clickButton);

    #endregion
}
