using System;
using System.Numerics;
using Controller.World.RayTracer;

namespace Controller.World.Entities
{
    public class Entity
    {
        public Vector2 Position;
        public float Radius;

        public Entity(Vector2 position, float radius)
        {
            Position = position;
            Radius = radius;
        }

        public bool Intersection(Ray ray, ref float tNear)
        {
            Ray localRay = new Ray(ray);
            localRay.MoveRay(Position);

            float a = localRay.Direction.LengthSquared();
            float b = 2 * Vector2.Dot(localRay.Direction, localRay.Origin);
            float c = localRay.Origin.LengthSquared() - (float) Math.Pow(Radius,2);

            float t0 = 0;
            float t1 = 0;
            
            if (!SolveQuadratic(a, b, c, ref t0, ref t1))
            {
                return false;
            }

            float midPoint = (t0 + t1) / 2;

            if (midPoint < localRay.MaxLenght && midPoint > 0 && midPoint < tNear)
            {
                tNear = midPoint;
                return true;
            }
            
            return false;
        }

        private bool SolveQuadratic(float a, float b, float c, ref float t0, ref float t1)
        {
            float discriminant = (b*b) - (4 * a * c);
            if (discriminant < 0.0f){
                return false; //can't find real solution
            }
            // Find two points of intersection
            t0 = (-b - (float)Math.Sqrt(discriminant)) / (2 * a);
            t1 = (-b + (float)Math.Sqrt(discriminant)) / (2 * a);
            //printf("potentialHit");
            return true; //found a real solution
        }
    }
}