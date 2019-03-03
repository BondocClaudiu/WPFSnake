using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace WpfSnake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //BonusPoint starting position
        private Point BonusPoint = new Point(200,300); 
        //The list of coordinates for the Snake body
        private List<Point> SnakePoints = new List<Point>();
        //List of Colours
        private Brush SnakeHeadColour = Brushes.Green, SnakeColour = Brushes.MediumSeaGreen, PointColour = Brushes.Red;

        //timer -> Used to update the snake moving at a given interval per level, expressed by the TimeSpanUpdate in miliseconds . UpdatePaintTimer -> Updates the canvas at aprox 30fps
        private DispatcherTimer timer = new DispatcherTimer() , UpdatePaintTimer = new DispatcherTimer();
       
        //Used for randomize the position of the BonusPoint
        Random r = new Random();

        //Base dir, hard-googled
        private static String BaseDir = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName;
        //All in-game sounds
        SoundPlayer UpPlayer = new SoundPlayer(BaseDir + @"\Audio\up.wav"), DownPlayer = new SoundPlayer(BaseDir + @"\Audio\down.wav"), RightPlayer = new SoundPlayer(BaseDir + @"\Audio\right.wav"),
            LeftPlayer = new SoundPlayer(BaseDir + @"\Audio\left.wav"),EatPlayer = new SoundPlayer(BaseDir + @"\Audio\eat.wav"), DeadPlayer = new SoundPlayer(BaseDir + @"\Audio\dead.wav") , 
            PausePlayer = new SoundPlayer(BaseDir + @"\Audio\pause.wav");

        //LastDir 0=U 1=D 2=L 3=R, LastDir = Last Direction ,LastKey = LasKey Pressed, TimeSpanUpdate = the interval between 2 moves of the snake in miliseconds(if there is no arrow key pressed)
        private int Score = 0, LastDir = 1 , LastKey = 26,TimeSpanUpdate = 65;
        //helper variables, used to increase the speed just once per level
        private bool Level2 = true, Level3 = true, Level4 = true, isPaused = false;
        
        public MainWindow()
        {
            InitializeComponent();

            //Snake Starting Point
            SnakePoints.Add(new Point(100, 200));

            PaintSnake();
            PaintBonusPoint();
            
            timer.Interval = new TimeSpan(0, 0, 0, 0, TimeSpanUpdate);
            timer.Tick += new EventHandler(TimerTick_GameUpdate);
            timer.Start();

            UpdatePaintTimer.Interval = new TimeSpan(0,0,0,0,30);
            UpdatePaintTimer.Tick += UpdatePaintTimer_Tick;
            UpdatePaintTimer.Start();
        }

        //Update the game graphics every 33 ms
        private void UpdatePaintTimer_Tick(object sender, EventArgs e)
        {
            UpdatePaint();
        }

        //Move the Snake in the the same direction as the last one, at a given TimeSpan period
        private void TimerTick_GameUpdate(object sender, EventArgs e)
        {
            MoveSnake(LastDir);

            //Level 2
            if(Score > 9 && Level2 == true)
            {
                TimeSpanUpdate -= 10;
                timer.Interval = new TimeSpan(0,0,0,0,TimeSpanUpdate);
                Level2 = false;
            }
            //Level 3
            else if (Score >= 30 && Level3 == true)
            {
                TimeSpanUpdate -= 10;
                timer.Interval = new TimeSpan(0, 0, 0, 0, TimeSpanUpdate);
                Level3 = false;
            }
            //Level 4
            else if (Score >= 50 && Level4 == true)
            {
                TimeSpanUpdate -= 10;
                timer.Interval = new TimeSpan(0, 0, 0, 0, TimeSpanUpdate);
                Level4 = false;
            }
        }

        //Utility function to end the game and display the Score
        private void ENDGAME()
        {
            //Play an ending sound
            DeadPlayer.Play();
            timer.Stop();
            UpdatePaintTimer.Stop();
            MessageBox.Show("You Lose! Your score is " +
                    Score.ToString(), "Game Over", MessageBoxButton.OK);
            this.Close();
        }

        //Move the Snake by pressing the keyboard arrows
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.P:
                    if(!isPaused)
                    {
                        PausePlayer.Play();
                        PausedLabel.Visibility = Visibility.Visible;
                        isPaused = true;
                        timer.Stop();
                    }
                    else if (isPaused)
                    {
                        PausePlayer.Play();
                        PausedLabel.Visibility = Visibility.Hidden;
                        isPaused = false;
                        timer.Start();
                    }
                    break;
                case Key.Up:
                    if (LastKey != (int)Key.Up && !isPaused)
                    {
                        UpPlayer.Play();
                        MoveSnake(0);
                        LastKey = (int)Key.Up;
                    }
                    break;
                case Key.Down:
                    if (LastKey != (int)Key.Down && !isPaused)
                    {
                        DownPlayer.Play();
                        MoveSnake(1);
                        LastKey = (int)Key.Down;
                    }
                    break;
                case Key.Right:
                    if (LastKey != (int)Key.Right && !isPaused)
                    {
                        RightPlayer.Play();
                        MoveSnake(2);
                        LastKey = (int)Key.Right;
                    }
                    break;
                case Key.Left:
                    if (LastKey != (int)Key.Left && !isPaused)
                    {
                        LeftPlayer.Play();
                        MoveSnake(3);
                        LastKey = (int)Key.Left;
                    }
                    break;
            }
        }
        
        //Compute next BonusPoint Random position
        private void Calc_BonusPoint()
        {
            //Play some crap music
            EatPlayer.Play();
            //random position for BonusPoint ,but not the same with any of the SnakePoints
            do
            {
                BonusPoint.X = r.Next(0, 600 / 20) * 20;
                BonusPoint.Y = r.Next(0, 400 / 20) * 20;
            } while (SnakePoints.Contains(BonusPoint));
        }
        
        //Draw the BonusPoint
        private void PaintBonusPoint()
        {
            Ellipse BPoint = new Ellipse
            {
                Fill = PointColour,
                Width = 14,
                Height = 14
            };

            Canvas.SetLeft(BPoint,BonusPoint.X+4);
            Canvas.SetTop(BPoint,BonusPoint.Y+4);

            paintCanvas.Children.Add(BPoint);
        }
        
        //Draw the Snake Body
        private void PaintSnake() {
            
            for (int i = 0; i < SnakePoints.Count; i++){
                Rectangle SnakePart = new Rectangle
                {
                    Fill = SnakeColour,
                    Width = 21,
                    Height = 21
                };

                if (i == 0) { SnakePart.Fill = SnakeHeadColour; }
                Canvas.SetLeft(SnakePart, SnakePoints[i].X);
                Canvas.SetTop(SnakePart, SnakePoints[i].Y+1);
                
                paintCanvas.Children.Add(SnakePart);
            }
        }
        
        //Update the entire Canvas
        private void UpdatePaint() {

            paintCanvas.Children.Clear();
            PaintSnake();
            PaintBonusPoint();
        }
        
        //Move the snake in a given direction: 0 = UP , 1 = DOWN , 2 = RIGHT, 3 = LEFT
        private void MoveSnake(int dir)
        {
            timer.Stop();
            switch (dir)
            {
                //UP
                case 0:
                    if (LastDir != 1 && SnakePoints[0].Y != 0)
                    {
                        if (SnakePoints.Contains(new Point(SnakePoints[0].X, SnakePoints[0].Y - 20))) { ENDGAME(); break; }
                        else
                        {
                            SnakePoints.Insert(0, new Point(SnakePoints[0].X, SnakePoints[0].Y - 20));

                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                Calc_BonusPoint();
                                Score++;
                            }
                            else
                            {
                                SnakePoints.Remove(SnakePoints.Last());
                            }
                            LastDir = 0;
                        }
                    }
                    else if(SnakePoints[0].Y == 0) { ENDGAME(); break; }
                    break;
                //DOWN
                case 1:
                    if (LastDir != 0 && SnakePoints[0].Y != 380)
                    {
                        if (SnakePoints.Contains(new Point(SnakePoints[0].X, SnakePoints[0].Y + 20))) { ENDGAME(); break; }
                        else
                        {
                            SnakePoints.Insert(0, new Point(SnakePoints[0].X, SnakePoints[0].Y + 20));

                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                Calc_BonusPoint();
                                Score++;
                            }
                            else
                            {
                                SnakePoints.Remove(SnakePoints.Last());
                            }
                            LastDir = 1;
                        }
                    }
                    else if (SnakePoints[0].Y == 380) { ENDGAME(); break; }
                    break;
                //RIGHT
                case 2:
                    if (LastDir != 3 && SnakePoints[0].X != 580)
                    {
                        if (SnakePoints.Contains(new Point(SnakePoints[0].X + 20, SnakePoints[0].Y))) { ENDGAME(); break; }
                        else
                        {
                            SnakePoints.Insert(0, new Point(SnakePoints[0].X + 20, SnakePoints[0].Y));

                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                Calc_BonusPoint();
                                Score++;
                            }
                            else
                            {
                                SnakePoints.Remove(SnakePoints.Last());
                            }
                            LastDir = 2;
                        }
                    }
                    else if (SnakePoints[0].X == 580) { ENDGAME(); break; }
                    break;
                //LEFT
                case 3:
                    if (LastDir != 2 && SnakePoints[0].X != 0)
                    {
                        if (SnakePoints.Contains(new Point(SnakePoints[0].X - 20, SnakePoints[0].Y))) { ENDGAME(); break; }
                        else
                        {
                            SnakePoints.Insert(0, new Point(SnakePoints[0].X - 20, SnakePoints[0].Y));

                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                Calc_BonusPoint();
                                Score++;
                            }
                            else
                            {
                                SnakePoints.Remove(SnakePoints.Last());
                            }
                            LastDir = 3;
                        }
                    }
                    else if (SnakePoints[0].X == 0) { ENDGAME(); break; }
                    break;
            }
            timer.Start();
            ScoreLabel.Content = Score;
        }
    }
}
