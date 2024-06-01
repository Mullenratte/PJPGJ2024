using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Player player;
    private SpriteRenderer spriteRenderer;
    private Animator anim;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update() {
        if (Mathf.Abs(player.GetVelocity().x) > 0) {
            anim.SetBool("IsMovingSidewards", true);
        } else {
            anim.SetBool("IsMovingSidewards", false);
        }

        switch (player.GetState()) {
            case Player.State.Grounded:
                anim.SetBool("IsGrounded", true);
                break;
            case Player.State.Airborne:
                anim.SetBool("IsGrounded", false);
                break;
            default:
                break;
        }
    }
}
