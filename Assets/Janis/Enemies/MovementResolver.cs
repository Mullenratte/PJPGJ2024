namespace Janis.Enemies
{
    public abstract class MovementResolver
    {
        public abstract void CalculateLocationNextFrame(EnemyBase enemy);
        public abstract void StartMovement(EnemyBase enemy);
    }
}