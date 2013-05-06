using System;
using System.Windows.Media;

namespace Engine
{
    public class TetraminoBuilder
    {
        private Random rand;

        public TetraminoBuilder()
        {
            rand = new Random();
        }

        public Tetramino GetTetramino()
        {            
            int index = rand.Next(6);

            Element el;
            //int[,] numbers = new int[4, 2] { {-1, 0}, {0, 0}, {1, 0}, { 0, -1} };
            int[,] numbers;

            Tetramino tetramino = new Tetramino();

            tetramino.Type = (TetraminoType)index;

            switch (tetramino.Type)
            {
                // I 
                case TetraminoType.I:
                default:
                    //block.color = Colors.Red;
                    //numbers = new int[4, 2] { { 0, -2 }, { 0, -1 }, { 0, 0 }, { 0, 1 } };
                    tetramino.color = Colors.Cyan;
                    numbers = new int[4, 2] { { -2, 0 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                    break;

                // J left oriented, blue
                case TetraminoType.J:
                    tetramino.color = Colors.Blue;
                    //numbers = new int[4, 2] { { 0, -1 }, { 0, 0 }, { 0, 1 }, { -1, 1 } };
                    numbers = new int[4, 2] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 1, 1 } };
                    break;

                // L na desno
                case TetraminoType.L:
                    tetramino.color = Colors.Orange;
                    //numbers = new int[4, 2] { { 0, -1 }, { 0, -0 }, { 0, 1 }, { 1, 1 } };
                    numbers = new int[4, 2] { { -1, 1 }, { -1, 0 }, { 0, 0 }, { 1, 0 } };
                    break;

                // O square, yellow
                case TetraminoType.O:                    
                    tetramino.rotatable = false;
                    tetramino.color = Colors.Yellow;
                    numbers = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 0, 1 }, { 1, 1 } };
                    break;

                // S mirror
                case TetraminoType.S:
                    //block.color = Colors.Magenta;
                    //numbers = new int[4, 2] { { 0, 0 }, { 1, 0 }, { -1, 1 }, { 0, 1 } };
                    tetramino.color = Colors.Green;
                    numbers = new int[4, 2] { { -1, 1 }, { 0, 1 }, { 0, 0 }, { 1, 0 } };
                    break;
                // T  0
                //   000 cian
                case TetraminoType.T:
                    tetramino.color = Colors.Purple;
                    //numbers = new int[4, 2] { { 0, 0 }, { -1, 1 }, { 0, 1 }, { 1, 1 } };
                    numbers = new int[4, 2] { { -1, 0 }, { 0, 0 }, { 1, 0 }, { 0, 1 } };
                    break;
                // Z
                case TetraminoType.Z:
                    tetramino.color = Colors.Red;
                    //numbers = new int[4, 2] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } };
                    numbers = new int[4, 2] { { -1, 0 }, { 0, 0 }, { 0, 1 }, { 1, 1 } };
                    break;
            }            

            for (int i = 0; i < 4; i++)
            {
                el = new Element(tetramino.color);
                el.Position = new Point(numbers[i, 0], numbers[i, 1]);
                tetramino.elements.Add(el);
            }

            return tetramino;
        }
    }
}
