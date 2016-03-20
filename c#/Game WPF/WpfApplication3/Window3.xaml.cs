using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for Window3.xaml
    /// </summary>
    /// 
    public partial class Window3 : Window
    {
        TextBlock[,] graphicTBLs;
        const int otstup = 5;
        const int thick = 1;
        double CurrentCellSize;
        game gameParent;


        public Window3()
        {
            InitializeComponent();
            gameParent = new game();
            gameParent.Start();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Paint(Field.ActualWidth, Field.ActualHeight);
        }

        //paintig field
        private void Paint(double Width, double Height)
        {
            InsideField.Children.Clear();

            int x = game.MapSizeX;
            int y = game.MapSizeY;

            graphicTBLs = new TextBlock[x, y];

            CurrentCellSize = (Width - otstup) / x;
            if ((Height - otstup) / y < CurrentCellSize)
                CurrentCellSize = (Height - otstup) / y;
            if (CurrentCellSize < 2 * thick)
                CurrentCellSize = 2 * thick;

            InsideField.Width = CurrentCellSize * x;
            InsideField.Height = CurrentCellSize * y;

            for (int i = 0; i < x; i++)
                for (int j = 0; j < y; j++)
                {
                    if (i != game.End.X || j != game.End.Y)
                        AddRgl(i, j, new SolidColorBrush(Color.FromRgb(255, 255, 0)));
                    else
                        AddRgl(i, j, new SolidColorBrush(Color.FromRgb(200, 0, 255)));

                    AddTBL(i, j);

                }
            chipPaint(Width, Height);
        }

        private void AddRgl(int x, int y, SolidColorBrush Color)
        {
            Rectangle rgl = new Rectangle()
            {
                Fill = Color,
                StrokeThickness = thick,
                Width = CurrentCellSize - 2 * thick,
                Height = CurrentCellSize - 2 * thick,
            };
            rgl.SetValue(Canvas.LeftProperty, x * CurrentCellSize + thick);
            rgl.SetValue(Canvas.BottomProperty, y * CurrentCellSize + thick);
            InsideField.Children.Add(rgl);

        }

        private void AddTBL(int x, int y)
        {
            graphicTBLs[x, y] = new TextBlock()
            {
                FontSize = CurrentCellSize / 5 + 0.01,
                Text = game.GetMapCell(x, y).Price.ToString(),
            };
            graphicTBLs[x, y].SetValue(Canvas.LeftProperty, (x + 0.5) * CurrentCellSize - 5 - graphicTBLs[x, y].ActualWidth / 2 + thick);
            graphicTBLs[x, y].SetValue(Canvas.BottomProperty, (y + 0.5) * CurrentCellSize - 5 - graphicTBLs[x, y].ActualHeight / 2 + thick);
            InsideField.Children.Add(graphicTBLs[x, y]);
        }

        //paintig chips
        private void chipPaint(double Width, double Height)
        {
            int x = game.MapSizeX;
            int y = game.MapSizeY;

            CurrentCellSize = (Width - otstup) / x;
            if ((Height - otstup) / y < CurrentCellSize)
                CurrentCellSize = (Height - otstup) / y;
            if (CurrentCellSize < 2 * thick)
                CurrentCellSize = 2 * thick;

            cell a = game.GetPlayer1CurrentState();
            AddElps(a.X, a.Y, new SolidColorBrush(Color.FromRgb(255, 180, 0)));
            AddTBL(a.X, a.Y, "1, L =" + a.Price.ToString());

            for (int i = 0; i < game.GetPlayer2Count(); i++)
            {
                a = game.GetPlayer2CurrentState(i + 1);
                AddElps(a.X, a.Y, new SolidColorBrush(Color.FromRgb(255, 180, 220)));
                AddTBL(a.X, a.Y, "2, L =" + a.Price.ToString());
            }
        }

        private void AddElps (int x, int y, SolidColorBrush Color)
        {
            Ellipse player1E = new Ellipse()
            {
                Width = (CurrentCellSize - 2 * thick),
                Height = (CurrentCellSize - 2 * thick),
                StrokeThickness = thick,
                Fill = Color
            };
            player1E.SetValue(Canvas.LeftProperty, x * CurrentCellSize + thick + 0.000000001);
            player1E.SetValue(Canvas.BottomProperty, y * CurrentCellSize + thick + 0.000000001);
            InsideField.Children.Add(player1E);
        }

        private void AddTBL(int x, int y, string text)
        {
            TextBlock player = new TextBlock()
            {
                Text = text,
                FontSize = CurrentCellSize / text.Length,
            };
            player.SetValue(Canvas.LeftProperty, (x + 0.5) * CurrentCellSize - otstup - player.ActualWidth / 2 + thick);
            player.SetValue(Canvas.BottomProperty, (y + 0.5) * CurrentCellSize - otstup - player.ActualHeight / 2 + thick);
            InsideField.Children.Add(player);
        }

        //step button
        private void GoB_Click(object sender, RoutedEventArgs e)
        {
            int fl = gameParent.Step();
            Paint(Field.ActualWidth, Field.ActualHeight);
            if (fl == 1)
            {
                Field.Background = new SolidColorBrush(Color.FromRgb(255, 180, 0));
                MessageBox.Show("Первый Выиграл!");
                this.Close();
            }
            if (fl == 2)
            {
                Field.Background = new SolidColorBrush(Color.FromRgb(255, 180, 220));
                MessageBox.Show("Второй выиграл!");
                this.Close();
            }
        }
    }
}
