using UnityEngine;

public abstract class BaseHealth : MonoBehaviour
{
    [SerializeField] protected int maxHealth;
    protected int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public virtual void TakeDamage(int damage)
    {
        if (IsDead()) return;

        currentHealth -= damage;

        if (IsDead())
            ReactToDeath();
    }

    public abstract void ReactToDeath();

    private bool IsDead() => currentHealth <= 0;
}
