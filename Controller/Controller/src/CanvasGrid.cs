using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Controller.CellData;

namespace Controller
{
    public class CanvasGrid
    {

        //TODO: In theory this should be based on vidoe footage.
        //Basically make CellSize based on video and then GridSize based on CellSize and Canvas Size.
        public ServoVec2<double> CellSize { get; private set; } = new ServoVec2<double>(0); 
        private ServoVec2<int> _gridSize = new ServoVec2<int>(8); 

        public List<List<GridCell>> GridCells { get; set; } = new List<List<GridCell>>();

        private Canvas _canvas;
        
        public CanvasGrid(Canvas canvas)
        {
            _canvas = canvas;
            CellSize.X = _canvas.ActualWidth / _gridSize.X;
            CellSize.Y = _canvas.ActualHeight / _gridSize.Y;
            for (int i = 0; i < _gridSize.X; i++)
            {
                List<GridCell> gridRow = new List<GridCell>();
                for (int j = 0; j < _gridSize.Y; j++)
                {
                    gridRow.Add(CreateGridCell(new ServoVec2<int>(i,j)));
                }
                GridCells.Add(gridRow);
            }
        }

        private GridCell CreateGridCell(ServoVec2<int> index)
        {
            Rectangle cell = new Rectangle();
            cell.Stroke = new SolidColorBrush(Colors.Black);
            // rect.Fill = new SolidColorBrush(Colors.Black);
            cell.Width = _canvas.ActualWidth / _gridSize.X;
            cell.Height = _canvas.ActualHeight / _gridSize.Y;
            Canvas.SetLeft(cell, index.X * cell.Width);
            Canvas.SetTop(cell, index.Y * cell.Height);
            _canvas.Children.Add(cell);
            CellContents cellContents = new EmptyCell(index);
            return new GridCell(cell, cellContents);
        }

        public CellContents GetCellContents(ServoVec2<int> index)
        {
            return GridCells[index.X][index.Y].CellContents;
        }

        public void PlaceServo(ServoVec2<int> index,string servoIP)
        {
            if (GetCellContents(index) is EmptyCell)
            {
                Servo newServo = new Servo(index, servoIP);
                GridCells[index.X][index.Y].CellContents = newServo;
                GridCells[index.X][index.Y].Representation.Fill = new SolidColorBrush(Colors.Purple);
                
            }
        }

        public void SetColor(ServoVec2<int> index, Color color)
        {
            GridCells[index.X][index.Y].Representation.Fill = new SolidColorBrush(color);
        }
    }
}