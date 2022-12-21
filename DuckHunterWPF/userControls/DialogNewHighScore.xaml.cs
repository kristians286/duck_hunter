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

        private readonly string _path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter";
        public readonly string _imagePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter" + "\\images";
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

                XmlDocument xdoc = new XmlDocument();
                xdoc.Load(_path + @_fileName);

                XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");
                XmlElement root = xdoc.DocumentElement;
                Debug.WriteLine(nodes.Count);
                if (nodes.Count < 5)
                {
                    //add
                    XmlElement player = xdoc.CreateElement("Player");
                    XmlElement position = xdoc.CreateElement("Position");
                    position.InnerText = $"{nodes.Count+1}";
                    XmlElement username = xdoc.CreateElement("Username");
                    username.InnerText = $"{Username}";
                    XmlElement score = xdoc.CreateElement("Score");
                    score.InnerText = $"{Score.Text}";
                    XmlElement image_location = xdoc.CreateElement("Image_location");
                    image_location.InnerText = $"{_imagePath}\\{Username}.png";
                    player.AppendChild(position);
                    player.AppendChild(username);
                    player.AppendChild(score);
                    player.AppendChild(image_location);

                    root.AppendChild(player);
                    xdoc.Save(_path + @_fileName);
                    //sort
                }
                else
                {
                    //find lowest
                    foreach (XmlNode node in nodes)
                    {
                        XmlNode position = node.SelectSingleNode("Position");
                        XmlNode username = node.SelectSingleNode("Username");
                        XmlNode score = node.SelectSingleNode("Score");
                        XmlNode image_location = node.SelectSingleNode("Image_location");
                        Debug.WriteLine(position.InnerText);
                        Debug.WriteLine(username.InnerText);
                        Debug.WriteLine(score.InnerText);
                        Debug.WriteLine(image_location.InnerText);
                    }
                    /*
                        //replace
                        //sort
                    }

                    XmlTextWriter tw = new XmlTextWriter(_path + @_fileName, null);
                    tw.Formatting = Formatting.Indented;
                    tw.WriteStartDocument();

                    tw.WriteStartElement("HighScores");
                    for (int i = 0; i < 5; i++) 
                    {
                        tw.WriteStartElement("Player");
                        tw.WriteElementString("Position", (i+1).ToString());
                        tw.WriteElementString("Username", Username);
                        tw.WriteElementString("Score", Score.Text);
                        tw.WriteElementString("Image_location", _imagePath + $"\\{Username}.png");
                        tw.WriteEndElement();
                    }



                    tw.WriteEndElement();
                    tw.WriteEndDocument();
                    tw.Flush();
                    tw.Close();
                    */
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
                
            //IsOpen = false;
            
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
                    File.Copy(openFileDialog.FileName, _imagePath + $"\\{Username}.png", true);
                
                }
            } 
            catch
            {

            }
        }
    }
}
