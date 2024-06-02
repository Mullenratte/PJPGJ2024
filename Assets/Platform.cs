using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private BoxCollider2D BoxCollider;

    private void Awake()
    {
        BoxCollider2D collider = GetComponent<BoxCollider2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == player.gameObject)
        {
            BoxCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        BoxCollider.enabled = true;
    }
}
