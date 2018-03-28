﻿using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
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
using System.Windows.Threading;
using Controller.CanvasGrid;

namespace Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Logic _logic;
        private WorldGrid _worldGrid;
        public MainWindow()
        {
            InitializeComponent();
            
            // https://stackoverflow.com/questions/1695101/why-are-actualwidth-and-actualheight-0-0-in-this-case
            Loaded += delegate
            {
                var currentDispatcher = Application.Current.Dispatcher;
                _worldGrid = new WorldGrid(videoCanvas, new Vector2(8, 8), ref currentDispatcher);
                _logic = new Logic(ref _worldGrid);
            };
        }


        private void videoCanvas_MouseMove(object sender, MouseEventArgs e)
        {

        }

        private void videoCanvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
              _logic.CanvasClick(e.GetPosition(videoCanvas));
        }

        private void videoCanvas_MouseUp(object sender, MouseButtonEventArgs e)
        {

        }

        private void OnConfrimPath(object sender, RoutedEventArgs e)
        {
            _logic.ConfirmSelectedPath();
        }

        private void OnConnectServo0(object sender, RoutedEventArgs e)
        {
            _logic.SelectServoByName("192.168.1.2");
        }

        private void OnConnectServo1(object sender, RoutedEventArgs e)
        {
            _logic.SelectServoByName("192.168.1.3");
        }

        private void OnConnectServo2(object sender, RoutedEventArgs e)
        {
            _logic.SelectServoByName("192.168.1.4");

        }


    }
}
