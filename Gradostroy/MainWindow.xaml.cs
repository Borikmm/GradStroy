using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Gradostroy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        // buildings list
        private Dictionary<string, Building> Buildings_list = new Dictionary<string, Building>();

        // Need parametres for building
        private string cursor_fillen; //type action selected in menu
        private string type_build_selected;



        // In window
        public static Action<int> Abuild_or_destroy;
        public static Func<int, bool> Acheck_balance;

        // In Building
        public static Action<int> AEarn_money;

        // Create service for working with balance, blocks and other game management
        Service service = Service.Instance;


        // Notification no money
        private System.Timers.Timer notification_lost_timer;
        private bool notification_anim_start = false;

        public MainWindow()
        {
            InitializeComponent();

            // Set block text and start it
            Dictionary<string, Main_block> blocks = new Dictionary<string, Main_block>()
            {
                {"Version_block" , new Main_block("Version_block", "0.2", "Version: ", Version_block) },
                {"Devel_block" , new Main_block("Devel_block", "Borikmm", "By: ", Devel_block) },
                {"Balance_block" , new Main_block("Balance_block", "100", "Balance: ", Balance_block) },
                {"Time_block" , new Main_block("Time_block", "6", ":00", Time_block, true) },
                {"Col_buildings_block" , new Main_block("Col_buildings_bloc", "0", "buildings: ", Col_buildings) }, // need doing with EventHandler
            };
            
            service.Start_Block_service(blocks);

            // Start day night switch
            service.Start_day_cycle_service(
                cycle_time: Convert.ToInt16(Service.Game_Settings["Cycle_time"]), 
                Update_on_hour: Convert.ToInt16(Service.Game_Settings["Update_on_hour"]), 
                day_night_relationship:new Tuple<int, int>(70, 30),  // doesnt working
                Night_Overlay:Night_overlay
                );

            service.Start_all_timers_service();

            service.Start_Main_loop();

            service.Start_statistic_mech(Statistic_block, this);
            

        }

        private void Building_house(object sender, RoutedEventArgs e)
        {
            cursor_fillen = "build";
            type_build_selected = "House";
        }

        private void Building_tower(object sender, RoutedEventArgs e)
        {
            cursor_fillen = "build";
            type_build_selected = "Cube";
        }


        private Building Create_build(string type_build)
        {
            Building cursor_building_ = null;
            switch (type_build)
            {
                case "Cube":
                    Cube cube = new Cube();
                    cursor_building_ = cube;
                    break;
                case "House":
                    
                    House house = new House();
                    cursor_building_ = house;
                    break;

            }
            return cursor_building_;
        }



        private void Main_canvas_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Point currentPosition = Mouse.GetPosition(Application.Current.MainWindow);
            double x = currentPosition.X;
            double y = currentPosition.Y;
            Do_task(X: x, Y: y);
        }


        private void Do_task(object building = null, double X = 0, double Y = 0)
        {
            switch (cursor_fillen)
            {
                case "build":
                    if (X != 0)
                        Spawn_building_in_sursor(Convert.ToInt32(X), Convert.ToInt32(Y));
                    break;
                case "Destroy":
                    if (building != null)
                        Destroy_build(building);
                    break;
                case "info":
                    break;
            }
        }


        private void Building_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Do_task((UIElement)sender);
        }


        private void Destroy_build(object building)
        {
            try
            {
                Abuild_or_destroy?.Invoke(((Building)((Canvas)building).Tag).Sell_Cost);
            }
            catch { }

            // Unsubscribe 
            ((Building)((Canvas)building).Tag).UnSub();


            // Delete from canvas
            Main_content.Children.Remove((UIElement)building);
        }

        private void Spawn_building_in_sursor(int x, int y)
        {
            var build = Create_build(type_build_selected);

            if ((bool)(Acheck_balance?.Invoke(build.Cost)))
            {
                StartAnimation();
                return;
            }

            build.Start_fixed_update();

            var Building = build.Build(x, y);

            Building.MouseDown += new MouseButtonEventHandler(Building_MouseDown);
            Building.Name = build.Name + Buildings_list.Count().ToString();
            Building.Tag = build;


            Main_content.Children.Add(Building);


            Abuild_or_destroy?.Invoke(build.Cost);

            Buildings_list.Add(Building.Name, build);
        }


        private void Change_Action(object sender, RoutedEventArgs e)
        {
            cursor_fillen = ((MenuItem)sender).Header.ToString();
        }



        // Notification about not money
        private void StartAnimation()
        {

            if (notification_anim_start)
                return;

            int timer_notification_lost = 2000;


            notification_lost_timer = new System.Timers.Timer(timer_notification_lost); // Устанавливаем интервал таймера в 2 секунды
            notification_lost_timer.Elapsed += LostAnimation;

            DoubleAnimation fadeInAnimation = new DoubleAnimation();
            fadeInAnimation.From = 0;
            fadeInAnimation.To = 1;
            fadeInAnimation.Duration = TimeSpan.FromSeconds(1);


            no_money_notification.BeginAnimation(TextBlock.OpacityProperty, fadeInAnimation);

            notification_lost_timer.Start();

            notification_anim_start = true;
        }

        private void LostAnimation(object sender, ElapsedEventArgs e)
        {
            notification_lost_timer.Stop();
            notification_anim_start = false;

            Dispatcher.Invoke(() =>
            {
                DoubleAnimation fadeOutAnimation = new DoubleAnimation();
                fadeOutAnimation.From = 1;
                fadeOutAnimation.To = 0;
                fadeOutAnimation.Duration = TimeSpan.FromSeconds(1);

                no_money_notification.BeginAnimation(TextBlock.OpacityProperty, fadeOutAnimation);
            });
        }

    }
}
