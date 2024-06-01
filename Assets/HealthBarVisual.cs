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

        for (int i = 0; i < healthSystem.GetHealthMax(); i++) {
            GameObject heart = Instantiate(heartFullPrefab);
            hearts.Add(heart);
            heart.transform.SetParent(this.transform);
            heart.transform.localScale = Vector3.one;
        }
    }

    private void HealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e) {
        for (int i = 0; i < e.damageAmount; i++) {
            Destroy(hearts[i]);
            hearts.RemoveAt(i);
        }
    }



}
