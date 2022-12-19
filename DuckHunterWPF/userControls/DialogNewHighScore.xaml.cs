using System;
using System.Collections.Generic;
using System.Diagnostics;
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


        public DialogNewHighScore()
        {

            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));
            DataContext = this;

        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine(Score);
            Debug.WriteLine(Username);
        }
    }
}
