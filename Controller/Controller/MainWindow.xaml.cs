using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Controller.CellData;

namespace Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
//        Rectangle gridCells[][];
//        private CanvasGrid _canvasGrid;
        private Logic _logic;
        public MainWindow()
        {
            InitializeComponent();
            
            // https://stackoverflow.com/questions/1695101/why-are-actualwidth-and-actualheight-0-0-in-this-case
            Loaded += delegate
            {
//                _canvasGrid = new CanvasGrid(videoCanvas);
                _logic = new Logic(new CanvasGrid(videoCanvas));
                //canvasGrid[0][0].Fill = new SolidColorBrush(Colors.Red);
            };
            
        }

        private void videoCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void videoCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _logic.TempClick(e.GetPosition(videoCanvas));
//            Console.WriteLine(e.GetPosition(videoCanvas));
//            Console.WriteLine(videoCanvas.Width);
//            Console.WriteLine(videoCanvas.ActualWidth);
        }

        private void videoCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnConfrimPath(object sender, RoutedEventArgs e)
        {
            _logic.FinalizePath();
        }

        private void OnConnectServo0(object sender, RoutedEventArgs e)
        {
            _logic.SelectRoomba("192.168.1.2");
        }

        private void OnConnectServo1(object sender, RoutedEventArgs e)
        {
            _logic.SelectRoomba("192.168.1.3");
        }

        private void OnConnectServo2(object sender, RoutedEventArgs e)
        {
            _logic.SelectRoomba("192.168.1.4");
        }
    }
}
