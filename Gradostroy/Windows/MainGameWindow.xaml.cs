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
        public static Dictionary<string, Building> Buildings_list = new Dictionary<string, Building>();

        // Need parametres for building
        private string cursor_fillen; //type action selected in menu
        private string type_build_selected;

        // In window
        public static Action<int> Abuild_or_destroy;
        public static Func<int, bool> Acheck_balance;
        public static Action<Canvas> ADestroyBuilding;

        // In Building
        public static Action<int> AEarn_money;
        private int colBuilding = 0;
        #endregion

        // Set block text and start it
        public Dictionary<string, Main_block> BlocksInfo;
        public Canvas DaYCycleInfo;
        public Grid StatisticGridInfo;
        public TextBlock NoMoneyNotificationBlock;

        public static Grid MainGrid;

        public MainGameWindow()
        {
            InitializeComponent();
            SubAllActions();
            SetGameInfo();
        }


        private void SubAllActions()
        {
            ADestroyBuilding += Destroy_canvas;
        }

        private void SetGameInfo()
        {
            BlocksInfo = new Dictionary<string, Main_block>()
            {
                {"Version_block" , new Main_block("Version_block", "0.5", "Version: ", Version_block) },
                {"Devel_block" , new Main_block("Devel_block", "Borikmm", "By: ", Devel_block) },
                {"Balance_block" , new Main_block("Balance_block", "100", "$", Balance_block, true) },
                {"Time_block" , new Main_block("Time_block", "6", ":00", Time_block, true) },
                {"MiningSpeed_block" , new Main_block("MiningSpeed_block", "0", " in one seconds", Mining_Speed_block, true) },

                // statistic blocks
                {"Col_buildings_block" , new Main_block("Col_buildings_bloc", "0", "buildings: ", Col_buildings) }, // need doing with EventHandler
            };

            MainGrid = MainGameObjectsGrid;

            DaYCycleInfo = Night_overlay;

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
                    Tower cube = new Tower(MainGrid);
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
                        Destroy_canvas((Canvas)building);
                        try
                        {
                            Abuild_or_destroy?.Invoke(((Building)((Canvas)building).Tag).Sell_Cost);
                        }
                        catch { }
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


        public void Destroy_canvas(Canvas canvas)
        {
            ((RenderElement)canvas.Tag).Destroy((RenderElement)canvas.Tag);

            switch (canvas.Tag)
            {
                case Building build:
                    Buildings_list.Remove(build.Name);
                    break;
                case Enemy enemy:
                    AEarn_money?.Invoke(-enemy.GoldEarned);
                    Service._enemyService.RemoveEnemy(enemy);
                    break;
            }
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
            Building.Name = build.BaseName + colBuilding++;
            Building.Tag = build;


            MainGameObjectsGrid.Children.Add(Building);
            build.Name = build.BaseName + colBuilding;
            // Call actions
            Abuild_or_destroy?.Invoke(build.Cost); // For balance changes

            ActionsService.ABuildBuilded?.Invoke((Building)build);

            Buildings_list.Add(build.Name, build);


        }


        private void Change_Action(object sender, RoutedEventArgs e)
        {
            cursor_fillen = ((MenuItem)sender).Header.ToString();
        }
    }
}
