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
    /// <summary>
    /// Interaction logic for DialogNewHighScore.xaml
    /// </summary>
    public partial class DialogNewHighScore : UserControl
    {

        public static readonly DependencyProperty IsOpenProperty =
        DependencyProperty.Register("IsOpen", typeof(bool), typeof(DialogNewHighScore), new PropertyMetadata(false));
        public bool IsOpen
        {
            get { return (bool)GetValue(IsOpenProperty); }
            set { SetValue(IsOpenProperty, value); }
        }
        public DialogNewHighScore()
        {

            InitializeComponent();
            DefaultStyleKeyProperty.OverrideMetadata(typeof(DialogNewHighScore), new FrameworkPropertyMetadata(typeof(DialogNewHighScore)));
            DataContext = this;
            Debug.WriteLine(DataContext);

        }

    }
}
