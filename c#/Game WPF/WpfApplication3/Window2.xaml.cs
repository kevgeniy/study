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
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        ColumnDefinition clonePlayer1Column;
        ColumnDefinition clonePlayer2Column1;
        ColumnDefinition clonePlayer2Column2;

        const int MaxPlayer1Life = 100;
        const int MaxPlayer2Life = 100;
        const int MaxPlayer2Number = 10;
        const int otstup = 5;
        int thick = 1;
        double CurrentCellSize;

        TextBlock[,] graphicTBLs;

        public Window2()
        {
            InitializeComponent();
            clonePlayer1Column = new ColumnDefinition();
            clonePlayer1Column.SharedSizeGroup = "player1Column";
            clonePlayer2Column1 = new ColumnDefinition();
            clonePlayer2Column1.SharedSizeGroup = "player2Column";
            clonePlayer2Column2 = new ColumnDefinition();
            clonePlayer2Column2.SharedSizeGroup = "player2Column";
            List<int> a = new List<int>();
            for (int i = 1; i <= MaxPlayer2Number; i++)
                a.Add(i);
            Player2NumberOfChipsLB.ItemsSource = a;
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


        //logic events begin
        private void Player1TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SCM.TBTreating(XPlayer1TB, game.MapSizeX);
            SCM.TBTreating(YPlayer1TB, game.MapSizeY);
            SCM.TBTreating(LifePlayer1TB, MaxPlayer1Life);

            if (XPlayer1TB.Text != "" && YPlayer1TB.Text != "" && LifePlayer1TB.Text != "")
                game.InitPlayer1(SCM.X(XPlayer1TB), SCM.Y(YPlayer1TB), SCM.Int32(LifePlayer1TB));
        }

        private void Player2NumberOfChips_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            List<int> a = new List<int>();
            for (int i = 1; i <= Convert.ToInt32(Player2NumberOfChipsLB.SelectedValue); i++)
                a.Add(i);
            Player2CurrentChipLB.ItemsSource = a;

            game.InitPlayer2Count(Convert.ToInt32(Player2NumberOfChipsLB.SelectedValue));

            XPlayer2TB.Clear();
            YPlayer2TB.Clear();
            LifePlayer2TB.Clear();
        }

        private void Player2CurrentChip_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            cell a = game.GetPlayer2CurrentState(Convert.ToInt32(Player2CurrentChipLB.SelectedValue));
            XPlayer2TB.Text = (SCM.InvX(a.X)).ToString();
            YPlayer2TB.Text = (SCM.InvY(a.Y)).ToString();
        }  //check

        private void XYPlayer2TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (Player2CurrentChipLB.SelectedValue == null)
            {
                XPlayer2TB.Clear();
                YPlayer2TB.Clear();
            }
            else
            {
                SCM.TBTreating(XPlayer2TB, game.MapSizeX);
                SCM.TBTreating(YPlayer2TB, game.MapSizeY);
            }
            if (XPlayer2TB.Text == "" || YPlayer2TB.Text == "")
                LifePlayer2TB.Clear();
        }

        private void LifePlayer2TB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (XPlayer2TB.Text != "" && YPlayer2TB.Text != "")
            {
                SCM.TBTreating(LifePlayer2TB, MaxPlayer2Life);
                if (LifePlayer2TB.Text != "")
                    game.InitPlayer2(Convert.ToInt32(Player2CurrentChipLB.SelectedValue), SCM.X(XPlayer2TB), SCM.Y(YPlayer2TB), SCM.Int32(LifePlayer2TB));
            }
            else
                LifePlayer2TB.Clear();
        }

        private void ContinueB_Click(object sender, RoutedEventArgs e)
        {
            if (game.IsPlayer1Initiated && game.IsPlayer2Initiated)
            {
                Window3 a = new Window3();
                a.Activate();
                a.Visibility = Visibility.Visible;
                this.Close();
            }
        }


        // slider's events
        private void Palyer1PinB_Click(object sender, RoutedEventArgs e)
        {
            if (Player1B.Visibility == Visibility.Visible)
            {
                Player1B.Visibility = Visibility.Collapsed;
                ExternField.ColumnDefinitions.Add(clonePlayer1Column);
                Player1G.Visibility = Visibility.Visible;
            }
            else
            {
                Player1B.Visibility = Visibility.Visible;
                Player1G.Visibility = Visibility.Collapsed;
                ExternField.ColumnDefinitions.Remove(clonePlayer1Column);
            }
        }

        private void Player2PinB_Click(object sender, RoutedEventArgs e)
        {
            if (Player2B.Visibility == Visibility.Visible)
            {
                Player2B.Visibility = Visibility.Collapsed;
                Player1G.ColumnDefinitions.Add(clonePlayer2Column2);
                ExternField.ColumnDefinitions.Add(clonePlayer2Column1);
                Player2G.Visibility = Visibility.Visible;
            }
            else
            {
                Player2B.Visibility = Visibility.Visible;
                Player2G.Visibility = Visibility.Collapsed;
                ExternField.ColumnDefinitions.Remove(clonePlayer2Column1);
                Player1G.ColumnDefinitions.Remove(clonePlayer2Column2);
            }
        }

        private void Player1B_Click(object sender, RoutedEventArgs e)
        {
            if (Player1G.Visibility == Visibility.Visible)
                Player1G.Visibility = Visibility.Collapsed;
            else
            {
                Player1G.Visibility = Visibility.Visible;
                Grid.SetZIndex(Player1G, 1);
                Grid.SetZIndex(Player2G, 0);
                if (Player2B.Visibility == Visibility.Visible)
                    Player2G.Visibility = Visibility.Collapsed;
            }
        }

        private void Player2B_Click(object sender, RoutedEventArgs e)
        {
            if (Player2G.Visibility == Visibility.Visible)
                Player2G.Visibility = Visibility.Collapsed;
            else
            {
                Player2G.Visibility = Visibility.Visible;
                Grid.SetZIndex(Player2G, 1);
                Grid.SetZIndex(Player1G, 0);
                if (Player1B.Visibility == Visibility.Visible)
                    Player1G.Visibility = Visibility.Collapsed;
            }
        }

        private void ExternField_MouseDown(object sender, MouseEventArgs e)
        {
            if (Player1B.Visibility == Visibility.Visible)
                Player1G.Visibility = Visibility.Collapsed;
            if (Player2B.Visibility == Visibility.Visible)
                Player2G.Visibility = Visibility.Collapsed;
        }
    }      

}
