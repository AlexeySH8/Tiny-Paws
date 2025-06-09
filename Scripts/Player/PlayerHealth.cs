using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public event Action<int, int> OnPlayerTakeDamage;

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
            OnPlayerTakeDamage.Invoke(currentHP, damage);
        }
    }

    public void FallDamage(int fallDamageUnits) => TakeDamage(_fallDamage * fallDamageUnits);

    protected override void ReactToDeath()
    {
        GameManager.Instance.GameOver();
    }

    private void EnableTakeDamage() => _canTakeDamage = true;

    private void DisableTakeDamage() => _canTakeDamage = false;
}
