using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{

    [SerializeField] private Player player;
    [SerializeField] private BoxCollider2D BoxCollider;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Entered");
    }
}
