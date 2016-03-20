using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
//using System.Numerics;
using System.Diagnostics;
using System.Drawing;
using System.IO;
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

namespace ImageControl {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>

	public class Picture {
		private bool modified = false;
		public string Size { get { return Width.ToString() + "x" + Height.ToString();  } }
		public int Width { get; set; }
		public int Height { get; set; }
		public bool Modified { get { return modified; } set { modified = value; } }
		public BitmapImage BmpImage { get; set; }
		public string FileName { get; set; }

		public BitmapImage Icon { get; set; }
		public Picture(BitmapImage bmp, BitmapImage icon, int width, int height, string fileName) {
			BmpImage = bmp;
			Icon = icon;
			Width = width;
			Height = height;
			FileName = fileName;
		}
	}
	public partial class MainWindow : Window {
		private string initialDirictory = @"D:\Pictures";
		public Picture currentPicture = null;
		public MainWindow() {
			InitializeComponent();
		}
		private void UpdateImage() {
			pictureImage.Source = currentPicture.BmpImage;
		}
		private void Help_Click(object sender, RoutedEventArgs e) {
			Window1 helpWin = new Window1();
			helpWin.Activate();
			helpWin.Visibility = Visibility.Visible;
		}

		private void Version_Click(object sender, RoutedEventArgs e) {
			MessageBox.Show("version 0.1");
		}

		private void Open_Click(object sender, RoutedEventArgs e) {
			Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();

			dlg.InitialDirectory = initialDirictory;
			dlg.FileName = "Select...";
			dlg.DefaultExt = ".bmp, .jpeg, .gif";
			dlg.Filter = "Image files|*.jpeg;*.jpg;*.JPG;*.bmp;|All files| *.*";
			dlg.Multiselect = true;

			Nullable<bool> result = dlg.ShowDialog();

			if (result == true) {
				string[] fileNames = dlg.FileNames;
				foreach (var item in fileNames) {
					BitmapImage bmp = new BitmapImage();
					BitmapImage icon = new BitmapImage();
	
					bmp.BeginInit();
					bmp.UriSource = new Uri(item);
					bmp.EndInit();

					icon.BeginInit();
					icon.UriSource = new Uri(item);
					icon.DecodePixelWidth = 83;
					icon.DecodePixelHeight = 70;
					icon.EndInit();

					Picture picture = new Picture(bmp, icon, bmp.PixelWidth, bmp.PixelHeight, item);
					pictureListBox.Items.Add(picture);
				}
			}
		}

		private void pictureListBox_SelectionChanged(object sender, SelectionChangedEventArgs e) {
			currentPicture = (Picture)pictureListBox.SelectedItem;
			UpdateImage();
		}

		private void Save_Click(object sender, RoutedEventArgs e) {
			currentPicture.Modified = false;
		}

		private void SaveAs_Click(object sender, RoutedEventArgs e) {
			Microsoft.Win32.SaveFileDialog dialog = new Microsoft.Win32.SaveFileDialog();
			dialog.Filter = "*.jpg;*.jpeg;*.JPG|*.jpg;|*.bmp|*.bmp";
			dialog.ShowDialog();

			if (dialog.FileName != "") {
				currentPicture.FileName = dialog.FileName;
				Save_Click(sender, e);
			}
		}
		private void CloseImg_Click(object sender, RoutedEventArgs e) {

		}
		private void Exit_Click(object sender, RoutedEventArgs e) {

		}
	}
}
