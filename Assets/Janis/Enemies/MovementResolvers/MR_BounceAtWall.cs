using UnityEngine;

namespace Janis.Enemies.MovementResolvers
{
    public class MR_BounceAtWall : MovementResolver
    {
        private float _direction;
        private float _creatureWidth;
        private int layerMask = 1 << 6;
        
        public MR_BounceAtWall(float direction, float creatureWidth)
        {
            if (Mathf.Approximately(0.0f, direction))
            {
                direction = 1.0f;
            }
            
            _direction = direction;
            _creatureWidth = creatureWidth;
            
        }
        
        public override void CalculateLocationNextFrame(EnemyBase enemy)
        {
            if (enemy == null)
            {
                Debug.Log("Ficl");
                return;
            }
            
            var currentLocation = enemy.transform.position;
            


            var hit = Physics2D.Raycast(currentLocation, new Vector2(_direction, 0.0f), _creatureWidth, layerMask);
            
            if (hit.collider)
            {
                Debug.Log("Hit Something");
                _direction *= -1.0f;
                UpdateSpriteFacingDirection(enemy);
                enemy.RigidBody.velocity = new Vector2(_direction * enemy.MoveSpeed, 0.0f);
            }
        }

        private void UpdateSpriteFacingDirection(EnemyBase enemy)
        {
            enemy.transform.localScale = new Vector2(_direction, 1.0f);
        }

        public override void StartMovement(EnemyBase enemy)
        {
            enemy.RigidBody.velocity = new Vector2(_direction * enemy.MoveSpeed, 0.0f);
        }
    }
}