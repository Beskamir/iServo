using System.Collections.Generic;
using System.Windows;

namespace Controller.CellData
{
    public class Servo : CellContents
    {
//        private Vector _position;
        private Queue<ServoVec2<int>> _destinations = new Queue<ServoVec2<int>>();
        public string ServoIP;
        
        public Servo(ServoVec2<int> initPosition,string servoIP):base(initPosition)
        {
            
        }

        public void AddDestinations(Queue<ServoVec2<int>>  nextDestinations)
        {
            
        }

        public void MoveTo(ServoVec2<int> newPosition)
        {
            
            
            
            if(Position.Equals(_destinations.Peek()))
            {
                _destinations.Dequeue();
            }
        }
    }
}