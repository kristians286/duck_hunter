using DuckHunter.Controllers;
using DuckHunter.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace DuckHunterWPF.userControls
{
    /// <summary>
    /// Interaction logic for DialogHighScores.xaml
    /// </summary>
    public partial class DialogHighScores : UserControl, INotifyPropertyChanged
    {

        public static readonly DependencyProperty IsHSOpenProperty =
            DependencyProperty.Register("IsHSOpen", typeof(bool), typeof(DialogHighScores), new PropertyMetadata(false));

        public Uri Path = new Uri(FilePaths.FOLDER_PATH + FilePaths.SAVE_FILE);


        private ObservableCollection<HighScores> _highScoresList;
        public ObservableCollection<HighScores> HighScoresList
        {
            get { return _highScoresList; }
            set 
            {
                _highScoresList = value;
                OnPropertyChanged("HighScoresList");
            }  
        } 


        public bool IsHSOpen
        {
            get { return (bool)GetValue(IsHSOpenProperty); }
            set 
            { 
                SetValue(IsHSOpenProperty, value);
                LoadData();
            }
        }
 
        public DialogHighScores()
        {
            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHighScores), new FrameworkPropertyMetadata(typeof(DialogHighScores)));
            DataContext = this;
        }

        public void LoadData()
        {
            try
            {
                if (IsHSOpen)
                {
                    HighScoresList = FileController.GetHighScoresListFromXml();
                }
                else
                {
                    HighScoresList = null;
                }
            } catch
            {

            }
        }
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
