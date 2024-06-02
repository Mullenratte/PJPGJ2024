using Janis;
using System;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    private InputActions input;

    public event EventHandler OnAttack;

    public GameObject attackHitbox;
    [SerializeField] private float hitboxRadius;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private int attackDamage = 1;

    private void Start() {
        input = new InputActions();
        input.Enable();

        input.Player.Attack.performed += Attack_performed;
    }

    private void Attack_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        OnAttack?.Invoke(this, EventArgs.Empty);
    }

    private void OnDisable() {
        input.Disable();
    }

    public void Attack() {
        Debug.Log("Attack Triggered");

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackHitbox.transform.position, hitboxRadius, enemyLayer);

        foreach (var enemy in enemies) {
            if (enemy.GetComponent<EnemyScript>() != null) {
                enemy.TryGetComponent<HealthSystem>(out HealthSystem enemyHealthSystem);
                enemyHealthSystem.Damage(attackDamage);
            }
        }
    }
}
