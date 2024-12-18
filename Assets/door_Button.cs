using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class door_Button : MonoBehaviour
{
    [SerializeField]LayerMask playerLayer;
    [SerializeField]LayerMask corpseLayer;
    SpriteRenderer SpriteRenderer_Button;
    BoxCollider2D BoxCollider2D;
    private bool isPressed = false;
    [SerializeField]Sprite unpressed;
    [SerializeField] Sprite pressed;
    private GameObject detected;

    [SerializeField] GameObject doorOrPlatform;
    [SerializeField] private bool isActiveAtStart;


    private void Awake()
    {
        SpriteRenderer_Button = GetComponent<SpriteRenderer>();
        BoxCollider2D = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (((1 << collision.gameObject.layer) == playerLayer.value || (1 << collision.gameObject.layer) == corpseLayer.value) && !isPressed)
        {
            isPressed = true;
            //Debug.Log("Button pressed");
            SpriteRenderer_Button.sprite = pressed;
            detected = collision.gameObject;

            doorOrPlatform.SetActive(!isActiveAtStart);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject != null && collision.gameObject == detected)
        {
            isPressed = false;
            //Debug.Log("Button released");
            SpriteRenderer_Button.sprite = unpressed;

            doorOrPlatform.SetActive(isActiveAtStart);
        }
        
    }

}
