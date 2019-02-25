using System;
using System.Collections.Generic;
using System.Linq;
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
        private Brush SnakeColour = Brushes.Green, PointColour = Brushes.Red;

        private DispatcherTimer timer = new DispatcherTimer();
       
        //Used for randomize the position of the BonusPoint
        Random r = new Random();

        private int Score = 0, LastDir = 1 , TimeSpanUpdate = 45;
        //helper variables, used to increase just once per level the game speed
        private bool Level2 = true, Level3 = true;
        
        public MainWindow()
        {
            InitializeComponent();

            //Snake Starting Point
            SnakePoints.Add(new Point(100, 100));
            
            PaintSnake();
            PaintBonusPoint();

            timer.Tick += new EventHandler(TimerTick_GameUpdate);
            timer.Interval = new TimeSpan(0,0,0,0,TimeSpanUpdate);
            timer.Start();
        }
        //Move the Snake in the the same direction as the last one, at a given TimeSpan period
        private void TimerTick_GameUpdate(object sender, EventArgs e)
        {
            MoveSnake(LastDir);

            //Level 2
            if(Score > 9 && Score < 50 && Level2 == true)
            {
                TimeSpanUpdate -= 5;
                timer.Interval = new TimeSpan(0,0,0,0,TimeSpanUpdate);
                Level2 = false;
            }
            //Level 3
            else if (Score >= 50 && Level3 == true)
            {
                TimeSpanUpdate -= 5;
                timer.Interval = new TimeSpan(0, 0, 0, 0, TimeSpanUpdate);
                Level3 = false;
            }
        }
        //Utility function to end the game and display the Score
        private void ENDGAME()
        {
            timer.Stop();
            MessageBox.Show("You Lose! Your score is " +
                    Score.ToString(), "Game Over", MessageBoxButton.OK);
            this.Close();
        }
        //Move the Snake by pressing the keyboard arrows
        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Up:
                    MoveSnake(0);
                    break;
                case Key.Down:
                    MoveSnake(1);
                    break;
                case Key.Right:
                    MoveSnake(2);
                    break;
                case Key.Left:
                    MoveSnake(3);
                    break;
            }
        }
        //Compute next BonusPoint Random position
        private void Calc_BonusPoint()
        {
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
                Width = 25,
                Height = 25
            };

            Canvas.SetLeft(BPoint,BonusPoint.X);
            Canvas.SetTop(BPoint,BonusPoint.Y);

            paintCanvas.Children.Add(BPoint);
        }
        //Draw the Snake Body
        private void PaintSnake() {
            
            foreach (Point p in SnakePoints){
                Rectangle SnakePart = new Rectangle
                {
                    Fill = SnakeColour,
                    Width = 20,
                    Height = 20
                };
                
                Canvas.SetLeft(SnakePart, p.X);
                Canvas.SetTop(SnakePart, p.Y);
                
                paintCanvas.Children.Add(SnakePart);
            }
        }
        //Update the entire Canvas
        private void UpdatePaint() {

            paintCanvas.Children.Clear();
            PaintSnake();
            PaintBonusPoint();
        }

        //0 = UP , 1 = DOWN , 2 = RIGHT, 3 = LEFT
        private void MoveSnake(int dir)
        {
            if (SnakePoints[0].X >= 0 && SnakePoints[0].X <= 580 && SnakePoints[0].Y >= 0 && SnakePoints[0].Y <= 380)
            {
                timer.Stop();
                switch (dir)
                {
                    //UP
                    case 0:
                        if (LastDir != 1)
                        {
                            if (SnakePoints.Contains(new Point(SnakePoints[0].X, SnakePoints[0].Y - 20))) { ENDGAME(); break; }

                            SnakePoints.Insert (0, new Point(SnakePoints[0].X, SnakePoints[0].Y-20));
                            SnakePoints.Remove(SnakePoints.Last());

                            if (Point.Equals(SnakePoints[0],BonusPoint))
                            {
                                SnakePoints.Add(SnakePoints.Last());
                                Calc_BonusPoint();
                                Score++;
                            }
                            UpdatePaint();
                            LastDir = 0;
                        }
                        break;
                    //DOWN
                    case 1:
                        if (LastDir != 0)
                        {
                            if (SnakePoints.Contains(new Point(SnakePoints[0].X, SnakePoints[0].Y + 20))) { ENDGAME(); break; }

                            SnakePoints.Insert(0, new Point(SnakePoints[0].X, SnakePoints[0].Y + 20));
                            SnakePoints.Remove(SnakePoints.Last());

                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                SnakePoints.Add(SnakePoints.Last());
                                Calc_BonusPoint();
                                Score++;
                            }
                            UpdatePaint();
                            LastDir = 1;
                        }
                        break;
                    //RIGHT
                    case 2:
                        if (LastDir != 3)
                        {
                            if (SnakePoints.Contains(new Point(SnakePoints[0].X + 20, SnakePoints[0].Y))) { ENDGAME(); break; }

                            SnakePoints.Insert(0, new Point(SnakePoints[0].X+20, SnakePoints[0].Y));
                            SnakePoints.Remove(SnakePoints.Last());
                            
                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                SnakePoints.Add(SnakePoints.Last());
                                Calc_BonusPoint();
                                Score++;
                            }
                            UpdatePaint();
                            LastDir = 2;
                        }
                        break;
                    //LEFT
                    case 3:
                        if (LastDir != 2)
                        {
                            if (SnakePoints.Contains(new Point(SnakePoints[0].X - 20, SnakePoints[0].Y))) { ENDGAME(); break; }

                            SnakePoints.Insert(0, new Point(SnakePoints[0].X-20, SnakePoints[0].Y));
                            SnakePoints.Remove(SnakePoints.Last());

                            if (Point.Equals(SnakePoints[0], BonusPoint))
                            {
                                SnakePoints.Add(SnakePoints.Last());
                                Calc_BonusPoint();
                                Score++;
                            }
                            UpdatePaint();
                            LastDir = 3;
                        }
                        break;
                }
                timer.Start();
                ScoreLabel.Content = Score;
            }
            else { ENDGAME(); }
        }
    }
}
