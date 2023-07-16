using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Tetris_Wpf.ViewModels;
using Tetris_Wpf.Model;
using Tetris_Wpf.Persistence;
using Microsoft.Win32;

namespace Tetris_Wpf
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private GameModel _model;
        private MainWindow _mainWindow;
        private ViewModel _viewModel;
        private ITetrisDataAccess _dataAccess;

        public App()
        {
            Startup += App_Startup;
            //same as Startup += new EventHandler(App_Startup);
        }

        private void App_Startup(object sender, StartupEventArgs e)
        {
            //modell 
            //TetrisFileDataAccess t = null;
            _model = new GameModel(_dataAccess); //width kell majd TODO
            _model._dataAccess = new TetrisFileDataAccess(_model.width);

            //viewModel
            _viewModel = new ViewModel(_model); 
            _mainWindow = new MainWindow()
            {
                DataContext = _viewModel
            };
            _mainWindow.Show();
        }
    }
}
