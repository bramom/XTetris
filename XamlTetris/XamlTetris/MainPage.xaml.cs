using GameUtility.Keyboard;
using GameUtility.Timers;
using Engine;
using System.Windows.Controls;
using System;
using System.Windows.Input;
using GameUtility;
using System.Windows;
using System.Windows.Shapes;

namespace XamlTetris
{
    public partial class MainPage : UserControl
    {
        //Game loop
        private KeyHandler keyHandler;
        private DispatcherTimerGameLoop gameLoop;
        private DispatcherTimerGameLoop gravityLoop;
        //private bool IsStoryCompleted = true;

        private World world;
        
        private int CurrentLevel = 5;

        private double ScaleFactor = 1.0;

        public MainPage()
        {
            InitializeComponent();

            //matrica je velicine 10x18, velicina elementa Settings.ElementSize
            //border + 2 sa obe strane
            BorderTable.Width = 10 * Settings.ElementSize + 2;
            BorderTable.Height = 18 * Settings.ElementSize + 2;
            cnvTable.Width = cnvParrent.Width = 10 * Settings.ElementSize;
            cnvTable.Height = cnvParrent.Height = 18 * Settings.ElementSize;
            clipTable.Rect = new Rect(0, 0, 10 * Settings.ElementSize, 18 * Settings.ElementSize);                        

            InitializeSounds();

            Application.Current.Host.Content.FullScreenChanged += new EventHandler(Content_FullScreenChanged);
            /*
            this.GotFocus += new RoutedEventHandler(Page_GotFocus);
            this.LostFocus += new RoutedEventHandler(Page_LostFocus);
            this.MouseLeftButtonDown += new MouseButtonEventHandler(Page_MouseLeftButtonDown);
             */
        }


        private void InitializeSounds()
        {
            SoundUtility.Initialize(pnlScore);
            SoundUtility.LoadMedia("Sounds/hit-01.mp3");
            SoundUtility.LoadMedia("Sounds/beep-03.mp3");
            SoundUtility.LoadMedia("Sounds/StingTheEndOfTheGame.mp3");
        }

        void Content_FullScreenChanged(object sender, EventArgs e)        
        {
            ScaleFactor = 1;
            Renderer.LayoutScaleTransform(RootLayoutScaleTransform, ScaleFactor);
        }

        void Page_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (gameLoop != null)
            {
                gameLoop.Start();
                gravityLoop.Start();
            }            
        }

        void Page_LostFocus(object sender, RoutedEventArgs e)
        {
            gameLoop.Stop();
            gravityLoop.Stop();
        }

        void Page_GotFocus(object sender, RoutedEventArgs e)
        {
            if (gameLoop != null)
            {
                gameLoop.Start();
                gravityLoop.Start();
            }
        }

        /// <summary>
        /// Main loop, input processing
        /// </summary>
        /// <param name="elapsed"></param>
        private void gameLoop_Update(TimeSpan elapsed)
        {
            if (keyHandler.IsAnyKeyPressed)
            {
                if (keyHandler.IsKeyPressed(Key.Up))
                {
                    world.RotateBlock(1);
                    RenderAll();
                }

                if (keyHandler.IsKeyPressed(Key.Down))
                    GameTick();

                if (keyHandler.IsKeyPressed(Key.Left))
                {
                    world.MoveBlock(-1);
                    RenderAll();
                }

                if (keyHandler.IsKeyPressed(Key.Right))
                {
                    world.MoveBlock(1);
                    RenderAll();
                }

                keyHandler.ClearKeyPresses();
            }
        }

        /// <summary>
        /// Gravity event handler
        /// </summary>
        /// <param name="elapsed"></param>
        private void gravityLoop_Update(TimeSpan elapsed)
        {
            GameTick();
        }

        /// <summary>
        /// Game render & update
        /// </summary>
        private void GameTick()
        {
            txbTest.Text = DateTime.Now.ToLongTimeString();

            if (!world.Move())
            {
                label1.Text = "Game Over";
                gameLoop.Stop();
                gravityLoop.Stop();
            }            

            txbLevel.Text = world.Level.ToString();
            txbLines.Text = world.CompletedLines.ToString();
            txbScore.Text = world.Score.ToString();

            if (world.Level != CurrentLevel)
            {
                gravityLoop.Interval = TimeSpan.FromMilliseconds(GetGameSpeed(world.Level));
                CurrentLevel = world.Level;
            }

            if (world.IsLineDroped)
            {
                SoundUtility.Play("Sounds/hit-01.mp3");                
            }
            else 
                if (world.IsDocked)
                {
                    SoundUtility.Play("Sounds/beep-03.mp3");
                    //myStoryboard.Begin();                    
                }
                        
            RenderAll();
        }

