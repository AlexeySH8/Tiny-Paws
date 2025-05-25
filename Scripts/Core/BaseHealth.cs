using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHP;

    private void Awake()
    {
        currentHP = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead()) return;

        currentHP -= damage;

        if (IsDead())
            ReactToDeath();
    }

    public abstract void ReactToDeath();

    private bool IsDead() => currentHP <= 0;
}
