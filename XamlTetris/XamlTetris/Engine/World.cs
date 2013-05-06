using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamlTetris;

namespace Engine
{
    public class World
    {
        private TetraminoBuilder tetraminoBuilder;

        public Matrix MatrixTable;
        public Tetramino ActiveTetramino;

        public bool IsDocked = false;
        public bool IsLineDroped = false;

        public int Level { get; set; }
        public int CompletedLines { get; set; }
        public int Score { get; set; }
        public int TetraminoNum { get; set; }
        public Queue<Tetramino> TetraminoInvenory;

        public World():this(18, 10)
        {
        }

        public World(int Cols, int Rows)
        {
            tetraminoBuilder = new TetraminoBuilder();

            TetraminoInvenory = new Queue<Tetramino>();

            for (int i = 0; i < 6; i++)
                TetraminoInvenory.Enqueue(tetraminoBuilder.GetTetramino());

            Level = 1;
            CompletedLines = 0;
            Score = 0;
            TetraminoNum = 0;

            MatrixTable = new Matrix(Cols, Rows + 3);
            
            GetNewBlock();
        }

        private void GetNewBlock()
        {
            TetraminoNum++;
            TetraminoInvenory.Enqueue(tetraminoBuilder.GetTetramino());
            ActiveTetramino = TetraminoInvenory.Dequeue();
        }

        /// <summary>
        /// Main Game Loop
        /// </summary>
        /// <returns>game over - false</returns>
        public bool Move()
        {
            bool GameOver = false;
            IsDocked = IsBlockDocked(ActiveTetramino);
            IsLineDroped = false;

            if (!IsDocked) ActiveTetramino.position.Y++;
            
            IsDocked = IsBlockDocked(ActiveTetramino);
            if (IsDocked)
            {
                Score += 10;

                MatrixTable = MatrixTable.Add(ActiveTetramino);
                IsLineDroped = CheckLines();
                GetNewBlock();
                GameOver = IsBlockDocked(ActiveTetramino);
            }

            return !GameOver;
        }

        //Action move block left & right
        public void MoveBlock(int Direction)
        {
            Tetramino block = (Tetramino) ActiveTetramino.Clone();

            ActiveTetramino.Move(Direction);

            if (IsOut(ActiveTetramino) || IsCollision(ActiveTetramino))
                ActiveTetramino = (Tetramino) block.Clone();
        }

        //Action rotate block 
        public void RotateBlock(int Direction)
        {
            Tetramino block = (Tetramino)ActiveTetramino.Clone();

            ActiveTetramino.Rotate(Direction);

            if (IsOut(ActiveTetramino) || IsCollision(ActiveTetramino))
                ActiveTetramino = (Tetramino)block.Clone();
        }

        public void DockBlock()
        {

        }

        private bool IsBlockDocked(Tetramino block)
        {
            bool IsDocked = false;

            int i = -1;

            while (!IsDocked && i++ < block.elements.Count - 1)
                IsDocked =
                    (block.position.Y + block.elements[i].Y >= MatrixTable.RowsCount - 1)
                    || (!MatrixTable[block.position.X + block.elements[i].X, block.position.Y + block.elements[i].Y + 1].IsNull());

            return IsDocked;
        }

        private bool IsOut(Tetramino block)
        {
            bool isOut = false;
            
            int i = 0;
            while (!isOut && i < block.elements.Count)
            {
                isOut = (block.position.X + block.elements[i].X >= MatrixTable.ColsCount)
                        || (block.position.X + block.elements[i].X < 0);
                i++;
            }

            return isOut;
        }

        private bool IsCollision(Tetramino block)
        {
            bool isCollision = false;

            int i = 0;
            while (!isCollision && i < block.elements.Count)
            {
                isCollision = !MatrixTable[block.position.X + block.elements[i].X, block.position.Y + block.elements[i].Y].IsNull();
                i++;
            }

            return isCollision;
        }


        private bool IsLineFull(int i)
        {
            bool isFull = true;

            int j = 0;
            while (isFull && j < MatrixTable.ColsCount)
                isFull = !MatrixTable[j++, i].IsNull();
            
            return isFull;
        }

        private void DropLine(int i)
        {
            for (int k = i - 1; k >= 0; k--)
                for (int j = 0; j < MatrixTable.ColsCount; j++)
                    MatrixTable[j, k + 1] = MatrixTable[j, k];
        }

        private bool CheckLines()
        {
            bool res = false;

            for (int i = MatrixTable.RowsCount - 1; i > 0; i--)
                if (IsLineFull(i))
                {
                    res = true;
                    Score += 100;
                    CompletedLines++;
                    DropLine(i++);

                    if (CompletedLines % 5 == 0)
                        Level++;
                }

            return res;
        }
    }
}
