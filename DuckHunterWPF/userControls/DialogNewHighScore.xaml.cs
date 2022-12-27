using DuckHunter.Controllers;
using DuckHunter.Models;
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

        private HighScores _highScores = new HighScores();
        private string _currentFile;
        public bool IsOpen
        {
            get { return _highScores.IsOpen; }
            set 
            {
                _highScores.IsOpen = value; 
            }
        }
        public DialogNewHighScore()
        {
            
            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));

            DataContext = _highScores;

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {

            //FileController.EditHighScoresXmlDocument(Username,Score.Text);
            bool hasError = false;
            Debug.WriteLine(_highScores.ImageSource);
            foreach (KeyValuePair<string,string> error in _highScores.ErrorCollection)
            {
                Debug.WriteLine(error);
                if (error.Value != null)
                {
                    hasError = true;
                }   
            }
            if (!hasError)
            {
                Debug.WriteLine("has no Error");
                FileController.EditHighScoresXmlDocument(_highScores.Username, Score.Text);
                try
                {
                    File.Copy(_currentFile, FilePaths.IMAGE_PATH + $"\\{_highScores.Username}.png", true);
                } catch (IOException et)
                {
                    Debug.WriteLine(et.ToString());
                }
                IsOpen = false;
            }

            Debug.WriteLine(_highScores.Error);


        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try { 
                if (_highScores.Username!= null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
                    openFileDialog.FilterIndex = 1;
                    if ((bool)openFileDialog.ShowDialog())
                    {
                        _currentFile = openFileDialog.FileName;
                        if (!File.Exists(FilePaths.IMAGE_PATH + $"\\{_highScores.Username}.png"))
                        {
                            File.Copy(_currentFile, FilePaths.IMAGE_PATH + $"\\{_highScores.Username}.png", true);
                        }
                        _highScores.ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                    }
                }
            } 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
