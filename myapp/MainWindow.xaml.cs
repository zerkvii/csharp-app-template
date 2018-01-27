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
using System.IO;
namespace myapp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Boolean is_max { set; get; }

        //public Boolean cursor_st { set; get; }
        public sealed class cursorhelper
        {
            private cursorhelper() { }
            public static Cursor frombytearray(byte[] array)
            {
                using (MemoryStream memory = new MemoryStream(array))
                {
                    return new Cursor(memory);
                }
            }

        }
        public Cursor cur { set; get; }
     
        public MainWindow()

        {
            cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
            this.Cursor = cur;
            InitializeComponent();
            is_max = false;
            //cursor_st = false;
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

        private void Set_cir_cursor(object sender, MouseEventArgs e)
        {
            //if (cursor_st == false)
            //{
            cur = cursorhelper.frombytearray(Properties.Resources.circ);
            this.Cursor = cur;
            //change_cur_sta();
            //}
            //    else
            //    {
            //        change_cur_sta();
            //        init_cur();
            //    }
            //}
        }

        private void Set_pen_cursor(object sender, MouseEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.handwriting);
            this.Cursor = cur;
        }

        private void Set_bomb_cursor(object sender, MouseEventArgs e)
        {

            cur = cursorhelper.frombytearray(Properties.Resources.bomb);
            this.Cursor = cur;

        }

        private void Set_era_cursor(object sender, MouseEventArgs e)
        {

            cur = cursorhelper.frombytearray(Properties.Resources.eraser);
            this.Cursor = cur;
        }

        private void Set_cross_cursor(object sender, MouseEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
        }

        private void Set_mouse_cursor(object sender, MouseEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
            this.Cursor = cur;
        }
        //public void init_cur()
        //{
        //    cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
        //    this.Cursor = cur;
        //}
        //public void change_cur_sta()
        //{
        //    cursor_st=cursor_st?false:true;
        //}
    }
}
