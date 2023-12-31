﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Tetris_Wpf.Persistence
{
    public class TetrisTable
    {
        public int width { get; set; }
        public int[,] nextPiece { get; set; }
        public String nextPieceName { get; set; }

        public Point nextPieceCoord1 { get; set; }
        public Point nextPieceCoord2 { get; set; }
        public Point nextPieceCoord3 { get; set; }
        public Point nextPieceCoord4 { get; set; }
        public int rotateNumber { get; set; }

        public int[,] bgGround { get; set; }

        public TetrisTable(int width, int[,] nextPiece, string nextPieceName, Point nextPieceCoord1, Point nextPieceCoord2, Point nextPieceCoord3, Point nextPieceCoord4, int rotateNumber, int[,] bgGround)
        {
            this.width = width;
            this.nextPiece = nextPiece;
            this.nextPieceName = nextPieceName;
            this.nextPieceCoord1 = nextPieceCoord1;
            this.nextPieceCoord2 = nextPieceCoord2;
            this.nextPieceCoord3 = nextPieceCoord3;
            this.nextPieceCoord4 = nextPieceCoord4;
            this.rotateNumber = rotateNumber;
            this.bgGround = bgGround;
        }
    }
}
