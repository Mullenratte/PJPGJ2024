using Janis.Enemies;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyBase : MonoBehaviour
{
    [SerializeField] private int healthPoints;
     public float MoveSpeed;

    protected MovementResolver movementResolver;
    public Rigidbody2D RigidBody { get; protected set; }
    public UnityEvent<GameObject> OnEntityHit;
    
    
    private void Awake()
    {
        RigidBody = GetComponent<Rigidbody2D>();
    }
    
    private void Start()
    {
        movementResolver?.StartMovement(this);
    }

    private void Update()
    {
        movementResolver?.CalculateLocationNextFrame(this);
    }
    
}
