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
        private LightsOutGame game;
        
        public MainWindow()
        {

            
            game = new LightsOutGame();
            InitializeComponent();
            CreateGrid();
            DrawGrid();
            exitButton.IsEnabled = false;
            exitButton.Opacity = .5;

        }

        private void OnDragMoveWindow(object sender,MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void CloseCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Close();
        }

        private void CloseCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            if (game.IsGameOver())
            {
                e.CanExecute = true;

            }
            else
                e.CanExecute = false;

        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            game.NewGame();
            exitButton.IsEnabled = false;
            exitButton.Opacity = .5;
            DrawGrid();
        }

        private void HelpCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = this;
            about.ShowDialog();

        }

        private void CreateGrid()
        {     
            // Remove all previously-existing rectangles
            boardCanvas.Children.Clear();            

            int rectSize = (int)boardCanvas.Width / game.GridSize;

            // Turn entire grid on and create rectangles to represent it
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
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
            for (int r = 0; r < game.GridSize; r++)
            {
                for (int c = 0; c < game.GridSize; c++)
                {
                    Rectangle rect = boardCanvas.Children[index] as Rectangle;
                    index++;

                    if (game.GetGridValue(r, c))
                    {
                        // On
                        rect.Fill = Brushes.Navy;
                        rect.Stroke = Brushes.DarkRed;
                    }
                    else
                    {
                        // Off
                        rect.Fill = Brushes.DarkRed;
                        rect.Stroke = Brushes.Navy;
                    }
                }
            }
        }

        private void Rect_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            // Get row and column from Rectangle's Tag
            Rectangle rect = sender as Rectangle;
            var rowCol = (Point)rect.Tag;
            int row = (int)rowCol.X;
            int col = (int)rowCol.Y;

            game.Move(row, col);

            // Redraw the board
            DrawGrid();

            if (game.IsGameOver())
            {
                exitButton.IsEnabled = true;
                exitButton.Opacity = 1;
                MessageBox.Show(this, "Congratulations!  You've won!", "Lights Out!",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

    }
}
