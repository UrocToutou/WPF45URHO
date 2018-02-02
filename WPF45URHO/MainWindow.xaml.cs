﻿using System;
using System.Collections.Generic;
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
using Urho;

namespace WPF45URHO
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //var x = Urho.IO.FileSystem.ProgramDir; //+ $"Data
            RunGame();
        }
        async void RunGame()
        {
            //var app = await UrhoSurfaceCtrl.Show(typeof(GameApplication), new ApplicationOptions(assetsFolder: "Data"));
            var app = await UrhoSurfaceCtrl.Show(typeof(GameApplication), new ApplicationOptions());
            Urho.Application.InvokeOnMain(() => { /*app.DoSomeStuff();*/});
        }
    }
}