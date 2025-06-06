using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _mobileInputUI;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _playerHpUI;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private Button _skipCutsceneButton;

    private PlayerHealth _playerHealth;

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
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
        ShowHomePage();
    }

    private void Start()
    {
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += GameStart;
        GameManager.Instance.OnGamePause += GamePause;
        GameManager.Instance.OnGameResume += GameResume;
        _playerHealth.OnPlayerTakeDamage += UpdatePlayerHPText;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStart;
        GameManager.Instance.OnGamePause -= GamePause;
        GameManager.Instance.OnGameResume -= GameResume;
        _playerHealth.OnPlayerTakeDamage -= UpdatePlayerHPText;
    }

    private void GameStart()
    {
        ShowHUD();
        HideMenu();
        HideTitle();
        HideSkipCutsceneButton();
    }

    private void GamePause()
    {
        _pauseUI.SetActive(true);
        HideHUD();
        ShowBackground();
        ShowTitle();
    }

    private void GameResume()
    {
        _pauseUI.SetActive(false);
        ShowHUD();
        HideBackground();
        HideTitle();
    }

    public void ShowHomePage()
    {
        HideHUD();
        ShowMenu();
        ShowTitle();
    }

    public void HideHomePage()
    {
        HideMenu();
        HideTitle();
    }

    private void ShowHUD()
    {
        _pauseButton.gameObject.SetActive(true);
        _playerHpUI.SetActive(true);
#if UNITY_ANDROID || UNITY_IOS
        _mobileInputUI.SetActive(true);
#endif
    }

    private void HideHUD()
    {
        _pauseButton.gameObject.SetActive(false);
        _playerHpUI.SetActive(false);
#if UNITY_ANDROID || UNITY_IOS
        _mobileInputUI.SetActive(false);
#endif
    }

    private void UpdatePlayerHPText(int currentHP, int damageTaken) =>
        _hpText.text = $"{currentHP} x";

    private void ShowMenu() => _menuUI.SetActive(true);
    private void HideMenu() => _menuUI.SetActive(false);

    private void ShowTitle() => _titleText.gameObject.SetActive(true);
    private void HideTitle() => _titleText.gameObject.SetActive(false);

    private void ShowBackground() => _background.gameObject.SetActive(true);
    private void HideBackground() => _background.gameObject.SetActive(false);

    public void ShowSkipCutsceneButton() => _skipCutsceneButton.gameObject.SetActive(true);
    public void HideSkipCutsceneButton() => _skipCutsceneButton.gameObject.SetActive(false);
}
