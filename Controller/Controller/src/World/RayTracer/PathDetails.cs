using System.Net.Mail;
using System.Numerics;
using Controller.World.Entities;

namespace Controller.World.RayTracer
{
    public class PathDetails
    {
        public float MainNear = float.MaxValue;
        public float SideNear0 = float.MaxValue;
        public float SideNear1 = float.MaxValue;

        public bool BlockedMain = false;
        public bool BlockedSide0 = false;
        public bool BlockedSide1 = false;
        
        public Vector2 Dir;
        public Vector2 Origin;
                        
        //Create two tangental points on the servo&destination
        public Vector2 OrthgonalDir; 
        public Vector2 Offset0;
        public Vector2 Offset1;

        public Ray MainRay; 
        public Ray SideRay0;
        public Ray SideRay1;
        
        public PathDetails(Servo servo, Vector2 destination)
        {
            Dir = Vector2.Normalize(servo.Waypoints.Peek() - servo.Position);
            Origin = servo.Position;
            OrthgonalDir = new Vector2(Dir.Y, -Dir.X);
            
            Offset0 = Vector2.Normalize(OrthgonalDir) * servo.Radius + Origin;
            Offset1 = Vector2.Normalize(OrthgonalDir) * -servo.Radius + Origin;

            float distance = Vector2.Distance(Origin, destination) + servo.Radius;
            MainRay = new Ray(Origin, Dir, distance);
            SideRay0 = new Ray(Offset0, Dir, distance);
            SideRay1 = new Ray(Offset1, Dir, distance);
        }

//        public void setOrigin(Vector2 newOrigin)
//        {
//            MainRay.Origin = newOrigin;
//            SideRay0.Origin = newOrigin;
//            SideRay1.Origin = newOrigin;
//            Offset0 -= Origin;
//            Offset1 -= Origin;
//            Origin = newOrigin;
//            Offset0 += newOrigin;
//            Offset1 += newOrigin;
//        }
    }
}