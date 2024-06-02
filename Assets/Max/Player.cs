using Janis;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
            ApplyPickedObjectStatusChanges(pickedObject?.GetComponent<IPickable>());
        }
    }
    private bool isCarryingObject;

    private float jumpHeightPenaltyMultiplier = 1f;
    private float walkSpeedPenaltyMultiplier = 1f;
    private float dropObjectPositionXOffset;
    private HealthSystem healthSystem;


    [SerializeField] private float acceleration;
    [SerializeField] private float velocityMax;
    [SerializeField] private float jumpStrength;
    [SerializeField] private LayerMask[] walkableLayers;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform grabHitbox;
    [SerializeField] private Transform carryTransform;
    [SerializeField] private Transform groundCheckTransform;

    public enum State { 
        Grounded,
        Airborne
    }

    private State state;

    private void Awake() {
        input = new InputActions();
        rb = GetComponent<Rigidbody2D>();
        healthSystem = GetComponent<HealthSystem>();

        input.Player.Move.performed += Move_performed;
        input.Player.Move.canceled += Move_canceled;
        input.Player.Jump.performed += Jump_performed;
        input.Player.Grab.performed += Grab_performed;

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, System.EventArgs e) {
        Debug.Log("You died!");
        Destroy(gameObject);
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
                dropObjectPositionXOffset = 1f;
            } else {
                transform.rotation = Quaternion.Euler(0f, 0, 0f); 
                dropObjectPositionXOffset = -1f;
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
        Collider2D[] hits = Physics2D.OverlapBoxAll(groundCheckTransform.position, new Vector2(0.1f, 0.1f), 0);

        foreach (var hit in hits) {
            foreach (var layer in walkableLayers) {
                if (layer.value == (1 << hit.gameObject.layer)) {
                    this.state = State.Grounded;
                    break;
                } else {
                    this.state = State.Airborne;
                }
            }           
        }
    }

    private void OnDrawGizmos() {
        Gizmos.DrawWireCube(groundCheckTransform.position, new Vector2(0.1f, 0.1f));
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
        if (jumpHeightPenaltyMultiplier <= 0f) return;

        state = State.Airborne;
        Vector2 jumpVec = new Vector2(0f, jumpStrength);
        rb.AddForce(jumpVec * jumpHeightPenaltyMultiplier);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (enemyLayer.value == (1 << collision.gameObject.layer)) {
            collision.gameObject.TryGetComponent<EnemyScript>(out EnemyScript enemy);
            healthSystem.Damage(/*enemy.damage*/1);

        }
    }

    private void TryGrab() {
        GameObject pickableObject = null;
        Collider2D[] collisions = Physics2D.OverlapCircleAll(grabHitbox.position, 1f);
        foreach (var coll in collisions) {
            if (coll.GetComponent<IPickable>() != null) {
                pickableObject = coll.gameObject;
                break;
            }
        }
        

        if(this.PickedObject == null) {
            TryPickUpObject(pickableObject);
        } else {
            TryDropPickedObject();
        }        
    }

    private void TryPickUpObject(GameObject pickableObject) {
        if (pickableObject?.GetComponent<IPickable>() != null) {
            this.PickedObject = pickableObject;

            pickableObject.transform.SetParent(carryTransform);
            pickableObject.transform.localPosition = new Vector3(0f, 0f, 0f);

            pickableObject.TryGetComponent<Rigidbody2D>(out Rigidbody2D pickableObjectRigidbody);
            pickableObjectRigidbody.simulated = false;

            pickableObject.TryGetComponent<Collider2D>(out Collider2D coll);
            coll.enabled = false;
        }
    }

    private void TryDropPickedObject() {
        Vector2 dropOffPosition = new Vector2(carryTransform.position.x + dropObjectPositionXOffset, carryTransform.position.y);

        Collider2D[] hits = Physics2D.OverlapBoxAll(dropOffPosition, Vector2.one * .8f, 0);

        foreach (var hit in hits) {
            foreach (var layer in walkableLayers) {
                if (layer.value == (1 << hit.gameObject.layer)) {
                    return;
                }
            }
        }

        DropPickedObject(dropOffPosition);
    }

    private void DropPickedObject(Vector2 dropOffPosition) {
        PickedObject.transform.SetParent(null);
        PickedObject.transform.position = dropOffPosition;

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

    public State GetState() {
        return this.state;
    }

    public Vector2 GetVelocity() {
        return rb.velocity;
    }
}



