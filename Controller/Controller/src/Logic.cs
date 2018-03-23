using System;
using System.Collections.Generic;
using System.Numerics;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using Controller.CanvasGrid;
using Controller.World.Entities;
using Controller.World.RayTracer;

namespace Controller
{
    public class Logic
    {
        private float _roombaRadius = 0.5f;
        private float _moveEpisilon = 0.5f;
        
        private string _activeServoName = "";
        private int _activeServoIndex = -1;
        private bool _activeSelected = false;
        
        private List<Entity> _entities = new List<Entity>();
        private List<Servo> _servos = new List<Servo>();
        private bool _gridLockOn = false; 

        private WorldGrid _worldGrid;

        public Thread ServoThreadVar;
        /// <summary>
        /// sets up servo thread and world data
        /// </summary>
        /// <param name="worldGrid"></param>
        public Logic(ref WorldGrid worldGrid)
        {
            _worldGrid = worldGrid;
            
            //create thread to go through servo list and do waypoint moving
            ServoThreadVar = new Thread(ServoThreadLoop);
            ServoThreadVar.SetApartmentState(ApartmentState.MTA);
            ServoThreadVar.Start();
        }

        //Threaded function that will keep checking whether
        // a servo can and should move.
        private void ServoThreadLoop()
        {
            while (true)
            {
                _worldGrid.DrawGrid();
                CheckServos();
                Thread.Sleep(500);
            }
        }

        /// <summary>
        /// check whether the servos have pending waypoints
        /// and try to go to those waypoints
        /// </summary>
        private void CheckServos()
        {
            foreach (Servo servo in _servos)
            {
                DrawServo(servo);
                DrawWaypoints(servo);
                // Console.WriteLine(servo.Name);
                // Console.WriteLine(servo.Waypoints.Count);
                if (servo.Waypoints.Count > 0)
                {
                    PathDetails pathDetails = new PathDetails(servo,servo.Waypoints.Peek());
                    
                    // Console.WriteLine(servo.Name);
                    
                    if (CheckIfClear(servo, ref pathDetails))
                    {
                        //if clear move servo by some episilon
                        servo.MoveTo(pathDetails.MainRay.GetHit(_moveEpisilon));
                    }
                    else
                    {
                        //TODO: else create new node orthogonal to the blockage
                        
                    }
                }
            }
        }

        private void DrawServo(Servo servo)
        {   
            _worldGrid.DrawCircleAt(servo.Position,_roombaRadius,Colors.Green);
        }

        /// <summary>
        /// check ray against all other servos and entities
        /// </summary>
        /// <param name="originalServo"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        private bool CheckIfClear(Servo originalServo, ref PathDetails path)
        {
            //create ray using servo + next destination
            
            foreach (Entity entity in _entities)
            {
                path.BlockedMain |= entity.Intersection(path.MainRay, ref path.MainNear);
                path.BlockedSide0 |= entity.Intersection(path.SideRay0, ref path.SideNear0);
                path.BlockedSide1 |= entity.Intersection(path.SideRay1, ref path.SideNear1);
            }

            foreach (Servo servo in _servos)
            {
                if (!servo.Equals(originalServo))
                {
                    path.BlockedMain |= servo.Intersection(path.MainRay, ref path.MainNear);
                    // Console.WriteLine(path.clearMain);
                    path.BlockedSide0 |= servo.Intersection(path.SideRay0, ref path.SideNear0);
                    // Console.WriteLine(path.clearSide0);
                    path.BlockedSide1 |= servo.Intersection(path.SideRay1, ref path.SideNear1);
                    // Console.WriteLine(path.clearSide1);
                }
            }
            
            //Debug
            _worldGrid.DrawCircleAt(path.Offset0,0.1f,(Colors.DarkSlateGray));
            _worldGrid.DrawCircleAt(path.Offset1,0.1f,(Colors.Red));
            _worldGrid.DrawCircleAt(path.Origin,0.1f,(Colors.Fuchsia));


            bool isBlocked = path.BlockedMain || path.BlockedSide0 || path.BlockedSide1;
            Console.WriteLine(isBlocked);
            return !isBlocked;
        }


        /// <summary>
        /// Process clicking on the canvas.
        /// Can select servos, place waypoints and place new servos
        /// </summary>
        /// <param name="newClick"></param>
        public void CanvasClick(Point newClick)
        {
            Vector2 clickLoc = new Vector2(
                (float) newClick.X/_worldGrid.CellSize.X,
                (float) newClick.Y/_worldGrid.CellSize.Y
            );
            if (_gridLockOn)
            {
                //Move clickLoc to center of grid like squares.
                // Basically create an integer lattice out of
                // R2 that spans all real numbers.
                clickLoc.X = (float)((int) clickLoc.X + 0.5);
                clickLoc.Y = (float)((int) clickLoc.Y + 0.5);
            }
            if (_activeSelected)
            {
                if (_activeServoIndex == -1)
                {
                    CreateNewServo(clickLoc);
                }
                else
                {
                    // record destination points if in correct mode
                    _servos[_activeServoIndex].TempWaypoints.Add(clickLoc);
                    // _worldGrid.DrawCircleAt(clickLoc,_roombaRadius,Colors.Brown);
                }
            }
            else
            {
                SelectServoByIntersection(clickLoc);
            }
        }

        /// <summary>
        /// place new servo at mouse location
        /// </summary>
        /// <param name="clickLoc"></param>
        private void CreateNewServo(Vector2 clickLoc)
        {
            Servo newServo = new Servo(clickLoc,_roombaRadius,_activeServoName);
            _servos.Add(newServo);
            _activeServoIndex = _servos.Count - 1;
            _activeSelected = false;
        }

        /// <summary>
        /// Check if mouse is inside of servo circle
        /// </summary>
        /// <param name="clickLoc"></param>
        private void SelectServoByIntersection(Vector2 clickLoc)
        {
            //check for circle-point intersection to select roombas
            float closestDistance = float.MaxValue;
            for (int i = 0; i < _servos.Count; i++)
            {
                float distance = Vector2.Distance(clickLoc, _servos[i].Position);
                if (distance < _servos[i].Radius && distance < closestDistance)
                {
                    closestDistance = distance;
                    _activeSelected = true;
                    _activeServoIndex = i;
                    _activeServoName = _servos[i].Name;
                }
            }
        }

        /// <summary>
        /// select servo using the list on that side of the ui
        /// </summary>
        /// <param name="name"></param>
        public void SelectServoByName(string name)
        {
            _activeServoName = name;
            _activeServoIndex = -1;
            _activeSelected = true;
            for (int i = 0; i < _servos.Count; i++)
            {
                if (_servos[i].Name.Equals(name))
                {
                    _activeServoIndex = i;
                }
            }
        }

        /// <summary>
        /// confirm the waypoints so they get sent to the servo
        /// </summary>
        public void ConfirmSelectedPath()
        {
            // store waypoints in selected roomba
            if (_activeSelected && _activeServoIndex >= 0)
            {
                _activeSelected = false;
                foreach (Servo servo in _servos)
                {
                    servo.AddDestinations();
                }
            }
        }

        private void DrawWaypoints(Servo servo)
        {
            foreach (var tempWaypoint in servo.TempWaypoints)
            {
                _worldGrid.DrawCircleAt(tempWaypoint,_roombaRadius/2,Colors.Purple);
            }   
        }
    }
}