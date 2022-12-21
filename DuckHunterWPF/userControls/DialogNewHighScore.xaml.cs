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

        private readonly string FOLDER_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter";
        public readonly string IMAGE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter" + "\\images";
        private readonly string SAVE_FILE = "\\HighScores.xml";

        public DialogNewHighScore()
        {

            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));
            DataContext = this;

            try
            {
                if (!Directory.Exists(FOLDER_PATH))
                {
                    Directory.CreateDirectory(FOLDER_PATH);
                    Debug.WriteLine("Creating `DuckHunter` dir in %appdata%");
                }
                if (!Directory.Exists(IMAGE_PATH))
                {
                    Directory.CreateDirectory(IMAGE_PATH);
                    Debug.WriteLine("Creating `images` dir in %appdata% DuckHunter");
                }
                if (!File.Exists(FOLDER_PATH + SAVE_FILE))
                {
                    File.Create(FOLDER_PATH + SAVE_FILE);
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
                xdoc.Load(FOLDER_PATH + SAVE_FILE);

                XmlNodeList nodes = xdoc.SelectNodes("HighScores/Player");
                XmlElement root = xdoc.DocumentElement;
                Debug.WriteLine(nodes.Count);

                XmlElement savedPlayer = xdoc.CreateElement("Player");
                XmlElement savedPosition = xdoc.CreateElement("Position");
                savedPosition.InnerText = $"{nodes.Count + 1}";
                XmlElement savedUsername = xdoc.CreateElement("Username");
                savedUsername.InnerText = $"{Username}";
                XmlElement savedScore = xdoc.CreateElement("Score");
                savedScore.InnerText = $"{Score.Text}";
                XmlElement savedImage_location = xdoc.CreateElement("Image_location");
                savedImage_location.InnerText = $"{IMAGE_PATH}\\{Username}.png";
                savedPlayer.AppendChild(savedPosition);
                savedPlayer.AppendChild(savedUsername);
                savedPlayer.AppendChild(savedScore);
                savedPlayer.AppendChild(savedImage_location);

                if (nodes.Count < 5)
                {
                    //add
                    

                    root.AppendChild(savedPlayer);
                    xdoc.Save(FOLDER_PATH + SAVE_FILE);
                    //sort
                }
                else
                {
                    //find lowest
                    int savedPlayerScore = int.Parse(Score.Text);
                    XmlNode oldPlayer = null;
                    foreach (XmlNode node in nodes)
                    {
                        
                        XmlNode position = node.SelectSingleNode("Position");
                        XmlNode username = node.SelectSingleNode("Username");
                        XmlNode score = node.SelectSingleNode("Score");
                        XmlNode image_location = node.SelectSingleNode("Image_location");

                        if (savedPlayerScore > int.Parse(score.InnerText)) 
                        {
                            savedPlayerScore = int.Parse(score.InnerText);

                            oldPlayer = node;
                        }
                    }
                    if (oldPlayer != null) {
                        root.ReplaceChild(savedPlayer, oldPlayer);
                        xdoc.Save(FOLDER_PATH + SAVE_FILE);
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
                    File.Copy(openFileDialog.FileName, IMAGE_PATH + $"\\{Username}.png", true);
                
                }
            } 
            catch
            {

            }
        }
    }
}
