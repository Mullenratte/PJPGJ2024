using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private int health = 5;
    private int healthMax;

    public event EventHandler OnDeath;
    public event EventHandler<OnDamagedEventArgs> OnDamaged;

    public class OnDamagedEventArgs : EventArgs { public float damageAmount; }

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
