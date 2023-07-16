using System;
using System.Drawing;
using System.Windows.Controls;
using System.Windows.Media;

namespace Tetris_Wpf.ViewModels
{
    public class MyButton : ViewModelBase
    {
        public int X { get; set;}
        public int Y { get; set; }
        private SolidColorBrush _backColor;
        public SolidColorBrush BackColor { get { return _backColor;  } set { _backColor = value; OnPropertyChanged(); } }

        public MyButton(int xP, int yP, SolidColorBrush colorP)
        {
            this.X = xP;
            this.Y = yP;
            this._backColor = colorP;
        }
    }
}