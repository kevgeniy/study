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
    /// Interaction logic for saveWindow.xaml
    /// </summary>
    public partial class saveWindow : Window
    {
        public string str;
        MapWindow parent;

        public saveWindow(MapWindow parent)
        {
            InitializeComponent();
            this.parent = parent;
        }

        private void saveNameTB_TextChanged(object sender, TextChangedEventArgs e)
        {
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            parent.Save(saveNameTB.Text);
            parent.LoadInit();
            parent.Visibility = Visibility.Visible;
        }
    }
}
