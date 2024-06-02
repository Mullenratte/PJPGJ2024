using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Janis
{
    public enum EMovementMode
    {
        WallBouncer,
        
    }
    [RequireComponent(typeof(Collider2D))]
    public class EnemyScript : MonoBehaviour
    {
        private HealthSystem healthSystem;

        [Header("Behaviour")]
        public UnityEvent<Collision2D> OnTouchedGameObject;
        public EMovementMode MovementMode = EMovementMode.WallBouncer;
        public Collider2D GroundCollider2D;
        public Transform StepHeight;

        [Header("Stats")]
        public float MoveSpeed = 1.0f;
        public float Gravity = 1.0f;
        public Vector2 Direction;
        public Vector2 Velocity;
        public bool stoppingAtGap;
        public int damage;

        [Header("Corpse")]
        [SerializeField] private GameObject corpsePrefab;
        [SerializeField] private LayerMask corpseLayer;

        [Header("Visuals")]
        [SerializeField] private Animator anim;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
        }
        private void Start() {
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnDeath += HealthSystem_OnDeath;
        }

        private void HealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e) {
            anim.SetTrigger("OnDamaged");
        }

        private void HealthSystem_OnDeath(object sender, EventArgs e) {
            Instantiate(corpsePrefab, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }

        private void Update()
        {
            // Resolve Movement
            Direction = Direction.normalized;
            transform.position = (Vector2)transform.position + (GetVelocity() * Time.deltaTime);

            if (Velocity.x > 0) {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            } else {
                transform.rotation = Quaternion.Euler(0f, 0, 0f);
            }

            if (Mathf.Abs(Velocity.x) > 0) {
                anim.SetBool("IsWalking", true);
            } else {
                anim.SetBool("IsWalking", false);
            }
        }

        private Vector2 GetVelocity()
        {
            switch (MovementMode)
            {
                case EMovementMode.WallBouncer: return WallBouncer();
                default: return Vector2.zero;
            }
        }

        private Vector2 WallBouncer()
        {
            // Base Velocity
            Velocity = (Direction * MoveSpeed + new Vector2(0f, -Gravity));

            if (Physics2D.Raycast(transform.position, Direction, 0.5f, GameConstants.WallCollisionMask) || (stoppingAtGap && isInFrontOfGap()) || isInFrontOfCorpse())
            {
                Velocity.x = -Velocity.x;
                Direction.x = -Direction.x;
            }
            
            if (Physics2D.Raycast(transform.position, Vector2.down, 0.5f, GameConstants.WallCollisionMask))
            {
                Velocity.y = 0f;
            }
            
            return Velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            switch (MovementMode)
            {
                case EMovementMode.WallBouncer:
                    if (other.GetContact(0).point.y > StepHeight.position.y)
                    {
                        Debug.Log("I Touched Grass");
                        Direction = new Vector2(-Direction.x , Direction.y);
                    }
                    break;
                    
                default: break; 
            }
            
            OnTouchedGameObject.Invoke(other);
        }

        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("I Touched Grass");
            Direction = new Vector2(-Direction.x , Direction.y);
        }

        private bool isInFrontOfGap()
        {
            return !Physics2D.Raycast((Vector2)transform.position + Direction * 0.5f, Vector2.down, 1.4f, GameConstants.WallCollisionMask);
        }

        private bool isInFrontOfCorpse()
        {
            return Physics2D.Raycast((Vector2)transform.position, Direction, 0.5f, corpseLayer.value);
        }
    }
}