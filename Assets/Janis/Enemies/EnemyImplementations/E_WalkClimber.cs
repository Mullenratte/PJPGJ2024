using Janis.Enemies.MovementResolvers;
using UnityEngine;


namespace Janis.Enemies.EnemyImplementations
{
    public class E_WalkClimber : EnemyBase
    {
        public float InitialDirection = 1.0f;
        private void Awake()
        {
            RigidBody = GetComponent<Rigidbody2D>();
            movementResolver = new MR_BounceAtWall(InitialDirection, 0.6f);
        }
    }
}