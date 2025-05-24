using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : BaseHealth
{
    //public event Action<float> OnHPChanged;

    [SerializeField] private bool _canTakeDamage;
    [SerializeField] private int _fallDamage;

    public override void TakeDamage(int damage)
    {
        if (_canTakeDamage)
        {
            base.TakeDamage(damage);
            Debug.Log("Player get " + damage + " damage");
            //OnHPChanged.Invoke(GetCurrentHPAsPercantage());
        }
    }

    public void FallDamage(int fallDamageUnits) => TakeDamage(_fallDamage * fallDamageUnits);

    public override void ReactToDeath()
    {
        Debug.Log("Player is Dead");
    }

    private float GetCurrentHPAsPercantage() => (float)currentHealth / (float)maxHealth;
}
