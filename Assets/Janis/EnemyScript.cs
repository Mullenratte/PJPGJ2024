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


        private void Awake()
        {

        }

        private void Update()
        {
            // Resolve Movement
            Direction = Direction.normalized;
            transform.position = (Vector2)transform.position + (GetVelocity() * Time.deltaTime);
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

            if (Physics2D.Raycast(transform.position, Direction, 0.5f, GameConstants.WallCollisionMask))
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
    }
}