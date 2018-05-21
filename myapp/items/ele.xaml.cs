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

        public int x_label { get; set; }
        public int y_label { get; set; }
        public Point cache_point { get; set; }
        public int delay { get; set; }
        public int duration { get; set; }
        public int color { get; set; }
        public int state { get; set; }
        public int group_id { get; set; }
        public bool is_grouped { get; set; }

        public ele(Point p_,int index)
        {
            InitializeComponent();
            this.is_grouped = false;
            this.group_id = 0;
            this.color = 0;
            this.delay = 2;
            this.state = 0;
            this.duration = 3;
            this.cache_point = new Point(p_.X - 20, p_.Y - 20);
            this.x_label = Convert.ToInt32(p_.X);
            this.y_label = Convert.ToInt32(p_.Y);
            this.border.Fill = new SolidColorBrush(Colors.Transparent);
            this.index_label.Text = index.ToString();

        }

        public void refresh()
        {
            this.group_id = 0;
            this.color = 0;
            this.delay = 2;
            this.state = 0;
            this.duration = 3;
            this.reset_color();
            this.x_label = Convert.ToInt32(cache_point.X);
            this.y_label = Convert.ToInt32(cache_point.Y);
        }

        public void change_delay(int val)
        {
            if (val >= 0)
                this.delay = val;
            else
            {
                //warning
            }
        }
        public void change_group_id(int val)
        {
            if (val >= 0)
            {
                this.is_grouped = true;
                this.group_id = val;
            }
            else
            {
                //warning
            }
        }

        public void change_duration(int val)
        {
            if (val >= 0)
                this.duration = val;
            else
            {
                //warning
            }
        }

        public void change_color(Color color)
        {
            this.el1.Fill = new SolidColorBrush(color);
        }
        //change the color of dif state of bomb
        public void set_bam_s()
        {
            if (this.state != 1)
            {
                this.state = 1;
                this.el2.Fill = new SolidColorBrush(Color.FromRgb(95, 95, 95));
                this.el3.Fill = new SolidColorBrush(Color.FromRgb(95, 95, 95));
            }
            else
            {
                reset_color();
            }
        }
        public void set_bam_a()
        {
            if (this.state != 2)
            {
                this.state = 2;
                this.el2.Fill = new SolidColorBrush(Color.FromRgb(95, 95, 95));
                this.el3.Fill = new SolidColorBrush(Color.FromRgb(195,195,195));
            }
            else
            {
                reset_color();
            }
        }
        public void set_st(int st)
        {
            if (st == 2) set_bam_a();
            else if (st == 1) set_bam_s();
            else reset_color();
        }

        public void reset_color(){
            this.el2.Fill = new SolidColorBrush(Color.FromRgb(195,195,195));
            this.el3.Fill = new SolidColorBrush(Color.FromRgb(195, 195, 195));
            this.state = 0;
        }
        public void synchronize_ele(ele cir)
        {
            this.delay = cir.delay;
            this.is_grouped = cir.is_grouped;
            this.duration = cir.duration;
            this.set_st(cir.state);
        }
        public void origined()
        {
            this.is_grouped = false;
            this.group_id = 0;
            this.color = 0;
            this.delay = 2;
            this.set_st(0);
            this.duration = 3;
            this.border.Fill = new SolidColorBrush(Colors.Transparent);
       
        }
        public void group_signed()
        {
            this.group_sign.Fill = new SolidColorBrush(Colors.Green);
        }
        public void not_grouped()
        {
            this.group_sign.Fill = new SolidColorBrush(Colors.Gray);
        }
    }
}
