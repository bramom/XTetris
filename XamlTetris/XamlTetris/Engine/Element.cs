using System.Windows.Media;

namespace Engine
{
    public class Element
    {
        protected Point position;

        public int X
        {
            get { return position.X ; }
            set { position.X = value; }
        }

        public int Y
        {
            get { return (int)position.Y; }
            set { position.Y = value; }
        }

        public Point Position
        {
            get { return position; }
            set { position = value; }
        }

        public Color Color { get; set; }

        public Element()
        {
        }

        public Element(Color color)
        {
            this.Color = color;
        }

        public override string ToString()
        {
            return this.IsNull() ? " " : "█";
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