        /// <summary>
        /// Kreira objekat World
        /// inicijalizuje game loop handler (TimerClock_Tick)
        /// inicijalizuje event handler za obradu ulaza sa tastature 
        /// </summary>
        private void StartGame()
        {
            SoundUtility.Play("Sounds/StingTheEndOfTheGame.mp3");

            world = new World(Settings.WorldColNum, Settings.WorldRowNum);
            //world.Level = 1;

            if (gameLoop == null)
            {
                btnPause.IsEnabled = true;
                btnStart.Content = "RESTART";

                //gravity loop initialization                
                gravityLoop = new DispatcherTimerGameLoop(GetGameSpeed(world.Level));                
                gravityLoop.Update += new GameLoop.UpdateHandler(gravityLoop_Update);

                //game loop initialization
                gameLoop = new DispatcherTimerGameLoop();
                gameLoop.Update += new GameLoop.UpdateHandler(gameLoop_Update);                

                keyHandler = new KeyHandler(this);
            }

            gravityLoop.Start();
            gameLoop.Start();
        }

        private void RenderAll()
        {
            RenderMatrix();
            RenderInventory();
            RenderActiveTetramino();
        }

        /// <summary>
        /// Draw matrix on cnvTable
        /// </summary>
        private void RenderMatrix()
        {
            cnvTable.Children.Clear();            

            //Matrix.Matrix matrix = world.MatrixTable.Add(world.ActiveBlock);
            Engine.Matrix matrix = world.MatrixTable;

            for (int j = 3; j < matrix.RowsCount; j++)
            {
                for (int i = 0; i < matrix.ColsCount; i++)
                {
                    if (matrix[i, j] != null)
                    {
                        Rectangle rectangle = Renderer.RenderElement(matrix[i,j]);

                        rectangle.SetValue(Canvas.TopProperty, (double)((j - 3) * Settings.ElementSize));
                        rectangle.SetValue(Canvas.LeftProperty, (double)(i * Settings.ElementSize));
                        rectangle.Opacity = 0.8;

                        cnvTable.Children.Add(rectangle);
                    }
                }
            }
        }

        public void RenderInventory()
        {
            //Show Inventory
            cnvTetraminoInventory.Children.Clear();
            int row = 0;

            foreach (Tetramino tetramino in world.TetraminoInvenory)
            {
                Canvas canvas = Renderer.RenderTetramino(tetramino);
                canvas.SetValue(Canvas.TopProperty, (double)(row * 3 * Settings.ElementSize) + 10);
                canvas.Opacity = 1 - row * 0.18;
                cnvTetraminoInventory.Children.Add(canvas);

                row++;
            }
        }
        
        public void RenderActiveTetramino()
        {
            //active tetramino
            foreach (Element element in world.ActiveTetramino.elements)
            {
                Rectangle rectangle = Renderer.RenderElement(element);                
                rectangle.SetValue(Canvas.TopProperty, (double)((element.Y + world.ActiveTetramino.position.Y - 3) * Settings.ElementSize));
                rectangle.SetValue(Canvas.LeftProperty, (double)((element.X + world.ActiveTetramino.position.X) * Settings.ElementSize));

                if ((double)rectangle.GetValue(Canvas.TopProperty) >= 0)
                    cnvTable.Children.Add(rectangle);
            }
        }

        private double GetGameSpeed(int level)
        {            
            double coeff = 1.5 - Math.Log10(level);
            coeff = coeff > 0 ? coeff : 0;
            return coeff * 300;
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            if (gravityLoop.IsEnabled)
            {
                btnPause.Content = "RESUME";
                gravityLoop.Stop();
            }
            else
            {
                btnPause.Content = "PAUSE";
                gravityLoop.Start();
            }
        }

        private void btnFullScreen_Click(object sender, RoutedEventArgs e)
        {            
            Application.Current.Host.Content.IsFullScreen = !Application.Current.Host.Content.IsFullScreen;
        }


        private void btnSizeUp_Click(object sender, RoutedEventArgs e)
        {
            ScaleFactor += 0.2;
            Renderer.LayoutScaleTransform(RootLayoutScaleTransform, ScaleFactor);
        }

        private void btnSizeDown_Click(object sender, RoutedEventArgs e)
        {
            ScaleFactor -= 0.2;
            Renderer.LayoutScaleTransform(RootLayoutScaleTransform, ScaleFactor);
            grdSpace.UpdateLayout();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {            
            //DialogStart dlg = new DialogStart();
            //dlg.Closed += new EventHandler(OnErrorDialogClosed);
            //dlg.Show();            
        }        
    }
}