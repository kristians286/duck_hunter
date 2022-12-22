using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace DuckHunter.Models
{
    public class HighScores : INotifyPropertyChanged, IDataErrorInfo
    {
        private string _username;
        private string _imageSource;

        public string Username
        {
            get { return _username; }
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string ImageSource
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
                        if (string.IsNullOrWhiteSpace(ImageSource))
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
