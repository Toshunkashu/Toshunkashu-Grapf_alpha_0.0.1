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
using OxyPlot;
using OxyPlot.Series;
using System.IO;
using static System.Math;
using System.Threading;

namespace Grapf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>

    public class x
    {
        public int power = 0;
        public double coef = 0;
        public x() { }
        public x(double koef, int pow) { coef = koef; power = pow; }
        ~x() { }
        public static x operator +(x left, x right)
        {
            if (left.power == right.power)
            {
                x sum = new x(left.coef + right.coef, left.power);
                return sum;
            }

            else return left;
        }
        public static x operator *(x left, x right)
        {
            x dob = new x(left.coef * right.coef, left.power + right.power);
            return dob;
        }
        public static x operator /(x left, x right)
        {
            x div = new x(left.coef / right.coef, left.power - right.power);
            return div;
        }
    };


    public partial class MainWindow : Window
    {

        double _BUFER_TOP;
        double _BUFER_LEFT;
        double _BUFER_HEIGHT;
        double _BUFER_WIDTH;
        bool state = false;
        bool switch_close = false;
        bool point_mode_switch = false;

        double tskH = 0.05;
        double tskW = 1;
        double bskH = 0.5;
        double bskW = 1;
        double lskH = 0.9;
        double lskW = 0.2;
        double rskH = 0.9;
        double rskW = 0.25;
        double wskH = 0.9;
        double wskW = 1;

        int N;
        int count_series = 0;
        public x[] generate_formul;
        LineSeries work_series = new LineSeries();
        List<x[]> list_function = new List<x[]>();
        List<Label> list_index = new List<Label>();
        List<TextBox> list_x = new List<TextBox>();
        List<TextBox> list_y = new List<TextBox>();
        List<PlotModel> plot_list = new List<PlotModel>();

        PlotModel Plot_Area = new PlotModel { Title = "Grapf area" };

        public MainWindow()
        {
            InitializeComponent();
            if (!Directory.Exists(@"C:/Users/vanya/AppData/Local/Toshunkashu")) Directory.CreateDirectory(@"C:/Users/vanya/AppData/Local/Toshunkashu");
            if (!Directory.Exists(@"C:/Users/vanya/AppData/Local/Toshunkashu/Grapf")) Directory.CreateDirectory(@"C:/Users/vanya/AppData/Local/Toshunkashu/Grapf");
            StreamWriter write = new StreamWriter(@"C:/Users/vanya/AppData/Local/Toshunkashu/Grapf/function.cs", false, System.Text.Encoding.UTF8);
            write.Close();


            this.Top = (System.Windows.SystemParameters.PrimaryScreenHeight / 2) - (((System.Windows.SystemParameters.PrimaryScreenHeight - 28) * 0.8) / 2);
            this.Left = (System.Windows.SystemParameters.PrimaryScreenWidth / 2) - (((System.Windows.SystemParameters.PrimaryScreenWidth + 12) * 0.7) / 2);
            this.Height = (System.Windows.SystemParameters.PrimaryScreenHeight - 28) * 0.8;
            this.Width = (System.Windows.SystemParameters.PrimaryScreenWidth + 12) * 0.7;
            this.MinHeight = this.Height;
            this.MinWidth = this.Width;
            left_space.Width = 50;

            plot_list.Add(new PlotModel { Title = "Model 0" });

            remove_point.BorderThickness = new Thickness(0);
            add_point.BorderThickness = remove_point.BorderThickness;
            sepr.Background = new SolidColorBrush(Color.FromArgb(255, 0, 0, 0));

            ContentPanel.Height = this.Height - toptool.Height - 12;
            ContentPanel.Width = this.Width - 12;
            toptool.Width = this.Width;
            _BUFER_TOP = this.Top;
            _BUFER_LEFT = this.Left;
            _BUFER_HEIGHT = this.Height;
            _BUFER_WIDTH = this.Width;


            close.BorderThickness = new Thickness(0);
            resize.BorderThickness = new Thickness(0);
            minim.BorderThickness = new Thickness(0);
            toptool.Background = new SolidColorBrush(Color.FromArgb(255, 186, 181, 203));
            close.Height = toptool.Height;
            close.Width = toptool.Height;
            resize.Height = toptool.Height;
            resize.Width = toptool.Height;
            minim.Height = toptool.Height;
            minim.Height = toptool.Height;

            series_list_warp.Orientation = Orientation.Horizontal;

            grapf.Background = this.Background;
            grapf.BorderThickness = new Thickness(2);
            grapf.BorderBrush = new SolidColorBrush(Color.FromArgb(255, 15, 15, 15));
            grapf.Height = ContentPanel.Height;
            grapf.Width = ContentPanel.Width;
            //LineSeries oXoY = new LineSeries();
            //oXoY.Points.Add(new DataPoint());
            //Plot_Area.Series.Add();
            //Plot_Area.Series.Add();
            //grapf.Model = Plot_Area;
            StreamReader read = new StreamReader(@"C:/Users/vanya/AppData/Local/Toshunkashu/Grapf/function.cs");
            if ((read.ReadToEnd()).Length == 0)
            {
                //top_space.Background = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            else
            {

            }
        }




        private void toptool_MouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void close_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void resize_Click(object sender, RoutedEventArgs e)
        {
            if (state)
            {
                this.Top = _BUFER_TOP;
                this.Left = _BUFER_LEFT;
                this.Height = _BUFER_HEIGHT;
                this.Width = _BUFER_WIDTH;
                state = false;
                ((Button)sender).Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/max.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = ((Button)sender).Height * 0.8, Width = ((Button)sender).Width * 0.8 };
            }
            else
            {
                this.Top = -6;
                this.Left = -6;
                this.Height = System.Windows.SystemParameters.PrimaryScreenHeight - 28 + 1;
                this.Width = System.Windows.SystemParameters.PrimaryScreenWidth + 12 + 1;
                state = true;
                ((Button)sender).Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/min.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = ((Button)sender).Height * 0.8, Width = ((Button)sender).Width * 0.8 };
            }


        }

        private void minim_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == System.Windows.WindowState.Minimized)
                this.WindowState = System.Windows.WindowState.Normal;
            else this.WindowState = System.Windows.WindowState.Minimized;
        }

        private void close_MouseEnter(object sender, MouseEventArgs e)
        {
            // close.Background = new SolidColorBrush(Color.FromArgb(255, 255, 99, 71));
            switch_close = false;
            close.Background = new SolidColorBrush(Color.FromArgb(255, 255, 99, 71));
        }
        private void close_MouseLeave(object sender, MouseEventArgs e)
        {
            if (switch_close) ((Button)sender).Background = toptool.Background;
            switch_close = true;
        }

        private void active_window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            ContentPanel.Height = this.Height - toptool.Height - 12;
            ContentPanel.Width = this.Width - 12;
            toptool.Width = this.Width;
            _BUFER_TOP = this.Top;
            _BUFER_LEFT = this.Left;
            _BUFER_HEIGHT = this.Height;
            _BUFER_WIDTH = this.Width;

            top_space.Width = ContentPanel.Width * tskW;
            top_space.Height = ContentPanel.Height * tskH;
            //left_space.Width = ContentPanel.Width * lskW;
            left_space.Height = ContentPanel.Height * lskH;
            point_mode.Width = ContentPanel.Width * lskW;
            point_mode.Height = ContentPanel.Height * lskH;
            point_mode_border_in.Height = point_mode.Height;
            point_mode_border_in.Width = point_mode.Width - 20;
            right_space.Width = ContentPanel.Width * rskW;
            right_space.Height = ContentPanel.Height * rskH;
            bottom_space.Width = ContentPanel.Width * bskW;
            bottom_space.Height = ContentPanel.Height * bskH;
            work_space.Width = ContentPanel.Width * wskW - 100;
            work_space.Height = ContentPanel.Height * wskH;
            grapf.Height = work_space.Height;
            grapf.Width = work_space.Width;
            point_mode_wrap.Width = point_mode.Width;
            point_mode_wrap.Height = point_mode.Height;

            white_panel_control.Margin = new Thickness(point_mode_border_in.Width / 20, 5, 0, 0);
            remove_point.Margin = new Thickness(0, 1, 0, 2);
            add_point.Margin = new Thickness(7, 1, 0, 2);
            top_space.Margin = new Thickness(0, 0, 0, 0);
            left_space.Margin = new Thickness(-ContentPanel.Width, top_space.Height, 0, 0);
            if (!point_mode_switch) point_mode.Margin = new Thickness(-ContentPanel.Width - point_mode.Width, top_space.Height, 0, 0);
            else point_mode.Margin = new Thickness(-ContentPanel.Width, top_space.Height, 0, 0);
            work_space.Margin = new Thickness(-ContentPanel.Width + left_space.Width, top_space.Height, 0, 0);
            right_space.Margin = new Thickness(-ContentPanel.Width - right_space.Width + 50 + left_space.Width + work_space.Width, top_space.Height, 0, 0);
            bottom_space.Margin = new Thickness(-ContentPanel.Width, top_space.Height + left_space.Height, 0, 0);
            point_mode_wrap.Margin = new Thickness(-point_mode_border_in.Width, 0, 0, 0);

            white_panel_control.Width = point_mode_border_in.Width * 0.9;
            label_XY.Width = white_panel_control.Width - add_point.Width - remove_point.Width - 14;
            sepr.Width = point_mode_border_in.Width * 0.9;
            sepr.Margin = new Thickness(point_mode_border_in.Width / 20, 5, 0, 0);
            mode_series_name.Width = point_mode_border_in.Width - 120;

            scrol_list_point.Width = point_mode.Width;
            scrol_list_point.Height = left_space.Height * 0.8;
            list_point.Width = scrol_list_point.Width;
            scrol_list_point.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;

            left_space_resize();

            panel_mode.Height = top_space.Height;
            panel_mode.Width = top_space.Width;

            save_border.Width = point_mode_border_in.Width - point_mode_border_in.CornerRadius.BottomRight;
            save_border.Margin = new Thickness(point_mode_border_in.CornerRadius.BottomRight / 2, 0, 0, 0);
            save.Width = save_border.Width - 10;

            series_warp_border.Width = right_space.Width;
            series_wrap.Width = right_space.Width;
            series_warp_border.Height = right_space.Height * 0.485;
            series_wrap.Height = series_warp_border.Height - series_warp_border.CornerRadius.TopLeft;
            series_warp_border.Margin = new Thickness(0, right_space.Height / 100, 0, 0);

            series_list_warp_scroll.Width = right_space.Width;
            series_list_warp_scroll.Height = series_warp_border.Height - series_warp_border.CornerRadius.TopLeft * 2;

            property_warp_border.Width = right_space.Width;
            property_wrap.Width = right_space.Width;
            property_warp_border.Height = right_space.Height * 0.485;
            property_wrap.Height = series_warp_border.Height - series_warp_border.CornerRadius.TopLeft;
            property_warp_border.Margin = new Thickness(0, right_space.Height / 100, 0, 0);

        }
        int pad = 5;

        string free_Uid = null;
        private void add_point_Click(object sender, RoutedEventArgs e)
        {

            list_index.Add(new Label());
            list_x.Add(new TextBox());
            list_y.Add(new TextBox());
            Border border = new Border();




            list_index[list_index.Count - 1].Height = 24;
            list_index[list_index.Count - 1].Width = 24;
            list_x[list_x.Count - 1].Height = 30;
            list_x[list_x.Count - 1].Width = (this.point_mode_border_in.Width - point_mode_border_in.Margin.Left - list_index[list_index.Count - 1].Width - 20) * 0.4 - 14;
            list_y[list_y.Count - 1].Height = 30;
            list_y[list_y.Count - 1].Width = (this.point_mode_border_in.Width - point_mode_border_in.Margin.Left - list_index[list_index.Count - 1].Width - 20) * 0.4 - 14;
            list_index[list_index.Count - 1].Content = list_index.Count.ToString();


            list_index[list_index.Count - 1].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            list_index[list_index.Count - 1].VerticalAlignment = System.Windows.VerticalAlignment.Center;
            list_index[list_index.Count - 1].HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            list_index[list_index.Count - 1].VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            list_index[list_index.Count - 1].Background = new SolidColorBrush(Color.FromArgb(0, 180, 180, 180));
            list_index[list_index.Count - 1].BorderBrush = list_point.Background;
            list_x[list_x.Count - 1].BorderBrush = list_point.Background;
            list_y[list_y.Count - 1].BorderBrush = list_point.Background;
            list_x[list_x.Count - 1].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            list_x[list_x.Count - 1].VerticalAlignment = System.Windows.VerticalAlignment.Center;
            list_x[list_x.Count - 1].HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            list_x[list_x.Count - 1].VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            list_y[list_y.Count - 1].HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            list_y[list_y.Count - 1].VerticalAlignment = System.Windows.VerticalAlignment.Center;
            list_y[list_y.Count - 1].HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
            list_y[list_y.Count - 1].VerticalContentAlignment = System.Windows.VerticalAlignment.Center;
            list_x[list_x.Count - 1].IsKeyboardFocusedChanged += new DependencyPropertyChangedEventHandler(text_box_foxused);
            list_y[list_y.Count - 1].IsKeyboardFocusedChanged += new DependencyPropertyChangedEventHandler(text_box_foxused);
            list_x[list_x.Count - 1].LostFocus += new RoutedEventHandler(TextBox_LostFocus);
            list_y[list_y.Count - 1].LostFocus += new RoutedEventHandler(TextBox_LostFocus);
            list_x[list_x.Count - 1].Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            list_y[list_y.Count - 1].Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
            list_x[list_x.Count - 1].Uid = (list_x.Count - 1).ToString() + "x";
            list_y[list_y.Count - 1].Uid = (list_y.Count - 1).ToString() + "y";
            list_index[list_index.Count - 1].Margin = new Thickness(0, 0, 0, 0);
            list_x[list_x.Count - 1].Margin = new Thickness(0, 0, 0, 0);
            list_y[list_y.Count - 1].Margin = new Thickness(0, 0, 0, 0);


            border.Child = list_index[list_index.Count - 1];
            border.Width = list_index[list_index.Count - 1].Width;
            border.Height = list_index[list_index.Count - 1].Height;
            border.CornerRadius = new CornerRadius(15);
            border.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            border.Margin = new Thickness(white_panel_control.Margin.Left * 1.5 - 2, pad, 0, 0);

            list_point.Children.Add(border);
            border = null;
            Border borderX = new Border();

            borderX.Child = list_x[list_x.Count - 1];
            borderX.Width = list_x[list_x.Count - 1].Width + 16;
            borderX.Height = list_x[list_x.Count - 1].Height;
            borderX.CornerRadius = new CornerRadius(15);
            borderX.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            borderX.Margin = new Thickness(white_panel_control.Margin.Left, pad, 0, 0);

            list_point.Children.Add(borderX);
            borderX = null;

            Border borderY = new Border();

            borderY.Child = list_y[list_y.Count - 1];
            borderY.Width = list_y[list_y.Count - 1].Width + 16;
            borderY.Height = list_y[list_y.Count - 1].Height;
            borderY.CornerRadius = new CornerRadius(15);
            borderY.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            borderY.Margin = new Thickness(white_panel_control.Margin.Left, pad, 0, 0);

            list_point.Children.Add(borderY);
            borderY = null;

            pad = 15;

        }

        private void MainWindow_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void button_const_Click(object sender, RoutedEventArgs e)
        {
            if (generation_errors()) return;

            N = list_index.Count();
            double[][] XY = new double[2][];
            XY[0] = new double[N];
            XY[1] = new double[N];

            for (int i = 0; i < N; i++)
            {
                XY[0][i] = Convert.ToDouble(list_x[i].Text);
                XY[1][i] = Convert.ToDouble(list_y[i].Text);
            }
            generate_formul = La_Granj_funcion(XY);
            string concon = "";
            for (int i = N - 1; i >= 0; i--)
            {
                if (generate_formul == null) break;
                if (i != N - 1 && generate_formul[i].coef >= 0) { concon += "+"; }
                concon += generate_formul[i].coef.ToString() + "*x^" + generate_formul[i].power.ToString() + " ";
            }
            double max_x = Convert.ToDouble(list_x[0].Text), min_x = max_x, max_y = Convert.ToDouble(list_y[0].Text), min_y = max_y;
            for (int i = 0; i < list_index.Count; i++)
            {
                if (Convert.ToDouble(list_x[i].Text) > max_x) max_x = Convert.ToDouble(list_x[i].Text);
                if (Convert.ToDouble(list_x[i].Text) < min_x) min_x = Convert.ToDouble(list_x[i].Text);
                if (Convert.ToDouble(list_y[i].Text) > max_y) max_y = Convert.ToDouble(list_y[i].Text);
                if (Convert.ToDouble(list_y[i].Text) < min_y) min_y = Convert.ToDouble(list_y[i].Text);
            }
            work_series = PrivateSeries(generate_formul, XY[0]);
            Plot_Area = null;
            Plot_Area = new PlotModel();
            if (mode_series_name.Text == null) Plot_Area.Title = "Empty...";
            else Plot_Area.Title = mode_series_name.Text;
            Plot_Area.Series.Add(addXY("X", min_x, max_x));
            Plot_Area.Series.Add(addXY("Y", min_y, max_y));
            Plot_Area.Series.Add(work_series);
            grapf.Model = null;
            grapf.Model = Plot_Area;
            grapf.ActualModel.Axes[0].Reset();
            grapf.ActualModel.Axes[1].Reset();
            grapf.ActualModel.Axes[0].Minimum = min_x - (max_x - min_x) / 2;
            grapf.ActualModel.Axes[0].Maximum = max_x + (max_x - min_x) / 2;
            grapf.ActualModel.Axes[1].Minimum = min_y - (max_y - min_y) / 2;
            grapf.ActualModel.Axes[1].Maximum = max_y + (max_y - min_y) / 2;
            grapf.InvalidatePlot(true);
        }

        private bool generation_errors()
        {
            bool invalid = false;
            if (list_index.Count == 0) return invalid = true;
            for (int i = 0; i < list_index.Count; i++)
            {
                try
                {
                    Convert.ToDouble(list_x[i].Text);
                }
                catch
                {
                    list_x[i].Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                    ((Border)list_point.Children[i * 3 + 1]).Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                    invalid = true;
                }

                try
                {
                    Convert.ToDouble(list_y[i].Text);
                }
                catch
                {
                    list_y[i].Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                    ((Border)list_point.Children[i * 3 + 2]).Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                    invalid = true;
                }

                for (int j = i + 1; j < list_index.Count; j++)
                {

                    if (list_x[i].Text == list_x[j].Text)
                    {
                        list_x[i].Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                        list_x[j].Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                        ((Border)list_point.Children[i * 3 + 1]).Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                        ((Border)list_point.Children[j * 3 + 1]).Background = new SolidColorBrush(Color.FromArgb(255, 255, 20, 20));
                        invalid = true;
                    }

                }


            }
            return invalid;
        }

        private void left_space_resize()
        {



            if (list_index.Count > 0)
            {
                for (int i = 0; i < list_point.Children.Count; i += 3)
                {
                    ((Border)list_point.Children[i]).Margin = new Thickness(((Border)list_point.Children[i]).Width, 5, 0, 0);
                    ((Border)list_point.Children[i + 1]).Width = (this.point_mode_border_in.Width - point_mode_border_in.Margin.Left - ((Border)list_point.Children[i]).Width - 20) * 0.4 + 2;
                    ((Border)list_point.Children[i + 2]).Width = (this.point_mode_border_in.Width - point_mode_border_in.Margin.Left - ((Border)list_point.Children[i]).Width - 20) * 0.4 + 2;
                }

                for (int i = 0; i < list_index.Count; i++)
                {
                    list_index[i].Height = 30;
                    list_index[i].Width = 50;
                    list_x[i].Height = 30;
                    list_x[i].Width = (this.point_mode_border_in.Width - point_mode_border_in.Margin.Left - ((Border)list_point.Children[i * 3]).Width - 20) * 0.4 - 14;
                    list_y[i].Height = 30;
                    list_y[i].Width = (this.point_mode_border_in.Width - point_mode_border_in.Margin.Left - ((Border)list_point.Children[i * 3]).Width - 20) * 0.4 - 14;
                }
            }
        }
        private void text_box_foxused(object sender, DependencyPropertyChangedEventArgs e)
        {
            ((TextBox)sender).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            if (((TextBox)sender).Uid[((TextBox)sender).Uid.Length - 1] == 'x')
            {
                ((Border)list_point.Children[atoi(((TextBox)sender).Uid) * 3 + 1]).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            else
            {
                ((Border)list_point.Children[atoi(((TextBox)sender).Uid) * 3 + 2]).Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
            }
            list_index[atoi(((TextBox)sender).Uid)].BorderBrush = new SolidColorBrush(Color.FromArgb(255, 110, 110, 255));
            list_x[atoi(((TextBox)sender).Uid)].BorderBrush = new SolidColorBrush(Color.FromArgb(255, 110, 110, 255));
            list_y[atoi(((TextBox)sender).Uid)].BorderBrush = new SolidColorBrush(Color.FromArgb(255, 110, 110, 255));
            free_Uid = ((TextBox)sender).Uid;
        }
        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (((atoi(((TextBox)sender).Uid) != atoi(free_Uid))) || ((atoi(((TextBox)sender).Uid) == atoi(free_Uid))) && (((TextBox)sender).Uid == free_Uid))
            {
                list_index[atoi(((TextBox)sender).Uid)].BorderBrush = list_point.Background;
                list_x[atoi(((TextBox)sender).Uid)].BorderBrush = list_point.Background;
                list_y[atoi(((TextBox)sender).Uid)].BorderBrush = list_point.Background;
            }
        }

        private void remove_point_Click(object sender, RoutedEventArgs e)
        {
            if (list_index.Count > 0)
            {
                /*
                if (atoi(free_Uid) >= 0)
                {

                    list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);
                    list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);
                    list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);

                    list_index.RemoveAt(atoi(free_Uid));
                    list_x.RemoveAt(atoi(free_Uid));
                    list_y.RemoveAt(atoi(free_Uid));

                    if (list_index.Count != 0)
                    {
                        for (int i = 0; i < list_index.Count; i++)
                        {
                            list_index[i].Content = i + 1;
                            list_x[i].Uid = i.ToString() + "x";
                            list_y[i].Uid = i.ToString() + "y";
                        }

                        free_Uid = list_x[list_x.Count - 1].Uid;
                    }

                }
                else if (free_Uid == null)
                {
                    list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);
                    list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);
                    list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);

                    list_index.RemoveAt(list_index.Count - 1);
                    list_x.RemoveAt(list_x.Count - 1);
                    list_y.RemoveAt(list_y.Count - 1);
                }
                */
                list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);
                list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);
                list_point.Children.Remove(list_point.Children[list_point.Children.Count - 1]);

                list_index.RemoveAt(list_index.Count - 1);
                list_x.RemoveAt(list_x.Count - 1);
                list_y.RemoveAt(list_y.Count - 1);
            }
        }
        public int atoi(string str)
        {
            int bufer;
            string result = null;
            if (str != null)
            {
                for (int i = 0; i < str.Length; i++)
                {
                    if (int.TryParse(str[i].ToString(), out bufer)) result += str[i].ToString();
                }
                return Convert.ToInt32(result);
            }
            else return -1;
        }

        public Double Eval(string exp)
        {
            System.Data.DataTable table = new System.Data.DataTable();
            return Convert.ToDouble(table.Compute(exp, String.Empty));
        }

        public x[] La_Granj_funcion(double[][] tabel)
        {
            double[][] XY = new double[2][];
            XY[0] = new double[N];
            XY[1] = new double[N];
            XY = tabel;

            if (N <= 1)
            {
                //MessageBox.Show("To build a function there must be at least two points.", "Eror", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
            //operator
            x[][] dodanki = new x[N][];
            for (int i = 0; i < N; i++) dodanki[i] = new x[Convert.ToInt32(Math.Pow(2, N - 1))];
            double[] znam = new double[N];
            for (int i = 0; i < N; i++)
            {
                znam[i] = 1;
            }
            x mnoj, x_kv;

            for (int i = 0; i < N; i++)//доданки (дроби)
            {
                for (int j = 0, n = 0, m = 2; j < N - 1; j++)//вираховування дробу без у(n)
                {

                    if (j + n != i)
                    {

                        mnoj = new x(-1 * (XY[0][j + n]), 0);
                        //chis
                        if (j == 0)
                        {
                            dodanki[i][0] = new x(1, 1); dodanki[i][1] = new x(-1 * (XY[0][j + n]), 0);
                        }
                        else
                        {
                            for (int z = m; z < m * 2; z++)
                            {
                                dodanki[i][z] = dodanki[i][z - m] * mnoj;
                            }
                            for (int z = 0; z < m; z++)
                            {
                                x_kv = new x(1, 1);
                                dodanki[i][z] = dodanki[i][z] * x_kv;
                            }
                            m *= 2;
                        }
                        //znam
                        znam[i] *= (XY[0][i] - XY[0][j + n]);
                    }
                    else { n++; j--; }
                }

                for (int j = 0; j < Math.Pow(2, N - 1); j++)
                {
                    dodanki[i][j].coef = (dodanki[i][j].coef * XY[1][i]) / znam[i];
                }

            }

            x[] formula = new x[N];
            for (int i = 0; i < N; i++) formula[i] = new x(0, i);
            for (int s = 0; s < N; s++)
            {
                for (int i = 0; i < N; i++)
                {
                    for (int j = 0; j < Math.Pow(2, N - 1); j++)
                    {
                        formula[s] = formula[s] + dodanki[i][j];
                    }
                }
                //formula[s].coef = rounding(formula[s].coef, 5);
            }



            return formula;
        }

        public double substitution(double x)
        {
            double y = 0;

            for (int i = 0; i < N; i++)
            {
                y += Pow(x, generate_formul[i].power) * generate_formul[i].coef;
            }
            y = Math.Round(y, 7, MidpointRounding.AwayFromZero);
            return y;
        }

        public LineSeries PrivateSeries(x[] formul, double x_start, double x_finish, double dx)
        {
            LineSeries newSeries = new LineSeries();
            for (double x = x_start; x <= x_finish + dx; x += dx)
            {
                newSeries.Points.Add(new DataPoint(Math.Round(x, 7, MidpointRounding.AwayFromZero), substitution(x)));
            }
            newSeries.Title = "Series " + count_series;
            count_series++;
            return newSeries;
        }
        public LineSeries PrivateSeries(x[] formul, double[] x_array)
        {
            LineSeries newSeries = new LineSeries();
            for (int i = 0; i < x_array.Length - 1; i++)
            {

                if (x_array[i] < x_array[i + 1])
                {
                    for (double j = x_array[i]; j < x_array[i + 1]; j += 0.01)
                    {
                        newSeries.Points.Add(new DataPoint(j, substitution(j)));
                        if ((j + 0.01) > x_array[i + 1] && i == x_array.Length - 2) newSeries.Points.Add(new DataPoint(x_array[i + 1], substitution(j + 0.01)));
                    }
                }
                else if (x_array[i] > x_array[i + 1])
                {
                    for (double j = x_array[i]; j > x_array[i + 1]; j -= 0.01)
                    {
                        newSeries.Points.Add(new DataPoint(j, substitution(j)));
                        if ((j - 0.01) < x_array[i + 1] && i == x_array.Length - 2) newSeries.Points.Add(new DataPoint(x_array[i + 1], substitution(j + 0.01)));
                    }
                }

            }
            newSeries.Title = "Series " + count_series;
            count_series++;
            return newSeries;


        }

        private void open_wrap_Click(object sender, RoutedEventArgs e)
        {
            point_mode.Margin = new Thickness(-ContentPanel.Width, top_space.Height, 0, 0);
            point_mode_switch = true;
        }

        private void hide_point_mode_Click(object sender, RoutedEventArgs e)
        {
            point_mode.Margin = new Thickness(-ContentPanel.Width - point_mode.Width, top_space.Height, 0, 0);
            point_mode_switch = false;
        }

        private LineSeries addXY(string XorY, double min, double max)
        {
            if (XorY == "X" || XorY == "x")
            {
                LineSeries os = new LineSeries();
                os.Title = "X";
                os.Color = OxyColor.FromArgb(255, 0, 0, 0);
                os.Points.Add(new DataPoint(int.MinValue - min, 0));
                os.Points.Add(new DataPoint(int.MaxValue + max, 0));
                return os;
            }
            else if (XorY == "Y" || XorY == "y")
            {
                LineSeries os = new LineSeries();
                os.Title = "Y";
                os.Color = OxyColor.FromArgb(255, 0, 0, 0);
                os.Points.Add(new DataPoint(0, int.MinValue - min));
                os.Points.Add(new DataPoint(0, int.MaxValue + max));
                return os;
            }
            else return null;

        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (work_series.Points.Count >= 2)
            {
                int pad = 15;
                if (plot_list[0].Series.Count == 0)
                {
                    plot_list[0].Series.Add(addXY("X", long.MinValue, long.MaxValue));
                    plot_list[0].Series.Add(addXY("Y", long.MinValue, long.MaxValue));
                    pad = 5;
                }
                LineSeries newSeries = new LineSeries();
                newSeries.ItemsSource = work_series.Points;
                if (mode_series_name.Text == "") newSeries.Title = "Series " + series_list_warp.Children.Count;
                else newSeries.Title = mode_series_name.Text;
                plot_list[0].Series.Add(newSeries);
                list_function.Add(generate_formul);
                newSeries = null;

                Border border = new Border();
                WrapPanel wrapPanel = new WrapPanel();
                Label label = new Label();
                Button edit = new Button();
                Button delete = new Button();
                Button hide = new Button();

                border.Width = (series_list_warp_scroll.Width - 20) * 0.9;
                border.Height = 30;
                border.Background = new SolidColorBrush(Color.FromArgb(255, 255, 255, 255));
                border.BorderThickness = new Thickness(0);
                border.CornerRadius = new CornerRadius(5);
                border.Margin = new Thickness(series_list_warp_scroll.Width / 20, pad, 0, 0);
                border.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                border.VerticalAlignment = System.Windows.VerticalAlignment.Top;

                wrapPanel.Width = border.Width - 10;
                wrapPanel.Height = border.Height;
                wrapPanel.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                wrapPanel.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                wrapPanel.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                wrapPanel.MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.series_MouseLeftButtonUp);

                label.Height = wrapPanel.Height;
                label.Width = wrapPanel.Width * 0.7;
                label.Padding = new Thickness(0);
                label.Content = plot_list[0].Series[plot_list[0].Series.Count - 1].Title;
                label.Background = wrapPanel.Background;
                label.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                label.VerticalAlignment = System.Windows.VerticalAlignment.Center;
                label.HorizontalContentAlignment = System.Windows.HorizontalAlignment.Center;
                label.VerticalContentAlignment = System.Windows.VerticalAlignment.Center;

                edit.Width = wrapPanel.Width / 10;
                edit.Height = wrapPanel.Height;
                edit.BorderThickness = new Thickness(0);
                edit.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                edit.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/edit.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = edit.Height * 0.8, Width = edit.Width * 0.8 };
                edit.Click += new System.Windows.RoutedEventHandler(edit_Click);

                delete.Height = wrapPanel.Height;
                delete.Width = wrapPanel.Width / 10;
                delete.BorderThickness = new Thickness(0);
                delete.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                delete.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/delete.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = delete.Height * 0.8, Width = delete.Width * 0.8 };
                delete.Click += new System.Windows.RoutedEventHandler(delete_Click);
                delete.Uid = series_list_warp.Children.Count.ToString();

                hide.Height = wrapPanel.Height;
                hide.Width = wrapPanel.Width / 10;
                hide.BorderThickness = new Thickness(0);
                hide.Background = new SolidColorBrush(Color.FromArgb(0, 0, 0, 0));
                hide.Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/hidden.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = hide.Height * 0.8, Width = hide.Width * 0.8 };
                hide.Click += new System.Windows.RoutedEventHandler(hide_Click);
                hide.Uid = "hide";

                wrapPanel.Children.Add(label);
                wrapPanel.Children.Add(edit);
                wrapPanel.Children.Add(delete);
                wrapPanel.Children.Add(hide);
                border.Child = wrapPanel;
                series_list_warp.Children.Add(border);
                border = null;
                wrapPanel = null;
                edit = null;
                delete = null;
                hide = null;
            }
        }

        private void series_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            for (int i = 0; i < series_list_warp.Children.Count; i++)
            {
                ((Border)series_list_warp.Children[i]).BorderThickness = new Thickness(0);
            }

            ((Border)((WrapPanel)sender).Parent).BorderThickness = new Thickness(2);
            ((Border)((WrapPanel)sender).Parent).BorderBrush = new SolidColorBrush(Color.FromArgb(255, 75, 25, 255));


        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            Plot_Area.Series.Clear();
            work_series.ItemsSource = null;
            list_index.Clear();
            list_x.Clear();
            list_y.Clear();
            list_point.Children.Clear();
            grapf.Model = null;
            grapf.Model = Plot_Area;
        }

        private void edit_Click(object sender, RoutedEventArgs e)
        {

        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            list_function.RemoveAt(atoi(((Button)sender).Uid));
            plot_list[0].Series.RemoveAt(atoi(((Button)sender).Uid) + 2);
            series_list_warp.Children.RemoveAt(atoi(((Button)sender).Uid));
            for (int i = 0; i < series_list_warp.Children.Count; i++)
            {
                ((Button)((WrapPanel)((Border)series_list_warp.Children[i]).Child).Children[2]).Uid = i.ToString();
            }
        }
        private void hide_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Uid == "hide")
            {
                ((Button)sender).Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/view.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = ((Button)sender).Height * 0.8, Width = ((Button)sender).Width * 0.8 };
                ((Button)sender).Uid = "view";
            }
            else if (((Button)sender).Uid == "view")
            {
                ((Button)sender).Content = new Image { Source = new BitmapImage(new Uri("pack://application:,,,/image/hidden.png")), VerticalAlignment = System.Windows.VerticalAlignment.Center, HorizontalAlignment = System.Windows.HorizontalAlignment.Center, Height = ((Button)sender).Height * 0.8, Width = ((Button)sender).Width * 0.8 };
                ((Button)sender).Uid = "hide";
            }
        }
    }
}