using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _menuUI;
    [SerializeField] private GameObject _playerHpUI;
    [SerializeField] private TextMeshProUGUI _titleText;
    [SerializeField] private TextMeshProUGUI _hpText;

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
        _playerHealth.OnPlayerTakeDamage += UpdatePlayerHPText;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= GameStart;
        _playerHealth.OnPlayerTakeDamage -= UpdatePlayerHPText;
    }

    private void GameStart()
    {
        ShowHUD();
        HideMenu();
        HideTitle();
    }

    private void ShowHUD()
    {
        _playerHpUI.SetActive(true);
    }

    private void UpdatePlayerHPText(int currentHP, int damageTaken) =>
        _hpText.text = $"{currentHP} x";

    private void ShowMenu() => _menuUI.SetActive(true);
    private void HideMenu() => _menuUI.SetActive(false);

    private void ShowTitle() => _titleText.gameObject.SetActive(true);
    private void HideTitle() => _titleText.gameObject.SetActive(false);
}
