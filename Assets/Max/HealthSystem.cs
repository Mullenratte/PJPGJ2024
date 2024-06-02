using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 5;
    private int healthMax;

    public event EventHandler OnDeath;
    public event EventHandler<OnDamagedEventArgs> OnDamaged;
    public event EventHandler<OnHealedEventArgs> OnHealed;

    public class OnDamagedEventArgs : EventArgs { public float damageAmount; }
    public class OnHealedEventArgs : EventArgs { public float healAmount; }

    private void Awake() {
        healthMax = health;
    }

    public void Damage(int damageAmount) {
        health -= damageAmount;

        OnDamaged?.Invoke(this, new OnDamagedEventArgs { damageAmount = damageAmount});

        if (health <= 0) {
            health = 0;
            Die();
        }
    }

    public void Heal(int healAmount) {
        health += healAmount;

        if (health >= healthMax) {
            health = healthMax;
        }

        OnHealed?.Invoke(this, new OnHealedEventArgs { healAmount = healAmount });


    }

    private void Die() {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

    public float GetHealthNormalized() {
        return (float)this.health / healthMax;
    }

    public int GetHealth() {
        return health;
    }

    public int GetHealthMax() {
        return healthMax;
    }
}
