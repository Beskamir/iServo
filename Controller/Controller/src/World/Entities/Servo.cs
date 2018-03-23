using System;
using System.Collections.Generic;
using System.Numerics;
using Controller.Communication;

namespace Controller.World.Entities
{
    public class Servo : Entity
    {
        public string Name;
        private PythonCall _pythonCall = new PythonCall();
        public Queue<Vector2> Waypoints = new Queue<Vector2>();
        
        public List<Vector2> TempWaypoints = new List<Vector2>();

        
        public Servo(Vector2 position, float radius, string name) : base(position, radius)
        {
            Name = name;
        }

        public void AddDestinations()
        {
            foreach (Vector2 tempWaypoint in TempWaypoints)
            {
                Waypoints.Enqueue(tempWaypoint);
            }
            TempWaypoints = new List<Vector2>();
        }

        public void MoveTo(Vector2 destination)
        {
            string instructions = ":Name:" + Name;
            if (Position == Waypoints.Peek())
            {
                Waypoints.Dequeue();
            }
            else if (Vector2.Distance(Position, Waypoints.Peek()) < Radius)
            {
                instructions += ":Move:" + Waypoints.Peek().X + "," + Waypoints.Peek().Y;
                _pythonCall.send(instructions);
                Position = Waypoints.Peek();
                
            }
            else
            {
                instructions += ":Move:" + destination.X + "," + destination.Y;
                _pythonCall.send(instructions);
                Position = destination;
            }
        }
    }
}