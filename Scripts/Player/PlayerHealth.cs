using System;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    public event Action<int, int> OnPlayerTakeDamage;

    [SerializeField] private bool _canTakeDamage;
    [SerializeField] private int _fallDamage;

    public override void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            base.TakeDamage(damage);
            OnPlayerTakeDamage.Invoke(currentHP, damage);
        }
    }

    public void FallDamage(int fallDamageUnits) => TakeDamage(_fallDamage * fallDamageUnits);

    public override void ReactToDeath()
    {
        Debug.Log("Player is Dead");
    }
}
