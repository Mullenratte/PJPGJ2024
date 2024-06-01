using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Player player;
    private SpriteRenderer spriteRenderer;

    private void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start() {
        player.OnMovingSidewards += Player_OnMovingSidewards;
    }

    private void Player_OnMovingSidewards(object sender, Player.OnMovingSidewardsEventArgs e) {
        if (e.movementDirX < 0) {
            spriteRenderer.flipX = false;
        } else {
            spriteRenderer.flipX = true;
        }
    }
}
