using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HealthBarVisual : MonoBehaviour
{
    [SerializeField] private HealthSystem healthSystem;
    [SerializeField] private GameObject heartFullPrefab, heartEmptyPrefab;

    private void Start() {
        healthSystem.OnDamaged += HealthSystem_OnDamaged;

        for (int i = 0; i < healthSystem.GetHealthMax(); i++) {
            GameObject heart = Instantiate(heartEmptyPrefab);
            heart.transform.SetParent(this.transform);
            heart.transform.localScale = Vector3.one;
        }
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e) {
        for (int i = 0; i < healthSystem.GetHealthNormalized(); i++) {
            GameObject heart = Instantiate(heartEmptyPrefab);
            heart.transform.SetParent(this.transform);
            heart.transform.localScale = Vector3.one;
        }
    }


}
