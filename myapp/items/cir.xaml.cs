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

namespace xym_.content
{
    /// <summary>
    /// circle.xaml 的交互逻辑
    /// </summary>
    /// 
    public partial class cir : UserControl
    {
        public double x_label { get; set; }
        public double y_label { get; set; }
        public int delay { get; set; }
        public int color { get; set; }
        public cir(Point p_)
        {
            InitializeComponent();
            this.color = 0;
            this.delay = 2;
            this.x_label = p_.X;
            this.y_label = p_.Y;
        }
        public void change_color(Color color)
        {
            this.elp.Fill = new SolidColorBrush(color);
        }
       
    }
}
