using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBarVisual : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private GameObject heartFullPrefab;

    private List<GameObject> hearts;

    private void Awake() {
        hearts = new List<GameObject>();
    }

    private void Start() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;

        UpdateUI();
    }

    private void HealthSystem_OnHealed(object sender, HealthSystem.OnHealedEventArgs e) {
        UpdateUI();
    }

    private void HealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e) {
        UpdateUI();
    }

    private void UpdateUI() {
        foreach (var heart in hearts) {
            Destroy(heart);
        }

        hearts = new List<GameObject>();

        for (int i = 0; i < healthSystem.GetHealth(); i++) {
            GameObject heart = Instantiate(heartFullPrefab);
            hearts.Add(heart);
            heart.transform.SetParent(this.transform);
            heart.transform.localScale = Vector3.one;
        }
    }



}
