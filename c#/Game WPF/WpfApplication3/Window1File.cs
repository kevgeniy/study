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
using Microsoft.Win32;

namespace WpfApplication3
{
    partial class MapWindow : Window
    {
        const string path = @"D:\Programming\projects\c#\2013\Game WPF\WpfApplication3\Save\";
        string saveName;


        //New map
        private void NewB_Click(object sender, RoutedEventArgs e)
        {
            XSizeTB.Clear();
            YSizeTB.Clear();
            XMapTB.Clear();
            CostMapTB.Clear();
            XEndTB.Clear();
            YEndTB.Clear();
            InsideField.Children.Clear();
            game.Clear();
        }

        //Load map
        private void Load(string name)
        {
            FileStream a = new FileStream(path + name + ".txt", FileMode.Open);
            StreamReader str = new StreamReader(a);
            string[] ints = str.ReadLine().Split(' ');
            ints = str.ReadLine().Split(' ');
            XSizeTB.Text = ints[0];
            YSizeTB.Text = ints[1];
            for (int i = SCM.Y(YSizeTB); i >= 0; i--)
            {
                ints = str.ReadLine().Split(' ');
                for (int j = 0; j < SCM.Int32(XSizeTB); j++)
                    game.InitMapCell(i, j, Convert.ToInt32(ints[j]));
            }
            XMapTB.Text = "1";
            YMapTB.Text = "1";
            ints = str.ReadLine().Split();
            XEndTB.Text = ints[0];
            YEndTB.Text = ints[1];
        }

        public void LoadInit()
        {
            OpenMU.Items.Clear();
            StreamReader file = new StreamReader(path + "0.txt");
            string[] ints = file.ReadLine().Split();
            int number = Convert.ToInt32(ints[0]);
            List<int> files = new List<int>();
            file.Close();
            for (int i = 1; i <= number; i++)
            {
                file = new StreamReader(path + i.ToString() + ".txt");
                TextBlock a = new TextBlock()
                {
                    Text = file.ReadLine(),
                    Name = String.Format("_" + i.ToString())
                };
                a.MouseDown += Item_Click;
                OpenMU.Items.Add(a);
                file.Close();
            }
        }

        private void Item_Click(object sender, EventArgs e)
        {
            TextBlock currentTextBlock = sender as TextBlock;
            if (sender != null)
            {
                string[] names = currentTextBlock.Name.Split('_');
                Load(names[1]);
            }
        }

        private void SaveB_Click(object sender, RoutedEventArgs e)
        {
            if (saveName == null)
                SaveAsB_Click(sender, e);
            else
            {
                Save(saveName);
                LoadInit();
            }
        }

        private void SaveAsB_Click(object sender, RoutedEventArgs e)
        {
            saveWindow a = new saveWindow(this);
            a.Activate();
            a.Visibility = Visibility.Visible;
            this.Visibility = Visibility.Collapsed;
            /*
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Document";
            dlg.DefaultExt = ".text";
            dlg.Filter = "Text documents (.txt)|*.txt";

            if (dlg.ShowDialog() == true)
            {
                saveName = dlg.FileName;
                Save(saveName);
            }
            LoadInit();*/
        }

        public void Save(string name)
        {
            if (game.IsMapInitiated && game.IsEndInitiated)
            {
                StreamReader read = new StreamReader(path + "0.txt");
                string[] ints = read.ReadLine().Split();
                read.Close();
                StreamWriter file = new StreamWriter(path + "0.txt");
                file.WriteLine(Convert.ToInt32(ints[0]) + 1);
                file.Close();

                file = new StreamWriter(path + (Convert.ToInt32(ints[0]) + 1).ToString() + ".txt");
                file.WriteLine(name);
                file.WriteLine(XSizeTB.Text + " " + YSizeTB.Text);
                for(int i = game.MapSizeY - 1; i >= 0; i--)
                {
                    for (int j = 0; j < game.MapSizeX; j++)
                    {
                        file.Write(game.GetMapCell(j, i).Price);
                        if (j != game.MapSizeX - 1)
                            file.Write(" ");
                    }
                    file.WriteLine();
                }
                file.Write(game.End.X.ToString() + " " + game.End.Y);
                file.Close();
                }
            else
                MessageBox.Show("Некорректная карта!");
        }
    }
}
