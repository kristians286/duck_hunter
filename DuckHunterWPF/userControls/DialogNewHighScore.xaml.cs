using DuckHunter.Controllers;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
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
using System.Xml;
using System.Xml.Linq;

namespace DuckHunterWPF.userControls
{
    public partial class DialogNewHighScore : UserControl
    {

        public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register("IsOpen", typeof(bool), typeof(DialogNewHighScore), new PropertyMetadata(false));

        public static readonly DependencyProperty UsernameProperty =
        DependencyProperty.Register("Username", typeof(string), typeof(DialogNewHighScore), new PropertyMetadata(""));

        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public string Username
        {
            get { return (string)GetValue(UsernameProperty); }
            set { SetValue(UsernameProperty, value); }
        }

        public DialogNewHighScore()
        {

            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));
            DataContext = this;

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            FileController.EditHighScoresXmlDocument(Username,Score.Text);
                
            IsOpen = false;
            
        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try { 
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
                openFileDialog.FilterIndex = 1;
                if (openFileDialog.ShowDialog() == true)
                {
                    UserImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                    File.Copy(openFileDialog.FileName, FileController.IMAGE_PATH + $"\\{Username}.png", true);
                }
            } 
            catch (Exception exc)
            {
                Debug.WriteLine(e);
            }
        }
    }
}
