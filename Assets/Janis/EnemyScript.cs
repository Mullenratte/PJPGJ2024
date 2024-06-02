using Audio;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

namespace Janis
{
    public enum EMovementMode
    {
        WallBouncer, FlyingBat,
        
    }
    [RequireComponent(typeof(Collider2D))]
    public class EnemyScript : MonoBehaviour
    {
        private HealthSystem healthSystem;
        private EntitySounds entitySounds;

        [Header("Behaviour")]
        public UnityEvent<Collision2D> OnTouchedGameObject;
        public EMovementMode MovementMode = EMovementMode.WallBouncer;
        public Collider2D GroundCollider2D;

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
        [SerializeField] private GameObject onDeathAudioSourcePrefab;

        [Header("Visuals")]
        [SerializeField] private Animator anim;

        [Header("Connected")]
        [SerializeField] private Player player;

        private Vector2 spawn;
        private int state;

        private void Awake()
        {
            healthSystem = GetComponent<HealthSystem>();
            entitySounds = GetComponent<EntitySounds>();
            GroundCollider2D = GetComponent<Collider2D>();
            spawn = transform.position;
            state = 0; //idle
        }
        private void Start() {
            healthSystem.OnDamaged += HealthSystem_OnDamaged;
            healthSystem.OnDeath += HealthSystem_OnDeath;
        }

        private void HealthSystem_OnDamaged(object sender, HealthSystem.OnDamagedEventArgs e) {
            anim.SetTrigger("OnDamaged");
            entitySounds.PlayDamagedSound();
        }

        private void HealthSystem_OnDeath(object sender, EventArgs e) {
            Instantiate(corpsePrefab, this.transform.position, Quaternion.identity);
            Instantiate(onDeathAudioSourcePrefab, this.transform.position, Quaternion.identity);
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
                case EMovementMode.FlyingBat: return FlyingBat();
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

        private Vector2 FlyingBat()
        {
            switch (state)
            {
                case 0:
                    if (Math.Abs(spawn.x - transform.position.x) > 5)
                    {
                        Velocity.x = -Velocity.x;
                        Direction.x = -Direction.x;
                    }
                    if (Math.Abs(transform.position.x - player.transform.position.x) < 0.5)
                    {
                        Velocity.x = 0f;
                        Velocity.y = -10f;
                        state = 1;
                    }
                    break;
                case 1:
                    if (Physics2D.Raycast(transform.position, Vector2.down, 0.5f, GameConstants.WallCollisionMask))
                    {
                        Velocity.y = 20f;
                        transform.position = (Vector2)transform.position + Velocity;
                        Velocity.y = 0f;
                        state = 2;
                    }
                    break;
                default:
                    break;
            }
     

            return Velocity;
        }

        private void OnCollisionEnter2D(Collision2D other)
        {           
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