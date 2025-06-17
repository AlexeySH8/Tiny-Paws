using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public event Action<int, int> OnPlayerHPChanged;

    [SerializeField] private int _fallDamage;
    [SerializeField] private bool _canTakeDamage;

    private void Start()
    {
        DisableTakeDamage();
        SubscribeToEvents();
    }

    private void SubscribeToEvents()
    {
        GameManager.Instance.OnGameStart += EnableTakeDamage;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnGameStart -= EnableTakeDamage;
    }

    public override void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            base.TakeDamage(damage);
            OnPlayerHPChanged.Invoke(currentHP, damage);
        }
    }

    public void FallDamage(int fallDamageUnits) => TakeDamage(_fallDamage * fallDamageUnits);

    protected override void ReactToDeath()
    {
        AdManager.Instance.LaunchRewardedAd(rewardReceived =>
        {
            if (rewardReceived)
                ResetHealth();
            else
                GameManager.Instance.GameOver();
        });
    }

    protected override void ResetHealth()
    {
        base.ResetHealth();
        OnPlayerHPChanged.Invoke(currentHP, 0);
    }

    private void EnableTakeDamage() => _canTakeDamage = true;

    private void DisableTakeDamage() => _canTakeDamage = false;
}
