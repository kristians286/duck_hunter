using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Xml;

namespace DuckHunterWPF.userControls
{
    public partial class DialogNewHighScore : UserControl
    {

        public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register("IsOpen", typeof(bool), typeof(DialogNewHighScore), new PropertyMetadata(true));

        public static readonly DependencyProperty UsernameProperty =
        DependencyProperty.Register("Username", typeof(string), typeof(DialogNewHighScore), new PropertyMetadata(""));

        public static readonly DependencyProperty ScoreProperty =
        DependencyProperty.Register("Score", typeof(int), typeof(DialogNewHighScore), new PropertyMetadata(0));
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

        public int Score
        {
            get { return (int)GetValue(ScoreProperty); }
            set { SetValue(ScoreProperty, value); }
        }

        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter";
        private readonly string _imagePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter" + "\\images";
        private readonly string _fileName = "\\HighScores.xml";
        public DialogNewHighScore()
        {

            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));
            DataContext = this;

            try
            {
                if (!Directory.Exists(_path))
                {
                    Directory.CreateDirectory(_path);
                    Debug.WriteLine("Creating `DuckHunter` dir in %appdata%");
                }
                if (!Directory.Exists(_imagePath))
                {
                    Directory.CreateDirectory(_imagePath);
                    Debug.WriteLine("Creating `images` dir in %appdata% DuckHunter");
                }
                if (!File.Exists(_path + @_fileName))
                {
                    File.Create(_path + @_fileName);
                    Debug.WriteLine("Creating `HighScores.xml` file");
                }


            } catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                /*
                XmlDocument doc = new XmlDocument();

                XmlNode root = doc.CreateElement("HighScores");
                doc.AppendChild(root);

                XmlNode player = doc.CreateElement("player");
                
                XmlAttribute username = doc.CreateAttribute("username");
                username.Value = Username;
                player.Attributes.Append(username);

                XmlAttribute score = doc.CreateAttribute("score");
                score.Value = Score.ToString();
                player.Attributes.Append(score);

                XmlAttribute img_location = doc.CreateAttribute("img");
                img_location.Value = _imagePath + $"\\{Username}.png";
                player.Attributes.Append(img_location);

                root.AppendChild(player);

                doc.Save(_path + @_fileName);
                */
                
                XmlTextWriter tw = new XmlTextWriter(_path + @_fileName, null);
                tw.Formatting = Formatting.Indented;
                tw.WriteStartDocument();

                tw.WriteStartElement("HighScores");
                for (int i = 0; i < 10; i++) 
                {
                    tw.WriteStartElement("Player");
                    tw.WriteElementString("Username", Username);
                    tw.WriteElementString("Score", Score.ToString());
                    tw.WriteElementString("Image_location", _imagePath + $"\\{Username}.png");
                    tw.WriteEndElement();
                }



                tw.WriteEndElement();
                tw.WriteEndDocument();
                tw.Flush();
                tw.Close();
                
            } catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            Debug.WriteLine(Score);
            Debug.WriteLine(Username);

            IsOpen= false;
        }

        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
            openFileDialog.FilterIndex = 1;
            if (openFileDialog.ShowDialog() == true)
            {
                UserImage.Source = new BitmapImage(new Uri(openFileDialog.FileName));
                File.Copy(openFileDialog.FileName, _imagePath + $"\\{Username}.png", true);
            }
        }
    }
}
