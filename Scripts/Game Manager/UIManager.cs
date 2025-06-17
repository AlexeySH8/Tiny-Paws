using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private GameObject _mobileInputUI;
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private Image _screenDimming;
    [SerializeField] private GameObject _playerHpUI;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _pauseUI;
    [SerializeField] private Button _skipCutsceneButton;
    [SerializeField] private GameObject _adPanel;
    [SerializeField] private Image _adTimerFilling;

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
        ShowHomePage();
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
        GameManager.Instance.OnGameStart += GameStart;
        GameManager.Instance.OnGamePause += GamePause;
        GameManager.Instance.OnGameResume += GameResume;
        _playerHealth.OnPlayerHPChanged += UpdatePlayerHPText;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStart;
        GameManager.Instance.OnGamePause -= GamePause;
        GameManager.Instance.OnGameResume -= GameResume;
        _playerHealth.OnPlayerHPChanged -= UpdatePlayerHPText;
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
        ScreenDimmingOn();
        ShowTitle();
    }

    private void GameResume()
    {
        _pauseUI.SetActive(false);
        ShowHUD();
        ScreenDimmingOff();
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

    public void ShowAdPanel()
    {
        ScreenDimmingOn();
        HideHUD();
        _adPanel.SetActive(true);
    }

    public void HideAdsPanelUI()
    {
        ScreenDimmingOff();
        ShowHUD();
        _adPanel.SetActive(false);
    }

    private void ShowHUD()
    {
        _pauseButton.gameObject.SetActive(true);
        _playerHpUI.SetActive(true);
        if (Application.isMobilePlatform)
            _mobileInputUI.SetActive(true);
    }

    private void HideHUD()
    {
        _pauseButton.gameObject.SetActive(false);
        _playerHpUI.SetActive(false);
        if (Application.isMobilePlatform)
            _mobileInputUI.SetActive(false);
    }

    public void UpdateAdTimer(float time) => _adTimerFilling.fillAmount = time;

    private void UpdatePlayerHPText(int currentHP, int damageTaken) =>
        _hpText.text = $"{currentHP} x";

    private void ShowMenu() => _menuUI.SetActive(true);
    private void HideMenu() => _menuUI.SetActive(false);

    private void ShowTitle() => _titleText.gameObject.SetActive(true);
    private void HideTitle() => _titleText.gameObject.SetActive(false);

    private void ScreenDimmingOn() => _screenDimming.gameObject.SetActive(true);
    private void ScreenDimmingOff() => _screenDimming.gameObject.SetActive(false);

    public void ShowSkipCutsceneButton() => _skipCutsceneButton.gameObject.SetActive(true);
    public void HideSkipCutsceneButton() => _skipCutsceneButton.gameObject.SetActive(false);
}
