using System;
using System.Collections.Generic;
using System.Numerics;
using Controller.Communication;

namespace Controller.World.Entities
{
    public class Servo : Entity
    {
//        public string Name;
        private PythonCall _pythonCall = new PythonCall();
        public int ID;

        public Queue<Vector2> Waypoints { get; set; } = new Queue<Vector2>();
        public List<Vector2> TempWaypoints { get; set; } = new List<Vector2>();
        public Vector2 HomeVector2 { get; set; } = new Vector2(0);
        public bool IsActive { get; set; }

        public Servo(Vector2 position, float radius, int id) : base(position, radius)
        {
            ID = id;
            ResetRealLocation(Position);
        }

        public void ResetRealLocation(Vector2 setPos)
        {
            Position = setPos;
            string instructions = ":Name("+ID + ")" +":ResetLocation(" + setPos.X + "," + setPos.Y + ")";
            _pythonCall.Send(instructions);
        }

        public void AddDestinations()
        {
            foreach (Vector2 tempWaypoint in TempWaypoints)
            {
                Waypoints.Enqueue(tempWaypoint);
            }
            TempWaypoints = new List<Vector2>();
        }

        public void MoveTo(Vector2 nextEpisilon, double speed)
        {
            string instructions = ":Name(" + ID + ")";

            // Check we're overshooting our destination.
            // Basically if vectors are pointing in the same direction everything's still alright but as soon as they're opposite we can't move forward anymore.
            if (Vector2.Dot((Waypoints.Peek() - Position), (Waypoints.Peek() - nextEpisilon)) > 0)
            {
                Position = nextEpisilon;
            }
            else
            {
                Position = Waypoints.Peek();
                Waypoints.Dequeue();
            }

            instructions += ":Move(" + Position.X + "," + Position.Y + ")" + 
                ":Speed(" + speed + ")";
            _pythonCall.Send(instructions);
        }

        public void Notify(int notification)
        {
            string instruction = ":Name(" + ID + ")" + ":Notify(" + notification + ")";
            _pythonCall.Send(instruction);
        }

        public void ReturnHome()
        {
            Waypoints.Clear();
            Waypoints.Enqueue(HomeVector2);
            TempWaypoints.Clear();
        }
    }
}
