using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Tetris_Wpf.Model;
using Tetris_Wpf.Persistence;

namespace Tetris_Wpf.ViewModels
{
    class ViewModel : ViewModelBase
    {

        #region Variables
        private OpenFileDialog _openFileDialog;
        private SaveFileDialog _saveFileDialog;

        public ObservableCollection<MyButton> Elements { get; set; }
        private GameModel _model;
        public bool IsPaused { get; set; }
        public String Time { get { return (_model.ellapsedTime).ToString("");  } }
        private int width;
        private int height;
        public int Width { get { return width; } set { width = value; OnPropertyChanged(); } }
        public int Height { get { return height; } set { height = value; OnPropertyChanged(); } }
        #endregion

        #region DelegatCommands
        public DelegateCommand Move { get; set; }
        public DelegateCommand Pause { get; set; }
        public DelegateCommand NewGameCommand { get; set; }
        public DelegateCommand SaveCommand { get; set; }
        public DelegateCommand LoadCommand { get; set; }
        #endregion

        #region Constructor
        public ViewModel(GameModel model)
        {
            _model = model;
            _openFileDialog = new OpenFileDialog();
            _openFileDialog.Filter = ".txt | *.txt";
            _saveFileDialog = new SaveFileDialog();
            _saveFileDialog.Filter = ".txt | *.txt";

            Elements = new ObservableCollection<MyButton>();
            IsPaused = false;
            Move = new DelegateCommand((_) => { return !IsPaused; }, Moving);
            Pause = new DelegateCommand(Pausing);

            NewGameCommand = new DelegateCommand((_) => { return !IsPaused; }, NewGaming);
            SaveCommand = new DelegateCommand(Saving);
            LoadCommand = new DelegateCommand(Loading);

            Refresh();
            _model.gameOver += ongameOver;
            _model.refreshTable += UpdateTable;
        }
        #endregion

        #region DelegateCommand: Pausing, Loading, Saving
        private void Pausing(object obj)
        {
            IsPaused = !IsPaused;
            if (_model.myTimer.IsEnabled)
                _model.myTimer.Stop();
            else
                _model.myTimer.Start();
        }

        private async void Loading(object obj)
        {
            _model.myTimer.Stop();
            if (_openFileDialog.ShowDialog() == true)
            {
                try
                {
                    await _model.LoadGameAsync(_openFileDialog.FileName);
                    IsPaused = false;
                    Width = _model.width;
                    Refresh();
                    _model.myTimer.Start();
                }
                catch
                {
                    MessageBox.Show("A játék betöltése sikertelen!");
                }
            }
        }

        private async void Saving(object obj)
        {
            _model.myTimer.Stop();
            if (_saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    _model._dataAccess = new TetrisFileDataAccess(_model.width);
                    await _model.SaveGameAsync(_saveFileDialog.FileName);
                    IsPaused = false;
                    _model.myTimer.Start();
                }
                catch
                {
                    MessageBox.Show("A játék mentése sikertelen!");
                }
            }
        }
        #endregion

        #region DelegateCommand: NewGaming, Moving        
        private void NewGaming(object obj)
        {
            //params: 4: New4x16Game; 8: New8x16Game; 12: New12x16Game
            int par = Convert.ToInt32(obj);
            switch(par)
            {
                case 4: _model.newGame(4);
                        Debug.WriteLine("new 4x16 game");
                        Refresh();
                        break;
                case 8: _model.newGame(8);
                        Debug.WriteLine("new 8x16 game");
                        Refresh();
                        break;
                case 12: _model.newGame(12);
                         Debug.WriteLine("new 12x16 game");
                        Refresh();
                        break;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void Moving(object obj)
        {
            //params: 0: A-Left; 1: D-Right; 2: W-Rotate
            int par = Convert.ToInt32(obj);
            switch (par)
            {
                case 0: _model.Move(Directions.Direction.Left); Debug.WriteLine("Moving: Left"); break;
                case 1: _model.Move(Directions.Direction.Right); Debug.WriteLine("Moving: Right"); break;
                case 2: _model.Move(Directions.Direction.Rotate); Debug.WriteLine("Moving: Rotate"); break;
                default: throw new ArgumentOutOfRangeException();
            }
            Refresh();
        }
        #endregion

        #region For Modell gameOver signal: ongameOver
        private void ongameOver(object sender, EventArgs e)
        {
            _model.myTimer.Stop();
            MessageBox.Show("Vége a játéknak, vesztettél!");
            _model.newGame(4);
        }
        #endregion

        #region For Modell refreshTable signal: UpdateTable
        private void UpdateTable(object sender, EventArgs e)
        {
            int k = 0;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    SolidColorBrush b = new SolidColorBrush();
                    switch (_model.bgGround[i, j])
                    {
                        case 0:
                            b = Brushes.White;
                            break;
                        case 1:
                            b = Brushes.Blue;
                            break;
                        case 2:
                            b = Brushes.Green;
                            break;
                    }
                    Elements[k].BackColor = b;
                    ++k;
                }
            }

            OnPropertyChanged("MyButton");
            OnPropertyChanged("Time");
            OnPropertyChanged("Time");
        }
        #endregion

        #region Refresh
        private void Refresh()
        {
            //buttongrid refresh
            Width = _model.width;
            Height = _model.height;

            Elements.Clear();

            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    SolidColorBrush b = new SolidColorBrush();
                    switch (_model.bgGround[i,j])
                    {
                        case 0:
                            b = Brushes.White;
                            break;
                        case 1:
                            b = Brushes.Blue;
                            break;
                        case 2:
                            b = Brushes.Green;
                            break;
                    }
                    Elements.Add(new MyButton(i,j,b));
                }

            }
            OnPropertyChanged("Time");
        }
        #endregion
    }
}
