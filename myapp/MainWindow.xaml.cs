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
using System.Windows.Threading;
using System.Text.RegularExpressions;

namespace myapp
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public Boolean is_max { set; get; }
        public DispatcherTimer timer { set; get; }
        public Cursors_type cty { get; set; }
        public int cd { set; get; }
        public int countd { get; set; }
        public int start_time { get; set; }
        public int holes_num { get; set; }
        private int remains { get; set; }
        private bool allow_show_property { get; set; }
        public int holes_index { get; set; }
        protected bool isDragging;
        Brush _color1 = Brushes.YellowGreen;
        Brush _color2 = Brushes.Red;
        private ele curele_in_panel;
        private int changeability = 0;
        private Utils ut;
        private int group_state { get; set; }
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
            this.group_state = 0;
            this.remains = 0;
            this.countd = 4;
            this.holes_num = 0;
            this.holes_index = 0;
            this.allow_show_property = true;
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
            this.start_time = 4;
            this.cd = 0;
            //计时器初始化
            timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += timer_Tick;
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            this.cd++;
            this.countd--;
            if(countd>0)
            this.timer_label.Text = string.Format("{0:D2}:{1:D2}", countd / 60, countd % 60);
            else
            {
                this.timer_label.Text = "开始爆破";
            }
            int temp_cutd = cd - this.start_time;
            if (temp_cutd == 0)
            {
                foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
                {
                    if (c is ele)
                    {
                        ele circle_ = c as ele;
                        if (circle_.state > 0)
                            (c as ele).change_color(Colors.Yellow);
                    }
                }
            }

            else if (temp_cutd > 0)
            {
                foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
                {
                    if (c is ele)
                    {
                        ele circle_ = c as ele;
                        if (temp_cutd - circle_.delay == 0)
                            if (circle_.state > 0)
                                circle_.change_color(Colors.Red);
                        if (temp_cutd - circle_.delay - circle_.duration == 0)
                        {
                            //对于当起爆时间小于默认的例子，会出现不归零情况，bug
                            remains++;
                            if (circle_.state > 0)
                                circle_.change_color(Colors.Black);
                        }
                        //(c as ele).change_color(Colors.Yellow);

                    }
                }
                if (remains == this.holes_num)
                {
                    ut.Kind_type = "Play";
                    //this.test_label.Text = "chnaged";
                    this.timer.Stop();
                    this.cd = 0;
                    this.countd = this.start_time;
                    this.remains = 0;
                    this.timer_label.Text = "爆破完毕";
                }
            }

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
                    //this.test_label.Text = name;
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
            cur = cursorhelper.frombytearray(Properties.Resources.circ);
            this.Cursor = cur;
            //if (Cur_rep("Hole")) this.test_label.Text = "打孔";
            cty.set_attr_val("Hole");
        }

        private void Set_pen_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.handwriting);
            this.Cursor = cur;
            //if (Cur_rep("Draft")) this.test_label.Text = "备注";
            cty.set_attr_val("Draft");
        }

        private void Set_bomb_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.bomb);
            this.Cursor = cur;
            //if (Cur_rep("Bomb_s")) this.test_label.Text = "连续装药";
            cty.set_attr_val("Bomb_s");
        }

        private void Set_bomba_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.bomb_a);
            this.Cursor = cur;
            //if (Cur_rep("Bomb_a")) this.test_label.Text = "空气炸药";
            cty.set_attr_val("Bomb_a");
        }

        private void Set_era_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.eraser);
            this.Cursor = cur;
            //if (Cur_rep("Eraser")) this.test_label.Text = "橡皮擦";
            cty.set_attr_val("Eraser");
        }

        private void Set_crossp_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            //if (Cur_rep("Gp")) this.test_label.Text = "分组添加";
            cty.set_attr_val("Gp");
        }

        private void Set_crossm_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            //if (Cur_rep("Gx")) this.test_label.Text = "分组交叉";
            cty.set_attr_val("Gx");
        }

        private void Set_crossl_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.cross);
            this.Cursor = cur;
            //if (Cur_rep("Gm")) this.test_label.Text = "分组移除";
            cty.set_attr_val("Gm");
        }

        private void Set_mouse_cursor(object sender, RoutedEventArgs e)
        {
            cur = cursorhelper.frombytearray(Properties.Resources.normal_select_blue);
            this.Cursor = cur;
            //if (Cur_rep("Move")) this.test_label.Text = "移动";
            cty.set_attr_val("Move");

        }
        //icon更改

        private void _pause(object sender, RoutedEventArgs e)
        {
            if (ut.Kind_type.ToString().Equals("Play"))
            {
                init_count_down();
            }
            else reset_count_down();
        }
        private void init_count_down()
        {
            //ut.Kind_type = ut.Kind_type.ToString().Equals("Play") ? "Pause" : "Play";
            this.countd = start_time;
            ut.Kind_type = "Pause";
            this.timer.Start();
            this.timer_label.Text = string.Format("{0:D2}:{1:D2}", countd / 60, countd % 60);
            //this.test_label.Text = ut.Kind_type.ToString();
            //this.test_label.Text = this.start_time_box.Text;
            //Console.Write("heelo");
            //if (IsInteger(this.start_time_box.Text))
            //{
            //    this.test_label.Text = int.Parse(this.start_time_box.Text).ToString();
            //    this.start_time = int.Parse(this.start_time_box.Text);
            //    this.timer.Start();
            //}
        }
        private void reset_count_down()
        {
            ut.Kind_type = "Play";
            this.timer.Stop();

        }
        private bool IsInteger(string value)
        {
            string pattern = @"^\d+$";
            return Regex.IsMatch(value, pattern);
        }
        private void _refresh(object sender, RoutedEventArgs e)
        {
            foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
            {
                if (c is ele)
                {
                    //this.test_label.Text = "refresh";
                    ele circle_ = c as ele;
                    circle_.refresh();

                    Canvas.SetLeft(circle_, circle_.cache_point.X);
                    Canvas.SetTop(circle_, circle_.cache_point.Y);
                }
            }
        }
        private void _delete(object sender, RoutedEventArgs e)
        {
            UIElement el = null;
            for (int index = this.ShapeCanvas.Children.Count - 1; index >= 0; index--)
                if ((el = this.ShapeCanvas.Children[index]) is ele)
                    this.ShapeCanvas.Children.Remove(el);
        }
        //private void hide_property(object sender, RoutedEventArgs e)
        //{
        //    this.property_panel.Visibility = Visibility.Hidden;
        //    ut.P_index = "1";
        //    changeability = 0;
        //}

        private void ShowGridlines_OnChecked(object sender, RoutedEventArgs e)
        {
            DrawGraph((int)slidval.Value, (int)slidval.Value, ShapeCanvas);
            this.slidval.IsEnabled = true;
        }

        private void ShowGridlines_OnUnchecked(object sender, RoutedEventArgs e)
        {
            RemoveGraph(ShapeCanvas);
            this.slidval.IsEnabled = false;
        }

        private void SliderValue_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (ShowGridlines.IsChecked ?? false)
            {
                DrawGraph((int)slidval.Value, (int)slidval.Value, ShapeCanvas);
            }
        }

        private void DrawGraph(int yoffSet, int xoffSet, Canvas mainCanvas)
        {
            RemoveGraph(mainCanvas);
            Image lines = new Image();
            lines.SetValue(Panel.ZIndexProperty, -100);
            //this.test_label.Text = Panel.ZIndexProperty.ToString();
            //Draw the grid
            DrawingVisual gridLinesVisual = new DrawingVisual();
            DrawingContext dct = gridLinesVisual.RenderOpen();
            Pen lightPen = new Pen(_color1, 0.8), darkPen = new Pen(_color2, 1);
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
            if (cty.Gp || cty.Gm || cty.Gx) rect_down(sender, e);
        }
        private void create_hole(MouseButtonEventArgs e)
        {
            Point p = Mouse.GetPosition(this.ShapeCanvas);
            this.holes_num++;
            this.holes_index++;
            ele c = new ele(p, holes_index) { Width = 40, Height = 40 };
            c.MouseLeftButtonDown += new MouseButtonEventHandler(ele_copes);
            c.MouseLeftButtonUp += new MouseButtonEventHandler(ele_copes_ad);
            c.MouseMove += new MouseEventHandler(ele_move);
            c.MouseLeave += new MouseEventHandler(ele_border_hide);
            c.MouseEnter += new MouseEventHandler(ele_border_show);
            c.MouseEnter += new MouseEventHandler(show_prop);
            c.MouseRightButtonDown += new MouseButtonEventHandler(show_prop_prior);
            ShapeCanvas.Children.Add(c);
            Canvas.SetLeft(c, e.GetPosition(this.ShapeCanvas).X - 20);
            Canvas.SetTop(c, e.GetPosition(this.ShapeCanvas).Y - 20);
            Canvas.SetZIndex(c, 5);

        }



        public void RemoveChild(Canvas canvas, Point position)
        {
            Point recurp = new Point();
            recurp.X = position.X + 12;
            recurp.Y = position.Y + 12;
            var element = canvas.InputHitTest(recurp) as UIElement;
            UIElement parent;

            while (element != null &&
                (parent = VisualTreeHelper.GetParent(element) as UIElement) != canvas)
            {
                element = parent;
            }

            if (element != null && element is ele)
            {
                canvas.Children.Remove(element);
                this.holes_num--;
            }
        }

        private void eraser_current(object sender, MouseButtonEventArgs e)
        {
            var canvas = sender as Canvas;
            RemoveChild(canvas, e.GetPosition(canvas));

        }

        private void ele_copes(object sender, MouseButtonEventArgs e)
        {
            if (cty.Bomb_s)
            {
                ele c = (ele)sender;
                c.set_bam_s();
                show_prop(sender, e);

            }
            else if (cty.Bomb_a)
            {
                ele c = (ele)sender;
                c.set_bam_a();
                show_prop(sender, e);
            }
            else if (cty.Move)
            {
                isDragging = true;
                ele c = (ele)sender;
                c.CaptureMouse();
                show_prop(sender, e);
            }

        }

        private void ele_copes_ad(object sender, MouseButtonEventArgs e)
        {
            if (cty.Move && e.LeftButton == MouseButtonState.Released)
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
                    Canvas.SetLeft(c, curp.X - 20);
                    Canvas.SetTop(c, curp.Y - 20);

                    c.x_label = Convert.ToInt32(curp.X-20);
                    c.y_label = Convert.ToInt32(curp.Y-20);

                }
            }
        }
        private void ele_border_show(object sender, MouseEventArgs e)
        {
            if (this.allow_show_property)
            {
                var c = sender as ele;
                c.border.Stroke = new SolidColorBrush(Colors.GhostWhite);
            }
        }
        private void ele_border_hide(object sender, MouseEventArgs e)
        {
            if (this.allow_show_property)
            {
                var c = sender as ele;
                c.border.Stroke = new SolidColorBrush(Colors.Transparent);
            }
        }
        private void show_prop(object sender, MouseEventArgs e)
        {
            if (this.allow_show_property)
            {
                ele c = (ele)sender;
                curele_in_panel = (ele)sender;
                access_panel(c);
            }
        }
        private void show_prop_prior(object sender, MouseEventArgs e)
        {
            if (this.allow_show_property)
            {
                ele c = (ele)sender;
                curele_in_panel = (ele)sender;
                access_panel(c);
                c.border.Stroke = new SolidColorBrush(Colors.GhostWhite);
                this.allow_show_property = false;
            }
            else
            {
                ele c = (ele)sender;
                this.allow_show_property = true;
                c.border.Stroke = new SolidColorBrush(Colors.Transparent);
            }
        }

        public void access_panel(ele cir)
        {
            this.delay_time.Text = cir.delay.ToString();
            this.group_id.Text = cir.group_id.ToString();
            this.xlabel.Text = cir.x_label.ToString();
            this.ylabel.Text = cir.y_label.ToString();
            this.bomb_state.IsChecked = cir.state > 0 ? true : false;
            this.bomb_duration.Text = cir.duration.ToString();
            this.property_panel.Visibility = Visibility.Visible;
            this.index_in_panel.Text = cir.index_label.Text;
            ut.P_index = "3";
            this.curele_in_panel = cir;
        }



        private void allow_change(object sender, MouseEventArgs e)
        {
            changeability = 1;
        }

        private void dual_delay(object sender, TextChangedEventArgs e)
        {
            if (IsInteger(this.delay_time.Text.Trim()))
            {
                this.curele_in_panel.change_delay(int.Parse(this.delay_time.Text.Trim()));
                //this.test_label.Text = changeability.ToString() + " " + this.curele_in_panel.delay.ToString();

            }
        }

        private void dual_group(object sender, TextChangedEventArgs e)
        {
            if (IsInteger(this.group_id.Text.Trim()))
            {
                this.curele_in_panel.change_group_id(int.Parse(this.group_id.Text.Trim()));
                //this.test_label.Text = changeability.ToString() + " " + this.curele_in_panel.group_id.ToString();
            }

        }


        private void dual_bomb_last(object sender, TextChangedEventArgs e)
        {
            if (IsInteger(this.bomb_duration.Text.Trim()))
            {
                this.curele_in_panel.change_duration(int.Parse(this.bomb_duration.Text.Trim()));
                //this.test_label.Text = changeability.ToString() + " " + this.curele_in_panel.duration.ToString();
            }
        }

        private void dual_start_timer(object sender, TextChangedEventArgs e)
        {
               
            if (IsInteger(this.start_time_box.Text.Trim()))
            {
                //this.test_label.Text = this.start_time_box.Text.Trim();
                this.start_time = int.Parse(this.start_time_box.Text.Trim());
                this.countd = start_time;
            }
        }

        private void slide_ability(object sender, DependencyPropertyChangedEventArgs e)
        {
            this.slidval.IsEnabled = false;
        }

        private void recountdown(object sender, RoutedEventArgs e)
        {
            this.cd = 0;
            this.countd = start_time;
            this.timer_label.Text = string.Format("{0:D2}:{1:D2}", countd / 60, countd % 60);
            this.timer.Stop();
            foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
            {
                if (c is ele)
                {

                    ele circle_ = c as ele;
                    circle_.change_color(Colors.Black);
                }

            }
            reset_count_down();
        }
        //fenzu
        bool mouseDown = false; // Set to 'true' when mouse is held down.
        Point mouseDownPos; // The point where the mouse button was clicked down.
        Rectangle selection = null;
        List<ele> ele_list = new List<ele>();
        private void rect_down(object sender, MouseButtonEventArgs e)
        {
            // Capture and track the mouse.
            if (cty.Gp||cty.Gm||cty.Gx)
            {
                if (cty.Gm) selection = this.selection_m;
                if (cty.Gx) selection = this.selection_x;
                if (cty.Gp) selection = this.selection_p;
              
                mouseDown = true;
                mouseDownPos = e.GetPosition(this.ShapeCanvas);

                // Initial placement of the drag selection box.         
                Canvas.SetLeft(selection, mouseDownPos.X);
                Canvas.SetTop(selection, mouseDownPos.Y);
                Canvas.SetZIndex(selection, 5);
                selection.Width = 0;
                selection.Height = 0;

                // Make the drag selection box visible.
                selection.Visibility = Visibility.Visible;
            }
        }

        private void rect_up(object sender, MouseButtonEventArgs e)
        {
           

            if (cty.Gp || cty.Gm || cty.Gx)
            {
                this.allow_show_property = false;
                //if (cty.Gp) selection = this.selection_p;
                //if (cty.Gm) selection = this.selection_m;
                //if (cty.Gx) selection = this.selection_x;
                // Release the mouse capture and stop tracking it.
                mouseDown = false;

                // Hide the drag selection box.
                selection.Visibility = Visibility.Collapsed;

                Point mouseUpPos = e.GetPosition(this.ShapeCanvas);
                //this.test_label.Text = mouseDownPos.X + "  down  " + mouseDownPos.Y + "  " + mouseUpPos.X + "  " + mouseUpPos.Y;
                if (cty.Gp)
                {
                    List<ele> templist = new List<ele>();
                    foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
                    {
                        if (c is ele)
                        {

                            ele cir = c as ele;
                            Point center = new Point(cir.x_label, cir.y_label);
                            if (is_in_rect(mouseDownPos, mouseUpPos, center))
                            {
                                cir.border.Stroke = new SolidColorBrush(Colors.Green);
                                templist.Add(cir);
                            }

                        }
                    }
                    ele_list = ele_list.Union(templist).ToList<ele>();
                }else if (cty.Gm)
                {
                    List<ele> templist = new List<ele>();
                    foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
                    {
                        if (c is ele)
                        {

                            ele cir = c as ele;
                            Point center = new Point(cir.x_label, cir.y_label);
                            if (is_in_rect(mouseDownPos, mouseUpPos, center))
                            {
                                cir.border.Stroke = new SolidColorBrush(Colors.Transparent);
                                templist.Add(cir);

                            }

                        }
                    }
                    ele_list = ele_list.Except(templist).ToList<ele>();

                }
                else
                {
                    List<ele> templist = new List<ele>();
                    foreach (var c in LogicalTreeHelper.GetChildren(this.ShapeCanvas))
                    {
                        if (c is ele)
                        {

                            ele cir = c as ele;
                            Point center = new Point(cir.x_label, cir.y_label);
                            if (!is_in_rect(mouseDownPos, mouseUpPos, center))
                            {
                                cir.border.Stroke = new SolidColorBrush(Colors.Transparent);
                                templist.Add(cir);
                            }

                        }
                    }
                    ele_list=ele_list.Except(templist).ToList<ele>();
                }
                //this.test_label.Text = ele_list.Count.ToString();
                // TODO: 
                //
                // The mouse has been released, check to see if any of the items 
                // in the other canvas are contained within mouseDownPos and 
                // mouseUpPos, for any that are, select them!
                //
                this.group_panel.Visibility = Visibility.Visible;
                this.ele_sum.Text = ele_list.Count().ToString();

            }
        }

        private void rect_move(object sender, MouseEventArgs e)
        {
            if (cty.Gp || cty.Gm || cty.Gx)
            {
                //if (cty.Gp) selection = this.selection_p;
                //if (cty.Gm) selection = this.selection_m;
                //if (cty.Gx) selection = this.selection_x;
                if (mouseDown)
                {
                    // When the mouse is held down, reposition the drag selection box.

                    Point mousePos = e.GetPosition(this.ShapeCanvas);
                    //this.test_label.Text = mousePos.X + " " + mousePos.Y;
                    //Canvas.SetLeft(selection_p, mouseDownPos.X);
                    //Canvas.SetTop(selection_p, mouseDownPos.Y);
                    //selection_p.Width = Math.Abs(mousePos.X - mouseDownPos.X);
                    //selection_p.Height = Math.Abs(mouseDownPos.Y - mousePos.Y);
                    if (mouseDownPos.X < mousePos.X)
                    {
                        Canvas.SetLeft(selection, mouseDownPos.X);
                        selection.Width = mousePos.X - mouseDownPos.X;
                    }
                    else
                    {
                        Canvas.SetLeft(selection, mousePos.X);
                        selection.Width = mouseDownPos.X - mousePos.X;
                    }

                    if (mouseDownPos.Y < mousePos.Y)
                    {
                        Canvas.SetTop(selection, mouseDownPos.Y);
                        selection.Height = mousePos.Y - mouseDownPos.Y;
                    }
                    else
                    {
                        Canvas.SetTop(selection, mousePos.Y);
                        selection.Height = mouseDownPos.Y - mousePos.Y;
                    }
                  
                }
              
            }
        }

        private Boolean is_in_rect(Point a,Point b,Point center)
        {
            double x1 = center.X - a.X;
            double x2 = center.X - b.X;
            double y1 = center.Y - a.Y;
            double y2 = center.Y - b.Y;
            double ans = x1 * x2 + y2 * y1;
            //FileStream fs = new FileStream("E://log.txt", FileMode.Append);
            ////获得字节数组
            //byte[] data = System.Text.Encoding.Default.GetBytes(ans.ToString()+" $ ");
            ////开始写入
            //fs.Write(data, 0, data.Length);
            ////清空缓冲区、关闭流
            //fs.Flush();
            //fs.Close();
            if (ans <= 0) return true;
         
            else return false;
         

        }

        private void set_group(object sender, RoutedEventArgs e)
        {

            if (IsInteger(this.group_text_block.Text.Trim())&&IsInteger(this.delay_text_block.Text.Trim())&&IsInteger(this.bomb_text_block.Text.Trim())&&group_state!=0){
                int group_ = int.Parse(this.group_text_block.Text.Trim());
                int delay_ = int.Parse(this.delay_text_block.Text.Trim());
                int bomb_ = int.Parse(this.bomb_text_block.Text.Trim());
                Point p = new Point(0, 0);
                ele sample = new ele(p, 0);
                //sample.se
                sample.set_st(group_state - 1);
                sample.change_delay(delay_);
                sample.change_duration(bomb_);
                sample.change_group_id(group_);
                foreach(ele cir in ele_list)
                {
                    cir.synchronize_ele(sample);
                    cir.change_group_id(sample.group_id);
                }
                this.group_panel.Visibility = Visibility.Collapsed;
                this.group_state = 0;
                this.plain_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                this.bomb_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                this.rocket_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                this.ele_list.Clear();
            }
        }

        private void plain_down(object sender, RoutedEventArgs e)
        {
         
            
                this.plain_state_icon.Foreground = new SolidColorBrush(Colors.OrangeRed);
                this.bomb_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                this.rocket_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                group_state = 1;
            

        }

        private void bomb_down(object sender, RoutedEventArgs e)
        {

            this.plain_state_icon.Foreground = new SolidColorBrush(Colors.Green);
            this.bomb_state_icon.Foreground = new SolidColorBrush(Colors.OrangeRed);
            this.rocket_state_icon.Foreground = new SolidColorBrush(Colors.Green);
            group_state = 2;
            
        }

        private void rocket_down(object sender, RoutedEventArgs e)
        {
          
                this.plain_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                this.bomb_state_icon.Foreground = new SolidColorBrush(Colors.Green);
                this.rocket_state_icon.Foreground = new SolidColorBrush(Colors.OrangeRed);
                group_state = 3;
            

        }
    }
}




