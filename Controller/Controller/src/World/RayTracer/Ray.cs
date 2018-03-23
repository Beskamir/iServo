using System.Numerics;

namespace Controller.World.RayTracer
{
    public class Ray
    {
        public Vector2 Origin;
        public Vector2 Direction;
        public float MaxLenght;

        public Ray(Ray oldRay)
        {
            Origin = oldRay.Origin;
            Direction = oldRay.Direction;
            MaxLenght = oldRay.MaxLenght;
        }
        
        public Ray(Vector2 origin, Vector2 direction, float maxLenght=float.MaxValue)
        {
            Origin = origin;
            Direction = direction;
            MaxLenght = maxLenght;
        }

        public Vector2 GetHit(float t)
        {
            return Origin + Direction * t;
        }

        public void MoveRay(Vector2 pos)
        {
            Origin -= pos;
        }
    }
}