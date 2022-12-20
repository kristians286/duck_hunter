using System;
using System.Collections.Generic;
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

        public static readonly DependencyProperty IsOpenProperty =
            DependencyProperty.Register("IsHSOpen", typeof(bool), typeof(DialogNewHighScore), new PropertyMetadata(false));

        public static readonly DependencyProperty PositionProperty =
            DependencyProperty.Register("PositionProperty", typeof(int), typeof(DialogHighScores), new PropertyMetadata(0));
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public int Position
        {
            get { return (int)GetValue(PositionProperty); }
            set { SetValue(PositionProperty, value); }
        }
        public DialogHighScores()
        {
            InitializeComponent();
            DataContext = this;
        }
    }
    public class ListHighScores
    {
    }
}
