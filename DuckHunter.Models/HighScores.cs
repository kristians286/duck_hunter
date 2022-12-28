using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace DuckHunter.Models
{
    public class HighScores : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private BitmapImage _imageSource;
        private bool _isOpen;

        public bool IsOpen // Only used for DialogNewHighScore
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
                _username = value;
                if (File.Exists( FilePaths.IMAGE_PATH + $"\\{_username}.png")) 
                {
                    //ImageSource = new BitmapImage(new Uri (FilePaths.IMAGE_PATH + $"\\{_username}.png"));

                    BitmapImage image = new BitmapImage();
                    image.BeginInit();
                    image.CacheOption = BitmapCacheOption.OnLoad;
                    image.UriSource = new Uri(FilePaths.IMAGE_PATH + $"\\{_username}.png");
                    image.EndInit();
                    ImageSource = image;
                } else
                {
                    ImageSource = null;
                }
                OnPropertyChanged("Username");
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
                            if (ImageSource == null)
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

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
