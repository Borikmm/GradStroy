using Gradostroy.Main_mechanics;
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

namespace Gradostroy.Windows
{
    /// <summary>
    /// Логика взаимодействия для MainGameWindow.xaml
    /// </summary>
    public partial class MainGameWindow : Page
    {

        #region Building build_params
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
        #endregion

        // Set block text and start it
        public Dictionary<string, Main_block> BlocksInfo;
        public Tuple<int, int, Canvas> DaYCycleInfo;
        public Grid StatisticGridInfo;
        public TextBlock NoMoneyNotificationBlock;

        public MainGameWindow()
        {
            InitializeComponent();
            SetGameInfo();
        }

        private void SetGameInfo()
        {
            BlocksInfo = new Dictionary<string, Main_block>()
            {
                {"Version_block" , new Main_block("Version_block", "0.2", "Version: ", Version_block) },
                {"Devel_block" , new Main_block("Devel_block", "Borikmm", "By: ", Devel_block) },
                {"Balance_block" , new Main_block("Balance_block", "100", "Balance: ", Balance_block) },
                {"Time_block" , new Main_block("Time_block", "6", ":00", Time_block, true) },
                {"MiningSpeed_block" , new Main_block("MiningSpeed_block", "0", " in one seconds", Mining_Speed_block, true) },

                // statistic blocks
                {"Col_buildings_block" , new Main_block("Col_buildings_bloc", "0", "buildings: ", Col_buildings) }, // need doing with EventHandler
            };


            DaYCycleInfo = new Tuple<int, int, Canvas>(
                Convert.ToInt16(Service.Game_Settings["Cycle_time"]),
                Convert.ToInt16(Service.Game_Settings["Update_on_hour"]),
                Night_overlay);

            StatisticGridInfo = Statistic_block;

            NoMoneyNotificationBlock = no_money_notification;

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
            switch (type_build)
            {
                case "Cube":
                    Cube cube = new Cube();
                    return cube;
                case "House":
                    House house = new House();
                    return house;
                default: return null;

            }
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
                    {
                        ActionsService.ABuildDestroyed?.Invoke(((Building)((Canvas)building).Tag));
                        Destroy_build(building);
                    }
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
                Build_mechanic.ANomoney?.Invoke();
                return;
            }

            build.Start_fixed_update();

            var Building = build.Render(x, y);

            Building.MouseDown += new MouseButtonEventHandler(Building_MouseDown);
            Building.Name = build.Name + Buildings_list.Count().ToString();
            Building.Tag = build;


            Main_content.Children.Add(Building);

            // Call actions
            Abuild_or_destroy?.Invoke(build.Cost); // For balance changes

            ActionsService.ABuildBuilded?.Invoke((Building)build);

            Buildings_list.Add(Building.Name, build);
        }


        private void Change_Action(object sender, RoutedEventArgs e)
        {
            cursor_fillen = ((MenuItem)sender).Header.ToString();
        }
    }
}
