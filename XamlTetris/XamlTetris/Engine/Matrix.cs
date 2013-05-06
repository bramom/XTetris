using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Engine
{
    public class Matrix
    {
        public int RowsCount;
        public int ColsCount;

        protected Element[,] elements;

        public Element this[int iCol, int iRow]
        {
            get { return elements[iCol, iRow]; }
            set { elements[iCol, iRow] = value; }
        }

        //constructor
        public Matrix(int Cols, int Rows)
        {
            ColsCount = Cols;
            RowsCount = Rows;
            elements = new Element[Cols, Rows];
        }

        public void DropLine()
        {
            //move elements down
            for (int j = RowsCount - 1; j > 0; j--)
                for (int i = 0; i < ColsCount; i++)                
                    this[i, j] = this[i, j - 1];

            //delete first line
            for (int i = 0; i < ColsCount; i++)
                this[i, 0] = null;
        }

        public Matrix Add(Tetramino block)
        {
            Matrix res = new Matrix(this.ColsCount, this.RowsCount);

            for (int j = 0; j < RowsCount; j++)
                for (int i = 0; i < ColsCount; i++)
                    res[i, j] = this[i, j];                    

            foreach (var element in block.elements)
                res[block.position.X + element.X, block.position.Y + element.Y] = element;
            
            return res;
        }

        public override string ToString()
        {
            string res = string.Empty;

            for (int j = 3; j < RowsCount; j++)
            {
                for (int i = 0; i < ColsCount; i++)                    
                    res += this[i, j] == null ? "□" : "■"; 
                    
                res += Environment.NewLine;
            }

            return res;
        }
    }
}
