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
    public partial class DialogHighScores : UserControl
    {

        public static readonly DependencyProperty IsHSOpenProperty =
            DependencyProperty.Register("IsHSOpen", typeof(bool), typeof(DialogHighScores), new PropertyMetadata(false));

        public Uri Path = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter\\HighScores.xml");

        //private List<String,String,String> HighScoresList = new List<>(); Model HighScores
        public EventHandler eventt ;
        public bool IsHSOpen
        {
            get { return (bool)GetValue(IsHSOpenProperty); }
            set { SetValue(IsHSOpenProperty, value); }
        }
 
        public DialogHighScores()
        {
            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogHighScores), new FrameworkPropertyMetadata(typeof(DialogHighScores)));
            DataContext = this;
            (Resources["HighScores"] as XmlDataProvider).Source = new Uri(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\DuckHunter\\HighScores.xml");
        }

        public void Refresh()
        {
            XmlDataProvider xmlData = FindResource("HighScores") as XmlDataProvider;
            xmlData.Refresh();
        }
    }
}
