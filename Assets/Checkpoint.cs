using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Checkpoint : MonoBehaviour
{
    private Collider2D Trigger;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private GameObject onActivatedAudioPrefab;

    private bool isActivated; 
    private void Awake()
    {
        Trigger = GetComponent<Collider2D>();
        Trigger.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var Player) && isActivated == false)
        {
            var HS = Player.GetComponent<HealthSystem>();
            HS.Heal(HS.GetHealthMax());
            isActivated = true;
            sr.sprite = activatedSprite;
            sr.color = new Color(0, 1, .133f);
            Instantiate(onActivatedAudioPrefab, transform.position, Quaternion.identity);
        }
    }
}
