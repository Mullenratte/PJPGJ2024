using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class door_Button : MonoBehaviour
{
    [SerializeField]LayerMask playerLayer;
    [SerializeField]LayerMask enemyLayer;
    SpriteRenderer SpriteRenderer_Button;
    BoxCollider2D BoxCollider2D;
    private bool isPressed = false;
    [SerializeField]Sprite unpressed;
    [SerializeField] Sprite pressed;
    private GameObject detected;

    [SerializeField] GameObject door;


    private void Awake()
    {
        SpriteRenderer_Button = GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((1 << collision.gameObject.layer) == playerLayer.value || (1 << collision.gameObject.layer) == enemyLayer.value)
        {
            isPressed = true;
            //Debug.Log("Button pressed");
            SpriteRenderer_Button.sprite = pressed;
            detected = collision.gameObject;

            door.SetActive(false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == detected)
        {
            isPressed = false;
            //Debug.Log("Button released");
            SpriteRenderer_Button.sprite = unpressed;

            door.SetActive(true);
        }
        
    }

}
