using DuckHunter.Controllers;
using DuckHunter.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace DuckHunterWPF.DialogControls
{
    public partial class DialogNewHighScore : UserControl, INotifyPropertyChanged, IDataErrorInfo
    {

        
        private string _selectedFile;

        private string _username;
        private BitmapImage _imageSource;
        private bool _isOpen;

        private static BitmapImage DEFAULT_IMAGE = new BitmapImage(new Uri("pack://application:,,,/Images/noimage.png"));


        public bool IsOpen
        {
            get { return _isOpen; }
            set
            {
                _isOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        public string Username
        {
            get { return _username; }
            set
            {
                if (_username!= value)
                {
                    _username = value;
                    if (FileController.PlayerExistsInXml(_username))
                    {
                        //ImageSource = new BitmapImage(new Uri (FilePaths.IMAGE_PATH + $"\\{_username}.png"));
                        try
                        {
                            BitmapImage image = new BitmapImage();
                            image.BeginInit();
                            image.CacheOption = BitmapCacheOption.OnLoad;
                            image.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
                            image.UriSource = new Uri(FileController.GetPlayerImageFromXml(_username));
                            image.EndInit();
                            ImageSource = image;
                        }
                        catch
                        {

                        }


                    }
                    else
                    {
                        ImageSource = DEFAULT_IMAGE;
                    }
                    OnPropertyChanged("Username");
                }
            }
        }

        public BitmapImage ImageSource
        {
            get { return _imageSource; }
            set
            {
                _imageSource = value;
                OnPropertyChanged("ImageSource");
            }
        }

        public Dictionary<string, string> ErrorCollection { get; private set; } = new Dictionary<string, string>();
        public string Error
        {
            get
            {
                return "";
            }
        }

        public string this[string name]
        {
            get
            {
                string result = null;
                switch (name)
                {
                    case "Username":
                        if (string.IsNullOrWhiteSpace(Username) || Username.Contains(" "))
                        {
                            result = "Username cant be empty or have spacebars\n";
                        }

                        if (ErrorCollection.ContainsKey(name))
                        {
                            ErrorCollection[name] = result;
                        }
                        else if (result != null)
                        {
                            ErrorCollection.Add(name, result);
                        }

                        OnPropertyChanged("ErrorCollection");
                        break;
                    case "ImageSource":
                        try
                        {
                            if (ImageSource == null || ImageSource == DEFAULT_IMAGE )
                            {
                                result = "Please upload your passport";
                            }
                            if (ErrorCollection.ContainsKey(name))
                            {
                                ErrorCollection[name] = result;
                            }
                            else if (result != null)
                            {
                                ErrorCollection.Add(name, result);
                            }
                            OnPropertyChanged("ErrorCollection");
                        }
                        catch
                        {

                        }

                        break;
                }

                return result;
            }
        }


        public DialogNewHighScore()
        {
            
            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));
            
            DataContext = this;

            
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var uuid = Guid.NewGuid().ToString();
                var image_path = FilePaths.IMAGE_PATH + $"\\{uuid}.png";
                bool hasError = false;
                foreach (KeyValuePair<string,string> error in ErrorCollection)
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
                   
                
               
                    if (_selectedFile != null)
                    {
                        File.Copy(_selectedFile, image_path, true);
                        _selectedFile = null;
                    } else
                    {
                        image_path = ImageSource.UriSource.ToString();
                    }
                    var player = new HighScore("", Username, Score.Text, image_path);
                    FileController.AddHighScore(player);

                    
                    Username = "";
                    IsOpen = false;
                }
            
                Debug.WriteLine(Error);

            }
            catch (Exception exc)
            {
                FileController.LogException(exc);
            }

        }
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            try { 
                if (Username!= null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Filter = "Image files|*.bmp;*.jpg;*.png";
                    openFileDialog.FilterIndex = 1;
                    if ((bool)openFileDialog.ShowDialog())
                    {
                        ImageSource = new BitmapImage(new Uri(openFileDialog.FileName));
                        _selectedFile = openFileDialog.FileName;
                    }
                }
            } 
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
