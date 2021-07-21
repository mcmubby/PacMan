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
        Stopwatch timer = new Stopwatch();

        bool goLeft, goRight, goDown, goUp;
        bool noLeft, noRight, noDown, noUp;

        int speed = 8;

        Rect pacmanHitBox;

        int ghostSpeed = 10;
        int ghostMoveStep = 160;
        int currentGhostStep;
        int score = 0;

        public MainWindow()
        {
            InitializeComponent();
            GameMenu();
        }

        private void CanvasKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Right && noRight == false)
            {
                goLeft = goUp = goDown = false;
                noLeft = noUp = noDown = false;

                goRight = true;

                pacman.RenderTransform = new RotateTransform(0, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Left && noLeft == false)
            {
                goRight = goUp = goDown = false;
                goRight = noUp = noDown = false;

                goLeft = true;

                pacman.RenderTransform = new RotateTransform(-180, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Up && noUp == false)
            {
                goRight = goLeft = goDown = false;
                goRight = noLeft = noDown = false;

                goUp = true;

                pacman.RenderTransform = new RotateTransform(-90, pacman.Width / 2, pacman.Height / 2);
            }

            if (e.Key == Key.Down && noDown == false)
            {
                goRight = goLeft = goUp = false;
                goRight = noLeft = noUp = false;

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
            timer.Start();

            currentGhostStep = ghostMoveStep;

            ImageBrush pacManImage = new ImageBrush();
            pacManImage.ImageSource = new BitmapImage(new Uri(@"assets/pacman.jpg", UriKind.Relative));
            pacman.Fill = pacManImage;

            ImageBrush redGhost = new ImageBrush();
            redGhost.ImageSource = new BitmapImage(new Uri(@"assets/red.jpg", UriKind.Relative));
            redGuy.Fill = redGuy1.Fill = redGhost;

            ImageBrush pinkGhost = new ImageBrush();
            pinkGhost.ImageSource = new BitmapImage(new Uri(@"assets/pink.jpg", UriKind.Relative));
            pinkGuy.Fill = pinkGuy1.Fill = pinkGhost;

            ImageBrush orangeGhost = new ImageBrush();
            orangeGhost.ImageSource = new BitmapImage(new Uri(@"assets/orange.jpg", UriKind.Relative));
            orangGuy.Fill = orangGuy1.Fill = orangeGhost;
        }

        private void GameLoop(object sender, EventArgs e)
        {
            Score.Content = "Score : " + score.ToString();
            Time.Content = "Time : " + timer.Elapsed.ToString(@"mm\:ss");
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
