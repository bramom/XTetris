using System.Collections.Generic;
using System.Windows.Media;

namespace Engine
{
    public class Tetramino
    {
        public Point position;
        public List<Element> elements = new List<Element>();
        public Color color;
        public TetraminoType Type;
        internal bool rotatable = true;

        public Tetramino()
        {            
            position.X = 4;
            position.Y = 2;
        }

        public void Move(int Direction)
        {         
            position.X += Direction;
        }

        public void Rotate(int Direction)
        {
            if (rotatable)
                foreach (var el in elements)         
                    el.Position = new Point(-el.Y * Direction, el.X * Direction);
        }

        public void Add(Element el)
        {
            elements.Add(el);
        }

        public object Clone()
        {
            Tetramino clone = (Tetramino) this.MemberwiseClone();
            
            List<Element> es = new List<Element>();
            foreach (Element e in elements)
                es.Add((Element)e.Clone());

            clone.elements = es;
            return clone;
        }
    }
}
