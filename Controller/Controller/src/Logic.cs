using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Numerics;
using System.Runtime.Serialization;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Xaml.Schema;
using Controller.CanvasGrid;
using Controller.World.Entities;
using Controller.World.RayTracer;

namespace Controller
{
    public class Logic
    {
        private float _roombaRadius = 0.5f;
        private float _moveEpisilon = 0.5f;
        
        //private string _activeServoName = "";
        private int _activeServoIndex = -1;
        private int _pendingServoID = -1;
        private bool _activeSelected = false;

        private List<Entity> _entities = new List<Entity>();
        private List<Servo> _servos = new List<Servo>();
        private List<Person> _people = new List<Person>();

        private WorldGrid _worldGrid;

        public Thread ServoThreadVar;
        public bool ResetLocation { get; set; } = false;
        public bool GridLockOn { get; set; } = false;
        public bool SetHome { get; set; } = false;
        public bool AddPerson { get; set; } = false;
        public bool RemovePerson { get; set; } = false;

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
                DrawPeople();
                CheckServos();
                Thread.Sleep(500);
            }
        }

        private void DrawPeople()
        {
            foreach (Person person in _people)
            {
                _worldGrid.DrawCircleAt(person.Position, _roombaRadius, Colors.Black);
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
            _worldGrid.DrawCircleAt(servo.Position, _roombaRadius,
                servo.IsActive ? Colors.LightGreen : Colors.DarkGreen);
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
                if (!entity.Equals(originalServo))
                {
                    path.BlockedMain |= entity.Intersection(path.MainRay, ref path.MainNear);
                    path.BlockedSide0 |= entity.Intersection(path.SideRay0, ref path.SideNear0);
                    path.BlockedSide1 |= entity.Intersection(path.SideRay1, ref path.SideNear1);
                }
            }

            foreach (Person person in _people)
            {
                path.BlockedMain |= person.Intersection(path.MainRay, ref path.MainNear);
                path.BlockedSide0 |= person.Intersection(path.SideRay0, ref path.SideNear0);
                path.BlockedSide1 |= person.Intersection(path.SideRay1, ref path.SideNear1);
            }
            
            //Debug
            _worldGrid.DrawCircleAt(path.Offset0,0.1f,(Colors.DarkSlateGray));
            _worldGrid.DrawCircleAt(path.Offset1,0.1f,(Colors.Red));
            _worldGrid.DrawCircleAt(path.Origin,0.1f,(Colors.Fuchsia));


            bool isBlocked = path.BlockedMain || path.BlockedSide0 || path.BlockedSide1;
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
            if (GridLockOn)
            {
                //Move clickLoc to center of grid like squares.
                // Basically create an integer lattice out of
                // R2 that spans all real numbers.
                clickLoc.X = (float)((int) clickLoc.X + 0.5);
                clickLoc.Y = (float)((int) clickLoc.Y + 0.5);
            }

            if (AddPerson)
            {
                Person person = new Person(clickLoc,_roombaRadius);
                _people.Add(person);
            }
            else if (RemovePerson)
            {
                RemovePersonByIntersection(clickLoc);
            }
            else
            {
                if (_activeSelected)
                {
                    if (_activeServoIndex == -1)
                    {
                        CreateNewServo(clickLoc);
                    }
                    else if (ResetLocation)
                    {
                        _servos[_activeServoIndex].ResetRealLocation(clickLoc);
                    }
                    else if (SetHome)
                    {
                        _servos[_activeServoIndex].HomeVector2 = clickLoc;
                    }
                    else
                    {
                        // record destination points if in correct mode
                        _servos[_activeServoIndex].TempWaypoints.Add(clickLoc);
                        _servos[_activeServoIndex].TempWaypoints.Add(clickLoc);
                        // _worldGrid.DrawCircleAt(clickLoc,_roombaRadius,Colors.Brown);
                    }
                }
                else if (SetHome)
                {
                    foreach (Servo servo in _servos)
                    {
                        servo.HomeVector2 = clickLoc;
                    }
                }
                else
                {
                    SelectServoByIntersection(clickLoc);
                } 
            }
        }

        private void RemovePersonByIntersection(Vector2 clickLoc)
        {
            //check for circle-point intersection to select roombas
            float closestDistance = float.MaxValue;
            int j = -1;
            
            for (int i = 0; i < _people.Count; i++)
            {
                float distance = Vector2.Distance(clickLoc, _people[i].Position);
                if (distance < _people[i].Radius && distance < closestDistance)
                {
                    closestDistance = distance;
                    j = i;
                }
            }

            if (j >= 0)
            {
                _people.RemoveAt(j);
            }
        }


        /// <summary>
        /// place new servo at mouse location
        /// </summary>
        /// <param name="clickLoc"></param>
        private void CreateNewServo(Vector2 clickLoc)
        {
            Servo newServo = new Servo(clickLoc,_roombaRadius,_pendingServoID);
            _servos.Add(newServo);
            _entities.Add(newServo);
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
                    _servos[_activeServoIndex].IsActive = true;
                    _pendingServoID = _servos[i].ID;
                    //_activeServoName = _servos[i].Name;
                }
            }
        }

        /// <summary>
        /// select servo using the list on that side of the ui
        /// </summary>
        /// <param name="name"></param>
        public void SelectServoByName(int id)
        {
            if (_activeServoIndex >= 0)
            {
                //remove ui selected coloring
                _servos[_activeServoIndex].IsActive = false;
            }
            _pendingServoID = id;
            _activeSelected = true;
            _activeServoIndex = id < _servos.Count ? id : -1;
            if (_activeServoIndex >= 0)
            {
                //enable ui selected coloring
                _servos[_activeServoIndex].IsActive = true;
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
                _servos[_activeServoIndex].IsActive = false;
                //Allow giving a new path to servo
                _servos[_activeServoIndex].Waypoints.Clear();
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
                _worldGrid.DrawCircleAt(tempWaypoint, _roombaRadius / 2, Colors.MediumPurple);
            }

            foreach (var tempWaypoint in servo.Waypoints)
            {
                _worldGrid.DrawCircleAt(tempWaypoint,_roombaRadius/2,Colors.Purple);
            }
        }

        public void Notify(int notification)
        {
            if (_activeSelected)
            {
                Servo activeServo = _servos[_activeServoIndex];
                foreach (Entity entity in _entities)
                {
                    bool difEntity = !entity.Equals(activeServo);

                    double distance = Vector2.Distance(entity.Position, activeServo.Position);
                    double radi = (entity.Radius + activeServo.Radius * 2);
                    if (difEntity && distance < radi)
                    {
                        if (notification > 2)
                        {
                            notification = 2;
                        }
                    }
                }
                activeServo.Notify(notification);
            }
        }

        public void RecallSelected()
        {
            _servos[_activeServoIndex].ReturnHome();
        }

        public void RecallAll()
        {
            foreach (Servo servo in _servos)
            {
                servo.ReturnHome();
            }
        }

        public void Stop()
        {
            if (_activeServoIndex >= 0)
            {
                _servos[_activeServoIndex].Waypoints.Clear();
            }
            else
            {
                foreach (Servo servo in _servos)
                {
                    servo.Waypoints.Clear();
                }
            }
        }
    }
}