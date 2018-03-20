using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Controller.CellData;

namespace Controller
{
    public class Logic
    {
        private bool _addingServo = false;
        private string _newServoIP = "";

        private Servo _currentServo;
        
        private Queue<ServoVec2<int>> _tempPoints = new Queue<ServoVec2<int>>();
        
        private bool _servoSelected = false;
        
        
        private CanvasGrid _canvasGrid;

        public Logic(CanvasGrid canvasGrid)
        {
            _canvasGrid = canvasGrid;
        }

        public void TempClick(Point newClick)
        {
            ServoVec2<int> tempGridCoord = new ServoVec2<int>((int) (newClick.X / _canvasGrid.CellSize.X),
                (int) (newClick.Y / _canvasGrid.CellSize.Y));
            CellContents cellContents = _canvasGrid.GetCellContents(tempGridCoord);

            if (_addingServo && (cellContents is EmptyCell))
            {
                _canvasGrid.PlaceServo(tempGridCoord,_newServoIP);
                _addingServo = false;
            }

            else if (!_servoSelected && cellContents is Servo)
            {
                //This should select the servo at the location of a click
                _currentServo = cellContents as Servo;
                //If a servo's selected don't change it
                _servoSelected = true; 
            }
            else if (_servoSelected && cellContents is EmptyCell)
            {
                //todo: Maybe implement some kind of check so you can tell the servo to move to the same cell twice in a row?
                //Todo: maybe check that the tile is empty?
                _tempPoints.Enqueue(tempGridCoord);
                _canvasGrid.SetColor(tempGridCoord, Colors.Red);
            }
        }

        public void FinalizePath()
        {
            if (_servoSelected)
            {
                _servoSelected = false;
                _currentServo.AddDestinations(_tempPoints);
                _tempPoints.Clear();
            }
        }
//        public void 

        public void SelectRoomba(string servoIP)
        {
            //TODO check if the roomba's on the canvas
            _addingServo = true;
            _newServoIP = servoIP;
        }
    }
}