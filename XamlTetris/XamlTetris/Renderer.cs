using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Engine;

namespace XamlTetris
{
    public static class Renderer
    {
        public static Rectangle RenderElement(Element element)
        {
            Rectangle rectangle = new Rectangle();
            /*
            RadialGradientBrush brush = new RadialGradientBrush(element.Color, Colors.Black);            
            brush.RadiusX = 5;
            brush.RadiusY = 5;
            rectangle.Fill = brush;
             */
            //rectangle.Fill = (RadialGradientBrush)App.Current.Resources["beautifullBrush"];
            
            rectangle.Fill = new SolidColorBrush(element.Color);
            rectangle.Stroke = new SolidColorBrush(Color.FromArgb(20, 200, 200, 200));
            rectangle.StrokeThickness = 0.5;
                        
            rectangle.Height = Settings.ElementSize;
            rectangle.Width = Settings.ElementSize;

            return rectangle;
        }

        public static Canvas RenderTetramino(Tetramino tetramino)
        {
            Canvas canvas = new Canvas();

            double xTranslationCoeff;

            switch (tetramino.Type)
            {
                case TetraminoType.I:
                    xTranslationCoeff = 2;
                    break;
                case TetraminoType.O:
                    xTranslationCoeff = 1;
                    break;
                default:
                    xTranslationCoeff = 1.5;
                    break;
            }            

            foreach (Element element in tetramino.elements)
            {
                Rectangle rectangle = Renderer.RenderElement(element);
                rectangle.SetValue(Canvas.LeftProperty, (double)((element.X + xTranslationCoeff) * Settings.ElementSize));
                rectangle.SetValue(Canvas.TopProperty, (double)((element.Y) * Settings.ElementSize));
                
                canvas.Children.Add(rectangle);
            }

            return canvas;
        }

        public static void LayoutScaleTransform(ScaleTransform rootLayoutScaleTransform, double scaleFactor)
        {
            rootLayoutScaleTransform.ScaleX = scaleFactor;
            rootLayoutScaleTransform.ScaleY = scaleFactor;
        }
    }
}
