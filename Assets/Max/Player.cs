using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class Player : MonoBehaviour
{
    private InputActions input;
    private Vector3 moveDir;
    private Rigidbody2D rb;
    private GameObject pickedObject;
    public GameObject PickedObject {
        get {
            return this.pickedObject;
        }
        set {
            this.pickedObject = value;
            if (value == null) {
                this.isCarryingObject = false;
            } else {
                this.isCarryingObject = true;
            }
            if (value.TryGetComponent<IPickable>(out IPickable pickedObj)) {
                ApplyPickedObjectStatusChanges(pickedObj);
            }            
        }
    }
    private bool isCarryingObject;

    private float jumpHeightPenaltyMultiplier = 1f;
    private float walkSpeedPenaltyMultiplier = 1f;


    [SerializeField] private float acceleration;
    [SerializeField] private float velocityMax;
    [SerializeField] private float jumpStrength;
    [SerializeField] private LayerMask walkableLayer;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform grabHitbox;
    [SerializeField] private Transform carryTransform;

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
        input.Player.Grab.performed += Grab_performed;
    }

    private void Grab_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj) {
        TryGrab();
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
            if (moveDir.x > 0) {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            } else {
                transform.rotation = Quaternion.Euler(0f, 0, 0f);
            }
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
            rb.AddForce(moveDir * acceleration * Time.fixedDeltaTime * walkSpeedPenaltyMultiplier);
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
        rb.AddForce(jumpVec * jumpHeightPenaltyMultiplier);
        state = State.Airborne;
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (walkableLayer.value == 1<<collision.gameObject.layer) {
            state = State.Grounded;
        }
    }

    private void TryGrab() {
        GameObject pickableObject = Physics2D.OverlapCircle(grabHitbox.position, grabHitbox.localScale.magnitude).gameObject;

        if(this.PickedObject == null) {
            TryPickUpObject(pickableObject);
        } else {
            DropPickedObject();
        }        
    }

    private void TryPickUpObject(GameObject pickableObject) {
        if (pickableObject.GetComponent<IPickable>() != null) {
            this.PickedObject = pickableObject;

            pickableObject.transform.SetParent(carryTransform);
            pickableObject.transform.localPosition = new Vector3(0f, 0f, 0f);

            pickableObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D pickableObjectRigidbody);
            pickableObjectRigidbody.simulated = false;

            pickableObject.TryGetComponent<Collider2D>(out Collider2D coll);
            coll.enabled = false;
        }
    }

    private void DropPickedObject() {
        PickedObject.transform.SetParent(null);

        PickedObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D pickedObjectRigidbody);
        pickedObjectRigidbody.simulated = true;

        PickedObject.TryGetComponent<Collider2D>(out Collider2D coll);
        coll.enabled = true;
        this.PickedObject = null;
    }

    private void ApplyPickedObjectStatusChanges(IPickable pickedObject) {
        if (pickedObject != null) {
            SO_CorpseStats objectStats = pickedObject.GetCorpseStats();

            walkSpeedPenaltyMultiplier = objectStats.walkSpeedPenalty;
            jumpHeightPenaltyMultiplier = objectStats.jumpHeightPenalty;
            if (objectStats.isBlockingJump) {
                jumpHeightPenaltyMultiplier = 0f;
            }
        } else {
            walkSpeedPenaltyMultiplier = 1f;
            jumpHeightPenaltyMultiplier = 1f;
        }
        
    }

}



