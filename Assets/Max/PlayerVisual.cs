using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private GameObject onHurtAudioPrefab;
    [SerializeField] private GameObject onLandedAudioPrefab;
    [SerializeField] private GameObject[] onJumpedAudioPrefabs;
    private PlayerAttack playerAttack;
    private Animator anim;

    private void Awake() {
        anim = GetComponent<Animator>();
        playerAttack = GetComponent<PlayerAttack>();
    }

    private void Start() {
        playerAttack.OnAttack += PlayerAttack_OnAttack;
        player.OnDamaged += Player_OnDamaged;
        player.OnJumped += Player_OnJumped;
        player.OnLanded += Player_OnLanded;
    }

    private void Player_OnLanded() {
        Debug.Log("landed!");
        Instantiate(onLandedAudioPrefab, transform.position, Quaternion.identity);

    }

    private void Player_OnJumped() {
        int rnd = Random.Range(0, onJumpedAudioPrefabs.Length);
        Instantiate(onJumpedAudioPrefabs[rnd], transform.position, Quaternion.identity);
    }

    private void Player_OnDamaged() {
        anim.SetTrigger("OnDamaged");
        Instantiate(onHurtAudioPrefab, transform.position, Quaternion.identity);
    }

    private void PlayerAttack_OnAttack(object sender, System.EventArgs e) {
        anim.SetTrigger("OnAttack");
    }

    private void Update() {
        if(player.PickedObject != null) {
            anim.SetBool("IsCarrying", true);
        } else {
            anim.SetBool("IsCarrying", false);
        }

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
