using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PacMan
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        DispatcherTimer gameTime = new DispatcherTimer();
        DateTime levelIime = DateTime.UtcNow + TimeSpan.FromMinutes(5);

        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp;

        int speed = 8;

        Rect pacmanHitBox;

        int ghostSpeed = 10;
        int ghostMoveStep = 320;
        int currentGhostStep;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            GameMenu();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                noRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Right && noRight == false)
            {
                goLeft = goUp = goDown = false;
                noLeft = noUp = noDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Up && noUp == false)
            {
                goRight = goLeft = goDown = false;
                noRight = noLeft = noDown = false;

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && noDown == false)
            {
                goRight = goLeft = goUp = false;
                noRight = noLeft = noUp = false;

                goDown = true;

                pacman.RenderTransform = new RotateTransform(90, pacman.Width / 2, pacman.Height / 2);
            }
        }

        private void GameMenu()
        {
            Level1Canvas.Focus();  //Change to main menu canvas later

            gameTime.Tick += GameLoop;
            gameTime.Interval = TimeSpan.FromMilliseconds(20);
            gameTime.Start();
            //timer.Start();

            currentGhostStep = ghostMoveStep;

            ImageBrush pacManImage = new ImageBrush();
            pacManImage.ImageSource = new BitmapImage(new Uri(@"assets/pacman.jpg", UriKind.Relative));
            pacman.Fill = pacManImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri(@"assets/red.jpg", UriKind.Relative));
            rightM3.Fill = leftM1.Fill = redGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri(@"assets/pink.jpg", UriKind.Relative));
            leftM2.Fill = rightM1.Fill = pinkGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri(@"assets/orange.jpg", UriKind.Relative));
            downM1.Fill = rightM2.Fill = orangeGhost;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            Score.Content = "Score : " + score.ToString();
            //Time.Content = "Time : " + timer.Elapsed.ToString(@"mm\:ss");
            Time.Content = "Time : " + (levelIime - DateTime.UtcNow).ToString(@"mm\:ss");


            //Pacman movement

            if (goRight)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + speed);
            }
            if (goLeft)
            {
                Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - speed);
            }
            if (goUp)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) - speed);
            }
            if (goDown)
            {
                Canvas.SetTop(pacman, Canvas.GetTop(pacman) + speed);
            }

            pacmanHitBox = new Rect(Canvas.GetLeft(pacman), Canvas.GetTop(pacman), pacman.Width, pacman.Height);
            var downM1HitBorder = new Rect(Canvas.GetLeft(downM1), Canvas.GetTop(downM1), downM1.Width, downM1.Height);

            foreach (var x in Level1Canvas.Children.OfType<Rectangle>())
            {

                Rect hitBox = new Rect(Canvas.GetLeft(x), Canvas.GetTop(x), x.Width, x.Height);

                if ((string)x.Tag == "Wall")
                {
                    if (goLeft == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) + 10);
                        noLeft = true;
                        goLeft = false;
                    }
                    
                    if (goRight == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetLeft(pacman, Canvas.GetLeft(pacman) - 10);
                        noRight = true;
                        goRight = false;
                    }
                    
                    if (goDown == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) - 10);
                        noDown = true;
                        goDown = false;
                    }

                    //trial
                    if (downM1HitBorder.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(downM1, Canvas.GetTop(downM1) - ghostSpeed);
                    }

                    if (goUp == true && pacmanHitBox.IntersectsWith(hitBox))
                    {
                        Canvas.SetTop(pacman, Canvas.GetTop(pacman) + 10);
                        noUp = true;
                        goUp = false;
                    }
                }

                if((string)x.Tag == "coin" && pacmanHitBox.IntersectsWith(hitBox) && x.Visibility == Visibility.Visible)
                {
                    x.Visibility = Visibility.Hidden;
                    score++;
                }

                /*if ((string)x.Tag == "ghost")
                {
                    if (pacmanHitBox.IntersectsWith(hitBox))
                    {
                        GameOver($"Ghosts got you. \nScore: {score}. \nClick ok to go to main menu");
                    }

                    // Ghost movement 
                    if (x.Name.Contains("left"))
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) - ghostSpeed);

                    }

                    if (x.Name.Contains("right"))
                    {
                        Canvas.SetLeft(x, Canvas.GetLeft(x) + ghostSpeed);
                    }

                    if (x.Name.Contains("down"))
                    {
                        Canvas.SetTop(x, Canvas.GetTop(x) + ghostSpeed);
                    }

                    // reduce one from the current ghost step integer
                    currentGhostStep--;

                    // if the current ghost step integer goes below 1 
                    if (currentGhostStep < 1)
                    {
                        // reset the current ghost step to the ghost move step value
                        currentGhostStep = ghostMoveStep;
                        // reverse the ghost speed integer
                        ghostSpeed = -ghostSpeed;
                    }
                }*/
            }

            if(score == 100)
            {
                GameOver($"You Win!. \nScore: {score}. \nClick ok to go to main menu");
            }

            if ((levelIime - DateTime.UtcNow).ToString(@"mm\:ss") == "00:00")
            {
                GameOver($"Time out!. \nScore: {score}. \nClick ok to go to main menu");
            }
        }

        private void GameOver(string message)
        {
            gameTime.Stop();
            MessageBox.Show(message, "Game Over");

            //Ask if user wants to replay. 
            //But for now just start a new application and shutdown the current one
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
    }
}
