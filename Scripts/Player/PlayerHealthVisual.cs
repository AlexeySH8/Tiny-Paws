using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerHealthVisual : MonoBehaviour
{
    private Animator _animator;
    private PlayerHealth _playerHealth;
    private float _idleTimeNormalized;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _playerHealth = FindFirstObjectByType<PlayerHealth>();
    }

    private void Start()
    {
        _playerHealth.OnPlayerHPChanged += PlayerTakeDamage;
    }

    private void OnDisable()
    {
        _playerHealth.OnPlayerHPChanged -= PlayerTakeDamage;
    }

    private void PlayerTakeDamage(int currentHP, int damageTaken)
    {
        if (damageTaken < 3)
            _animator.Play("Dizziness");
        else if (damageTaken >= 3)
            _animator.Play("Squints");
    }

}
