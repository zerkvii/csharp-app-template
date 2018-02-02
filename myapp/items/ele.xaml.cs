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

namespace myapp.items
{
    /// <summary>
    /// Interaction logic for ele.xaml
    /// </summary>
    public partial class ele : UserControl
    {

        public double x_label { get; set; }
        public double y_label { get; set; }
        public int delay { get; set; }
        public int color { get; set; }
        public ele(Point p_)
        {
            InitializeComponent();
            this.color = 0;
            this.delay = 2;
           
        }
    
   
        public void change_color(Color color)
        {
            this.el1.Fill = new SolidColorBrush(color);
        }

    }
}
