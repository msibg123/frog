using Frog;
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
using System.Media;


namespace WpfApplication3
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        Image frog_image, car1_image, car2_image, car3_image;
        Thickness frog_m, car1_m, car2_m, car3_m;
        Thickness frog_start_m, car1_start_m, car2_start_m, car3_start_m;
        System.Timers.Timer t, t2, timer;
        bool flag = true;
        bool start = false;
        Label label_score, label_time;
        int now_score = 0, now_time = 30;
        int setting_time = 30;
        int speed = 10;
        int max_score = 10;
        Image horsee;

        public MainWindow()
        {

            InitializeComponent();
            init_score();
            init_frog_location();
            init_car();
            init_check();
            horsee = this.horse;
            horse.Visibility = Visibility.Hidden;
        }

        private void init_score()
        {
            label_score = this.score;
            now_score = 0;
            label_score.Content = now_score;
        }

        private void init_frog_location()
        {
            frog_image = this.frog;
            frog_m = frog_image.Margin;
            frog_start_m = frog_m;
        }

        private void reset_frog_location()
        {
            frog_image.Margin = frog_start_m;
            frog_m = frog_image.Margin;
        }



        private void init_car()
        {
            car1_image = this.car1;
            car2_image = this.car2;
            car3_image = this.car3;

            car1_m = car1_image.Margin;
            car2_m = car2_image.Margin;
            car3_m = car3_image.Margin;

            car1_start_m = car1_m;
            car2_start_m = car2_m;
            car3_start_m = car3_m;

            t = new System.Timers.Timer(10);
            t.Elapsed += new System.Timers.ElapsedEventHandler(car_run);
            t.AutoReset = true;
            t.Enabled = true;
        }

        private void init_check()
        {
            t2 = new System.Timers.Timer(10);
            t2.Elapsed += new System.Timers.ElapsedEventHandler(check_collision);
            t2.AutoReset = true;
            t2.Enabled = true;
        }

        delegate void ChgLabelHandler2();
        private void chgLabel2()
        {
            if (frog_m.Top > 100 && frog_m.Top < 200)
            {
                if (frog_m.Left == car1_m.Left)
                {
                    die();
                }
            }
            else if (frog_m.Top > 200 && frog_m.Top < 300)
            {
                if (frog_m.Left == car2_m.Left)
                {
                    die();
                }
            }
            else if (frog_m.Top > 300)
            {
                if (frog_m.Left == car3_m.Left)
                {
                    die();
                }
            }
        }

        private void die()
        {
            timer.Stop();
            t2.Stop();
            t.Stop();

            BitmapImage bi3 = new BitmapImage();
            bi3.BeginInit();
            bi3.UriSource = new Uri("frog-2.png", UriKind.Relative);
            bi3.EndInit();
            frog_image.Source = bi3;

            MessageBox.Show("被撞死了！總得分：" + now_score);
            max();

            BitmapImage bi2 = new BitmapImage();
            bi2.BeginInit();
            bi2.UriSource = new Uri("frog-1.png", UriKind.Relative);
            bi2.EndInit();
            frog_image.Source = bi2;

            reset_frog_location();
            init_score();
            start = false;
            time.Content = setting_time;
            t.Start();
            t2.Start();
        }

        private void init_timer()
        {
            label_time = this.time;
            now_time = setting_time;
            label_time.Content = now_time;
            timer = new System.Timers.Timer(1000);
            timer.Elapsed += new System.Timers.ElapsedEventHandler(set_timer);
            timer.AutoReset = true;
            timer.Enabled = true;
        }

        delegate void ChgLabelHandler3();
        private void chgLabel3()
        {
            now_time--;
            label_time.Content = now_time;
            if (now_time == 0)
            {
                timer.Stop();
                t2.Stop();
                t.Stop();
                MessageBox.Show("遊戲結束！總得分：" + now_score);
                max();
                horse.Visibility = Visibility.Hidden;

                BitmapImage bi2 = new BitmapImage();
                bi2.BeginInit();
                bi2.UriSource = new Uri("frog-1.png", UriKind.Relative);
                bi2.EndInit();
                frog_image.Source = bi2;
                reset_frog_location();
                init_score();
                this.time.Content = setting_time;
                start = false;
                t.Start();
                t2.Start();
            }
        }

        private void sound()
        {
            SoundPlayer _sp = new SoundPlayer();
            _sp.SoundLocation = "sound.wav";
            _sp.Play();                     
        }

        private void max()
        {
            if (now_score > max_score)
            {
                Label lb_max = this.tv_max_score;

                lb_max.Content = now_score;
                max_score = now_score;
            }
        }

        public void set_timer(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new ChgLabelHandler3(chgLabel3));
        }

        public void check_collision(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new ChgLabelHandler2(chgLabel2));
        }

        public void car_run(object source, System.Timers.ElapsedEventArgs e)
        {
            this.Dispatcher.Invoke(new ChgLabelHandler(chgLabel));
        }

        delegate void ChgLabelHandler();
        private void chgLabel()
        {
            fun_car1();
            fun_car2();
            fun_car3();
        }

        private void fun_car1()
        {
            if (flag)
            {
                car1_m.Right = car1_m.Right + speed;
                car1_m.Left = car1_m.Left - speed;
                car1_image.Margin = car1_m;
                if (car1_m.Left < 0)
                {
                    flag = false;
                }
            }
            else
            {
                car1_m.Right = car1_m.Right - 10;
                car1_m.Left = car1_m.Left + 10;
                car1_image.Margin = car1_m;
                if (car1_m.Right < 0)
                {
                    flag = true;
                }
            }
        }

        private void fun_car2()
        {
            car2_m.Right = car2_m.Right + speed;
            car2_m.Left = car2_m.Left - speed;
            car2_image.Margin = car2_m;
            if (car2_m.Left < -139)
            {
                car2_image.Margin = car2_start_m;
                car2_m = car2_start_m;
            }
        }

        private void fun_car3()
        {
            car3_m.Right = car3_m.Right - speed;
            car3_m.Left = car3_m.Left + speed;
            car3_image.Margin = car3_m;
            if (car3_m.Right < -139)
            {
                car3_image.Margin = car3_start_m;
                car3_m = car3_start_m;
            }
        }


        private void Window_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!start)
            {
                start = true;
                init_timer();
            }

            Key kPressed;
            if (e.Key == Key.ImeProcessed)
                kPressed = e.ImeProcessedKey;
            else
                kPressed = e.Key;
            switch (kPressed)
            {
                case Key.Down:
                    frog_m.Top = frog_m.Top + 90;
                    frog_m.Bottom = frog_m.Bottom - 90;
                    frog_image.Margin = frog_m;
                    e.Handled = true;
                    break;
                case Key.Up:
                    frog_m.Top = frog_m.Top - 90;
                    frog_m.Bottom = frog_m.Bottom + 90;
                    frog_image.Margin = frog_m;
                    e.Handled = true;
                    break;
                case Key.Left:
                    frog_m.Left = frog_m.Left - 90;
                    frog_m.Right = frog_m.Right + 90;
                    frog_image.Margin = frog_m;
                    e.Handled = true;
                    break;
                case Key.Right:
                    frog_m.Left = frog_m.Left + 90;
                    frog_m.Right = frog_m.Right - 90;
                    frog_image.Margin = frog_m;
                    e.Handled = true;
                    if (frog_m.Right < 0)
                    {
                        t2.Stop();

                        BitmapImage bi3 = new BitmapImage();
                        bi3.BeginInit();
                        bi3.UriSource = new Uri("strong.png", UriKind.Relative);
                        bi3.EndInit();
                        frog_image.Source = bi3;
                        reset_frog_location();
                        sound();
                        horse.Visibility = Visibility.Visible;

                    }
                    break;
                case Key.Space:
                    frog_m.Top = frog_m.Top + 310;
                    frog_m.Bottom = frog_m.Bottom - 311;
                    frog_image.Margin = frog_m;
                    e.Handled = true;
                    break;
            }
            if (frog_m.Bottom < 0)
            {
                reset_frog_location();
                now_score++;
                label_score.Content = now_score;
            }

        }
        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (start)
            {
                timer.Stop();
            }

            MessageBox.Show("按上下左右鍵操作青蛙，必須躲避汽車，讓青蛙順利過街。", "遊戲說明", MessageBoxButton.OK, MessageBoxImage.Information);

            reset_frog_location();
            init_score();
            start = false;


            time.Content = setting_time;
            t.Start();
            t2.Start();
        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AboutBox1 about = new AboutBox1();
            about.Show();
        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MenuItem_Click_3(object sender, RoutedEventArgs e)
        {
            setting_time = 10;
            this.time.Content = setting_time;
        }
        private void MenuItem_Click_4(object sender, RoutedEventArgs e)
        {
            setting_time = 20;
            this.time.Content = setting_time;
        }
        private void MenuItem_Click_5(object sender, RoutedEventArgs e)
        {
            setting_time = 30;
            this.time.Content = setting_time;
        }

        private void MenuItem_Click_6(object sender, RoutedEventArgs e)
        {
            speed = 10;
        }
        private void MenuItem_Click_7(object sender, RoutedEventArgs e)
        {
            speed = 20;
        }
        private void MenuItem_Click_8(object sender, RoutedEventArgs e)
        {
            speed = 30;
        }
    }


}
