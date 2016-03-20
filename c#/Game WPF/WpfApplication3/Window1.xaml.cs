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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;


namespace WpfApplication3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MapWindow : Window
    {
        const int MaxX = 10;
        const int MaxY = 10;
        const int MaxCost = 100;

        const double otstup = 5;
        const double thick = 1;

        ColumnDefinition PropertiesCloneColumn;
        TextBox[,] graphicTBs;
        private double CurrentCellSize;

        public MapWindow()
        {
            InitializeComponent();

            PropertiesCloneColumn = new ColumnDefinition();
            PropertiesCloneColumn.SharedSizeGroup = "column";

            PropertiesB.Visibility = Visibility.Collapsed;
            ExternField.ColumnDefinitions.Add(PropertiesCloneColumn);
            Properties.Visibility = Visibility.Visible;

            LoadInit();
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            HelloL.FontSize = (Win.ActualHeight + 2 * Win.ActualWidth) / 70;
            ContinueB.FontSize = (Win.ActualHeight + 2 * Win.ActualWidth) / 180;
            Paint();
        }

        //painting field
        private void Paint()
        {
            InsideField.Children.Clear();
            if (XSizeTB.Text != "" && YSizeTB.Text != "")
            {
                int x = SCM.Int32(XSizeTB);
                int y = SCM.Int32(YSizeTB);

                graphicTBs = new TextBox[x, y];

                CurrentCellSize = (Field.ActualWidth - otstup) / x;
                if ((Field.ActualHeight - otstup) / y < CurrentCellSize)
                    CurrentCellSize = (Field.ActualHeight - otstup) / y;

                InsideField.Width = CurrentCellSize * x;
                InsideField.Height = CurrentCellSize * y;

                for (int i = 0; i < x; i++)
                    for (int j = 0; j < y; j++)
                    {
                        AddRgl(i, j);
                        AddTB(i, j);
                    }
            }
        }

        private void AddTB(int x, int y)
        {
            graphicTBs[x, y] = new TextBox()
            {
                Width = CurrentCellSize / 2,
                HorizontalContentAlignment = HorizontalAlignment.Center,
                FontSize = CurrentCellSize / 6,
                Text = game.GetMapCell(x, y).Price.ToString()
            };

            graphicTBs[x, y].Name = String.Format("a" + x.ToString() + "_" + y.ToString());
            graphicTBs[x, y].TextChanged += graphicTextBoxes_TextChanged;

            graphicTBs[x, y].SetValue(Canvas.LeftProperty, (x + 0.25) * CurrentCellSize - graphicTBs[x, y].ActualWidth / 2 + thick);
            graphicTBs[x, y].SetValue(Canvas.BottomProperty, (y + 0.4) * CurrentCellSize - graphicTBs[x, y].ActualHeight / 2 + thick);
            InsideField.Children.Add(graphicTBs[x, y]);
        }

        private void AddRgl(int x, int y)
        {
            Rectangle rgl = new Rectangle()
            {
                Fill = new SolidColorBrush(Color.FromRgb(255, 255, 0)),
                StrokeThickness = thick,
                Width = CurrentCellSize - 2 * thick,
                Height = CurrentCellSize - 2 * thick,
            };
            rgl.SetValue(Canvas.LeftProperty, x * CurrentCellSize + thick);
            rgl.SetValue(Canvas.BottomProperty, y * CurrentCellSize + thick);
            InsideField.Children.Add(rgl);

        }


        //logic events begin
        private void SizeTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SCM.TBTreating(XSizeTB, MaxX);
            SCM.TBTreating(YSizeTB, MaxY);

            if (XSizeTB.Text != "" && YSizeTB.Text != "")
            {
                game.InitGameMap(SCM.Int32(XSizeTB), SCM.Int32(YSizeTB));
                Paint();
                CostMapTB.Text = "1";
                YMapTB.Text = "1";
                XMapTB.Text = "1";
            }
        }

        private void MapTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SCM.TBTreating(XMapTB, XSizeTB);
            SCM.TBTreating(YMapTB, YSizeTB);
            if (XMapTB.Text != "" && YMapTB.Text != "")
                CostMapTB.Text = game.GetMapCell(SCM.X(XMapTB), SCM.Y(YMapTB)).Price.ToString();
            if (XMapTB.Text == "" && YMapTB.Text == "")
                CostMapTB.Clear();
        }

        private void CostMapTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (XMapTB.Text != "" && YMapTB.Text != "")
            {
                SCM.TBTreating(CostMapTB, MaxCost);
                if (CostMapTB.Text != "")
                {
                    game.InitMapCell(SCM.X(XMapTB), SCM.Y(YMapTB), SCM.Int32(CostMapTB));
                    graphicTBs[SCM.X(XMapTB), SCM.Y(YMapTB)].Text = CostMapTB.Text;
                }
            }
            else
                CostMapTB.Clear();
        }

        private void graphicTextBoxes_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {
                SCM.TBTreating(currentTextBox, MaxCost);
                if (currentTextBox.Text != "")
                {
                    string[] indexes = currentTextBox.Name.Split('_', 'a');
                    int x = Convert.ToInt32(indexes[1]);
                    int y = Convert.ToInt32(indexes[2]);
                    game.InitMapCell(x, y, SCM.Int32(currentTextBox));
                    if (XMapTB.Text == indexes[1] && YMapTB.Text == indexes[2])
                        CostMapTB.Text = game.GetMapCell(x, y).Price.ToString();
                }
            }
        }                               
        
        private void XYEndTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            SCM.TBTreating(XEndTB, XSizeTB);
            SCM.TBTreating(YEndTB, YSizeTB);
            if (XEndTB.Text != "" && YEndTB.Text != "")
            {
                game.End = new cell(SCM.X(XEndTB), SCM.Y(YEndTB));
            }
        }

        private void ContinueB_Click(object sender, RoutedEventArgs e)
        {
            if (game.IsMapInitiated && game.IsEndInitiated)
            {
                Window2 a = new Window2();
                a.Activate();
                a.Visibility = Visibility.Visible;
                this.Close();
            }
        }
        

        // slider's events
        private void PropertiesB_Click(object sender, RoutedEventArgs e)
        {
            if (Properties.Visibility == Visibility.Visible)
                Properties.Visibility = Visibility.Collapsed;
            else
                Properties.Visibility = Visibility.Visible;
        }

        private void PropertiesPinB_Click(object sender, RoutedEventArgs e)
        {
            if (PropertiesB.Visibility == Visibility.Visible)
            {
                PropertiesB.Visibility = Visibility.Collapsed;
                ExternField.ColumnDefinitions.Add(PropertiesCloneColumn);
                Paint();
                Properties.Visibility = Visibility.Visible;
            }
            else
            {
                PropertiesB.Visibility = Visibility.Visible;
                ExternField.ColumnDefinitions.Remove(PropertiesCloneColumn);
                Paint();
            }
        }

        private void ExternField_MouseDown(object sender, MouseEventArgs e)
        {
            if (PropertiesB.Visibility == Visibility.Visible)
                Properties.Visibility = Visibility.Collapsed;
        }

       
    }
}
