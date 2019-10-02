using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Wpf_LightsOut
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private LightsOutGame gameLogic;
        
        public MainWindow()
        {
            InitializeComponent();

            gameLogic = new LightsOutGame();
            SizeThree.IsChecked = true;
            CreateGrid();
            DrawGrid();
        }

        private void CreateGrid()
        {     
            // Remove all previously-existing rectangles
            boardCanvas.Children.Clear();            

            int rectSize = (int)boardCanvas.Width / gameLogic.NumCells;

            // Turn entire grid on and create rectangles to represent it
            for (int r = 0; r < gameLogic.NumCells; r++)
            {
                for (int c = 0; c < gameLogic.NumCells; c++)
                {
                    Rectangle rect = new Rectangle();
                    rect.Fill = Brushes.White;
                    rect.Width = rectSize + 1;
                    rect.Height = rect.Width + 1;
                    rect.Stroke = Brushes.Black;

                    // Store each row and col as a Point
                    rect.Tag = new Point(r, c);
                    rect.MouseLeftButtonDown += Rect_MouseLeftButtonDown;

                    int x = c * rectSize;
                    int y = r * rectSize;

                    Canvas.SetTop(rect, y);
                    Canvas.SetLeft(rect, x);

                    // Add the new rectangle to the canvas' children
                    boardCanvas.Children.Add(rect);
                }
            }
        }              

        private void DrawGrid()
        {
            int index = 0;
            
            // Set the colors of the rectangles
            for (int r = 0; r < gameLogic.NumCells; r++)
            {
                for (int c = 0; c < gameLogic.NumCells; c++)
                {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;

                    if (gameLogic.GetGridValue(r, c))
                    {
                        // On
                        rect.Fill = Brushes.White;
                        rect.Stroke = Brushes.Black;
                    }
                    else
                    {
                        // Off
                        rect.Fill = Brushes.Black;
                        rect.Stroke = Brushes.White;
                    }
                }
            }
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Rectangle rect = sender as Rectangle;
            var rowCol = (Point)rect.Tag;
            int row = (int)rowCol.X;
            int col = (int)rowCol.Y;

            gameLogic.MakeMove(row, col);

            // Redraw the board
            DrawGrid();

            if (gameLogic.PlayerWon())
            {
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void boardCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            /*
               int rectSize = (int)boardCanvas.Width / numberOfCells;

               // Find row, col of mouse press
               Point mousePosition = e.GetPosition(boardCanvas);
               int row = (int)(mousePosition.Y) / rectSize;
               int col = (int)(mousePosition.X) / rectSize;

               // Invert selected box and all surrounding boxes
               for (int i = row - 1; i <= row + 1; i++)
                   for (int j = col - 1; j <= col + 1; j++)
                       if (i >= 0 && i < numberOfCells && j >= 0 && j < numberOfCells)
                           grid[i, j] = !grid[i, j];

               // Redraw the board
               DrawGrid();

               if (gameLogic.PlayerWon())
               {
                   MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!",
                       MessageBoxButton.OK, MessageBoxImage.Information);
               }
               */
        }

        private void HelpAbout_Click(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();
        }

        private void SizeChanged_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = sender as MenuItem;
            SizeThree.IsChecked = false;
            SizeFive.IsChecked = false;
            SizeSeven.IsChecked = false;
            switch (menuItem.Name)
            {
                case "SizeThree":
                    SizeThree.IsChecked = true;
                    gameLogic.NumCells = 3;
                    break;
                case "SizeFive":
                    SizeFive.IsChecked = true;
                    gameLogic.NumCells = 5;
                    break;
                case "SizeSeven":
                    SizeSeven.IsChecked = true;
                    gameLogic.NumCells = 7;
                    break;
            }

            CreateGrid();
            DrawGrid();
        }

        private void MenuExit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MenuNew_Click(object sender, RoutedEventArgs e)
        {
            gameLogic.NewGame();
            DrawGrid();
        }
    }
}
