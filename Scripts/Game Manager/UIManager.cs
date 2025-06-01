using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private Image _background;
    [SerializeField] private GameObject _playerHpUI;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Button _pauseButton;
    [SerializeField] private GameObject _pauseUI;

    private PlayerHealth _playerHealth;

    private void Awake()
    {
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
        ShowMenu();
        ShowTitle();
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

    private void ShowHUD()
    {
        _pauseButton.gameObject.SetActive(true);
        _playerHpUI.SetActive(true);
    }

    private void HideHUD()
    {
        _pauseButton.gameObject.SetActive(false);
        _playerHpUI.SetActive(false);
    }

    private void UpdatePlayerHPText(int currentHP, int damageTaken) =>
        _hpText.text = $"{currentHP} x";

    private void ShowMenu() => _menuUI.SetActive(true);
    private void HideMenu() => _menuUI.SetActive(false);

    private void ShowTitle() => _titleText.gameObject.SetActive(true);
    private void HideTitle() => _titleText.gameObject.SetActive(false);

    private void ShowBackground() => _background.gameObject.SetActive(true);
    private void HideBackground() => _background.gameObject.SetActive(false);
}
