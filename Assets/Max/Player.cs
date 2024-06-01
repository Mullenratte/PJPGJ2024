using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private InputActions input;
    private Vector3 moveDir;
    private Rigidbody2D rb;

    [SerializeField] private float acceleration;
    [SerializeField] private float velocityMax;
    [SerializeField] private float jumpStrength;
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private Animator anim;

    public event EventHandler<OnMovingSidewardsEventArgs> OnMovingSidewards;

    public class OnMovingSidewardsEventArgs : EventArgs {
        public float movementDirX;
    }

    public enum State { 
        Grounded,
        Airborne
    }

    private State state;

    private void Awake() {
        input = new InputActions();
        rb = GetComponent<Rigidbody2D>();

        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
        input.Player.Jump.performed += Jump_performed;
    }

    private void Start() {
        state = State.Grounded;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        TryJump();
    }



    private void Move_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        if (obj.ReadValue<Vector2>().magnitude != 0) {
            moveDir = obj.ReadValue<Vector2>();
        } 
    }

    private void Move_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        moveDir = Vector3.zero;
    }

    private void OnEnable() {
        input.Enable();
    }

    private void OnDisable() {
        input.Disable();
    }

    private void FixedUpdate() {
        if (Mathf.Abs(rb.velocity.x) < this.velocityMax) {
            rb.AddForce(moveDir * acceleration * Time.fixedDeltaTime);
        } else {
            rb.velocity = new Vector2(Mathf.Sign(rb.velocity.x) * velocityMax, rb.velocity.y);
        }
    }

    private void Update() {
        switch (state) {
            case State.Grounded:
                anim.SetBool("IsGrounded", true);
                break;
            case State.Airborne:
                anim.SetBool("IsGrounded", false);
                break;
            default:
                break;
        }

        if (Mathf.Abs(rb.velocity.x) > 0) {
            anim.SetBool("IsMovingSidewards", true);
            OnMovingSidewards?.Invoke(this, new OnMovingSidewardsEventArgs { movementDirX = rb.velocity.x });
        } else {
            anim.SetBool("IsMovingSidewards", false);
        }
    }

    private void TryJump() {
        switch (state) {
            case State.Grounded:
                Jump();
                break;
            case State.Airborne:
                // potential double jump mechanic
                break;
        }
    }

    private void Jump() {
        Vector2 jumpVec = new Vector2(0f, jumpStrength);
        rb.AddForce(jumpVec);
        state = State.Airborne;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (walkableLayer.value == 1<<collision.gameObject.layer) {
            state = State.Grounded;
        }
    }
}



