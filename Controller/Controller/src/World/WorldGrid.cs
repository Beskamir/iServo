

using System.Numerics;
using System.Security.Policy;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Controller.CanvasGrid
{
    public class WorldGrid
    {
        public Vector2 CellSize;
        public Vector2 GridSize;
        private Canvas _canvas;
        private Dispatcher _currentDispatcher;


        public WorldGrid(Canvas canvas, Vector2 canvasSize, ref Dispatcher currentDispatcher)
        {
            _canvas = canvas;
            GridSize = canvasSize;
            _currentDispatcher = currentDispatcher;
            CellSize = new Vector2(
                (float) (_canvas.ActualWidth / GridSize.X),
                (float) (_canvas.ActualHeight / GridSize.Y)
            );
            DrawGrid();
        }

        public void DrawGrid()
        {
            if (_currentDispatcher.CheckAccess())
            {
                DrawGridCells();
            }
            else {
                _currentDispatcher.Invoke(DrawGridCells);
            }
        }

        private void DrawGridCells()
        {
            _canvas.Children.Clear();
            for (int i = 0; i < GridSize.X; i++)
            {
                for (int j = 0; j < GridSize.Y; j++)
                {
                    DrawRectangleAt(new Vector2(i, j));
                }
            }
        }
        
        private void DrawRectangleAt(Vector2 index)
        {
            Rectangle cell = new Rectangle();
            cell.Stroke = new SolidColorBrush(Colors.Black);
            // rect.Fill = new SolidColorBrush(Colors.Black);
            cell.Width = CellSize.X;
            cell.Height = CellSize.Y;
            Canvas.SetLeft(cell, index.X * cell.Width);
            Canvas.SetTop(cell, index.Y * cell.Height);
            _canvas.Children.Add(cell);
        }
      
        public void DrawCircleAt(Vector2 pos, float radius, Color color)
        {
            
            if (_currentDispatcher.CheckAccess()) 
            {
                AddToCanvas(pos, radius, color);
            }
            else {
                _currentDispatcher.Invoke(()=>
                {
                    AddToCanvas(pos, radius, color);
                });
            }
        }

        private void AddToCanvas(Vector2 pos, float radius,  Color color)
        {
            pos *= CellSize;
            Path element = new Path();
            element.Fill = new SolidColorBrush(color);
            EllipseGeometry ellipseGeometry = new EllipseGeometry();
            ellipseGeometry.Center = new Point(pos.X,pos.Y);
            ellipseGeometry.RadiusX = radius * CellSize.X;
            ellipseGeometry.RadiusY = radius * CellSize.Y;
            element.Data = ellipseGeometry;

            //No idea how to make this work on any thread other than the main UI one
            _canvas.Children.Add(element); 
        }
    }
}