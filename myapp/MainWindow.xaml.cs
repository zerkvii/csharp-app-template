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

namespace myapp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Boolean is_max { set; get; }
        public MainWindow()
        {
            InitializeComponent();
            is_max = false;
        }

        private void drag(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void close_wd(object sender, MouseButtonEventArgs e)
        {
            //this.Close();
            Application.Current.Shutdown();
        }

        private void max_wd(object sender, MouseButtonEventArgs e)
        {
            if (!is_max)
            {
                this.WindowState = WindowState.Maximized;
                is_max = true;
            }
            else
            {
                this.WindowState = WindowState.Normal;
                is_max = false;
            }
        }

        private void min_wd(object sender, MouseButtonEventArgs e)
        {
            this.WindowState = WindowState.Minimized;

        }
    }
}
