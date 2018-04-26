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
using MaterialDesignThemes.Wpf;
using System.Reflection;
using myapp.items;

namespace myapp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Boolean is_max { set; get; }

        //private class cursor_types
        //{
        //    Boolean Hole { get; set; }
        //    Boolean Bomb_s { get; set; }
        //    Boolean Bomb_a{ get; set; }
        //    Boolean Draft { get; set; }
        //    Boolean Eraser { get; set; }
        //    Boolean Gp { get; set; }
        //    Boolean Gx { get; set; }
        //    Boolean Gm { get; set; }
        //    Boolean Move { get; set; }

        //}
        public Cursors_type cty { get; set; }

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
            ut = new Utils() { Kind_type = "Play", P_index = "3" };
            this.DataContext = ut;
            //鼠标标记初始化
            cty = new Cursors_type();
            //属性栏隐藏
            this.property_panel.Visibility = Visibility.Visible;
            init_canvas_tunnel();
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

        //设置鼠标类型代理
        private bool Cur_rep(String name)
        {
            foreach (PropertyInfo p in cty.GetType().GetProperties())
            {
                if (p.Name.Equals(name))
                {
                    this.test_label.Text = name;
                    p.SetValue(cty, true, null);
                    return true;
                }
                else
                    p.SetValue(cty, false, null);
            }
            return false;
        }

        private void Set_cir_cursor(object sender, RoutedEventArgs e)
        {
            //if (cursor_st == false)
            //{
            cur = cursorhelper.frombytearray(Properties.Resources.circ);
            this.Cursor = cur;
            if (Cur_rep("Hole")) this.test_label.Text = "打孔";
            cty.set_attr_val("Hole");
            //change_cur_sta();
            //}
            //    else
            //    {
            //        change_cur_sta();
            //        init_cur();
            //    }
            //}
        }

        private void Set_pen_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.handwriting);
            this.Cursor = cur;
            if (Cur_rep("Draft")) this.test_label.Text = "备注";
            cty.set_attr_val("Draft");
        }

        private void Set_bomb_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.bomb);
            this.Cursor = cur;
            if (Cur_rep("Bomb_s")) this.test_label.Text = "连续装药";
            cty.set_attr_val("Bomb_s");
        }

        private void Set_bomba_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.bomb_a);
            this.Cursor = cur;
            if (Cur_rep("Bomb_a")) this.test_label.Text = "空气炸药";
            cty.set_attr_val("Bomb_a");
        }

        private void Set_era_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.eraser);
            this.Cursor = cur;
            if (Cur_rep("Eraser")) this.test_label.Text = "橡皮擦";
            cty.set_attr_val("Eraser");
        }

        private void Set_crossp_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            if (Cur_rep("Gp")) this.test_label.Text = "分组添加";
            cty.set_attr_val("Gp");
        }

        private void Set_crossm_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            if (Cur_rep("Gx")) this.test_label.Text = "分组交叉";
            cty.set_attr_val("Gx");
        }

        private void Set_crossl_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            if (Cur_rep("Gm")) this.test_label.Text = "分组移除";
            cty.set_attr_val("Gm");
        }

        private void Set_mouse_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
            this.Cursor = cur;
            if (Cur_rep("Move")) this.test_label.Text = "移动";
            cty.set_attr_val("Move");

        }
        //icon更改
        private Utils ut;
        private void _pause(object sender, RoutedEventArgs e)
        {
            ut.Kind_type = ut.Kind_type.ToString().Equals("Play") ? "Pause" : "Play";
            this.test_label.Text = ut.Kind_type.ToString();
        }

        private void _refresh(object sender, RoutedEventArgs e)
        {

        }
        private void _delete(object sender, RoutedEventArgs e)
        {
            UIElement el = null;
            for (int index = this.ShapeCanvas.Children.Count - 1; index >= 0; index--)
                if ((el = this.ShapeCanvas.Children[index]) is ele)
                    this.ShapeCanvas.Children.Remove(el);
            //this.ShapeCanvas.Children.Clear();
            //var children= from UIElement c in ShapeCanvas.Children where c is ele select c;
            // foreach(UIElement u in children)
            // {
            //     ShapeCanvas.Children.Remove(u);
            //     //break;
            // }
            //foreach (UIElement obj in this.ShapeCanvas.Children)
            //{
            //    if (obj is ele) this.ShapeCanvas.Children.Remove(obj);
            //}
        }
        private void show_property(object sender, RoutedEventArgs e)
        {
            this.property_panel.Visibility = Visibility.Hidden;
            ut.P_index = "1";

        }

        private void ShowGridlines_OnChecked(object sender, RoutedEventArgs e)
        {
            DrawGraph((int)slidval.Value, (int)slidval.Value, ShapeCanvas);
        }

        private void ShowGridlines_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RemoveGraph(ShapeCanvas);
        }

        private void SliderValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ShowGridlines.IsChecked ?? false)
            {
                //DrawGraph((int)SliderValue.Value, (int)SliderValue.Value, ShapeCanvas);
                DrawGraph((int)slidval.Value, (int)slidval.Value, ShapeCanvas);
            }
        }

        Brush _color1 = Brushes.Black;
        Brush _color2 = Brushes.Gray;
        private void DrawGraph(int yoffSet, int xoffSet, Canvas mainCanvas)
        {
            RemoveGraph(mainCanvas);
            Image lines = new Image();
            lines.SetValue(Panel.ZIndexProperty, -100);
            this.test_label.Text = Panel.ZIndexProperty.ToString();
            //Draw the grid
            DrawingVisual gridLinesVisual = new DrawingVisual();
            DrawingContext dct = gridLinesVisual.RenderOpen();
            Pen lightPen = new Pen(_color1, 0.5), darkPen = new Pen(_color2, 1);
            lightPen.Freeze();
            darkPen.Freeze();

            int yOffset = yoffSet,
                xOffset = xoffSet,
                rows = (int)(this.main_canvas.ActualHeight),
                columns = (int)(this.main_canvas.ActualWidth),
                alternate = yOffset == 5 ? yOffset : 1,
                j = 0;

            //Draw the horizontal lines
            Point x = new Point(0, 0.5);
            Point y = new Point(this.main_canvas.ActualWidth, 0.5);

            for (int i = 0; i <= rows; i++, j++)
            {
                dct.DrawLine(j % alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(0, yOffset);
                y.Offset(0, yOffset);
            }
            j = 0;
            //Draw the vertical lines

            x = new Point(0.5, 0);

            y = new Point(0.5, this.main_canvas.ActualHeight);

            for (int i = 0; i <= columns; i++, j++)
            {
                dct.DrawLine(j % alternate == 0 ? lightPen : darkPen, x, y);
                x.Offset(xOffset, 0);
                y.Offset(xOffset, 0);
            }
            dct.Close();
            //this.test_label.Text = this.main_canvas.ActualHeight.ToString();
            RenderTargetBitmap bmp = new RenderTargetBitmap((int)this.main_canvas.ActualWidth,
                (int)this.main_canvas.ActualHeight - 50, 96, 96, PixelFormats.Pbgra32);
            bmp.Render(gridLinesVisual);
            bmp.Freeze();
            lines.Source = bmp;
            lines.Opacity = 0.5;
            mainCanvas.Children.Add(lines);
            Canvas.SetZIndex(lines, 4);


        }

        //设置涵洞模型
        private void init_canvas_tunnel()
        {

        }
        private void RemoveGraph(Canvas mainCanvas)
        {
            foreach (UIElement obj in mainCanvas.Children)
            {
                if (obj is Image)
                {
                    mainCanvas.Children.Remove(obj);
                    break;
                }
            }
        }

        private void cursor_event_handlers(object sender, MouseButtonEventArgs e)
        {
            if (cty.Hole) create_hole(e);
            if (cty.Eraser) eraser_current(sender, e);

        }

        protected bool isDragging;
        //private Point clickPosition;
        private void create_hole(MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(this.ShapeCanvas);
            ele c = new ele(p) { Width = 40, Height = 40 };
            c.MouseLeftButtonDown += new MouseButtonEventHandler(ele_copes);
            c.MouseLeftButtonUp += new MouseButtonEventHandler(ele_copes_ad);
            c.MouseMove += new MouseEventHandler(ele_move);
            c.MouseRightButtonDown += new MouseButtonEventHandler(show_prop);
            ShapeCanvas.Children.Add(c);
            Canvas.SetLeft(c, e.GetPosition(this.ShapeCanvas).X - 19);
            Canvas.SetTop(c, e.GetPosition(this.ShapeCanvas).Y - 15);
            Canvas.SetZIndex(c, 5);
        }

        public  void RemoveChild(Canvas canvas, Point position)
        {
            Point recurp = new Point();
            recurp.X = position.X +14;
            recurp.Y = position.Y + 14;
            //this.test_label.Text = position.X + " " + position.Y;
            var element = canvas.InputHitTest(recurp) as UIElement;
            UIElement parent;

            while (element != null &&
                (parent = VisualTreeHelper.GetParent(element) as UIElement) != canvas)
            {
                element = parent;
            }

            if (element !=null&&element is ele)
            {
                canvas.Children.Remove(element);
            }
        }

        private void eraser_current(object sender, MouseButtonEventArgs e)
        {
           
                var canvas = sender as Canvas;
                RemoveChild(canvas, e.GetPosition(canvas));

            //ele c = sender as ele;
            //this.test_label.Text = c.GetType().ToString();
            //if (c != null)
            //{
            //    this.ShapeCanvas.Children.Remove(c);
            //}
        }

        private void ele_copes(object sender, MouseButtonEventArgs e)
        {
            if (cty.Bomb_s)
            {
                ele c = (ele)sender;
                c.el2.Fill = new SolidColorBrush(Color.FromRgb(95, 95, 95));
                c.el3.Fill = new SolidColorBrush(Color.FromRgb(95, 95, 95));

            }
            else if (cty.Bomb_a)
            {
                ele c = (ele)sender;
                c.el2.Fill = new SolidColorBrush(Color.FromRgb(95, 95, 95));
                c.el3.Fill = new SolidColorBrush(Color.FromRgb(172, 172, 172));
            }
            else if (cty.Move)
            {
                isDragging = true;
                ele c = (ele)sender;
                //clickPosition = e.GetPosition(this.Parent as UIElement);
                c.CaptureMouse();
            }

        }

        private void ele_copes_ad(object sender, MouseButtonEventArgs e)
        {
            if (cty.Move)
            {
                isDragging = false;
                var c = sender as ele;
                c.ReleaseMouseCapture();
            }
        }


        private void ele_move(object sender, MouseEventArgs e)
        {
            if (cty.Move)
            {
                var c = sender as ele;
                if (isDragging && c != null)
                {
                    Point curp = e.GetPosition(this.ShapeCanvas);
                    //this.test_label.Text = curp.X.ToString() + " " + curp.Y.ToString();

                    Canvas.SetLeft(c, curp.X - 19);
                    Canvas.SetTop(c, curp.Y - 15);
                    //var trf = c.RenderTransform as TranslateTransform;
                    //var trf = new TranslateTransform();
                    //if (trf == null)
                    //{
                    //    trf = new TranslateTransform();
                    //    c.RenderTransform = trf;
                    //}
                    //Try
                    //trf.X = curp.X - clickPosition.X;
                    //trf.Y = curp.Y - clickPosition.Y;
                    //c.RenderTransform = trf;
                    //clickPosition.X = curp.X;
                    //clickPosition.Y = curp.Y;

                }
            }
        }

        private void show_prop(object sender, MouseEventArgs e)
        {

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
