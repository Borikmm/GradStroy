using Gradostroy.Main_mechanics;
using Gradostroy.Windows;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Gradostroy
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool full_window = false;
        MainGameWindow GameWindow;
        // Create service for working with balance, blocks and other game mechanics
        Service service = Service.Instance;

        public MainWindow()
        {
            InitializeComponent();

            CreateGameWindow();

            SetupServices();

            StartGame();
        }


        private void SetupServices()
        {

            // Main services:
            service.Start_all_timers_service();
            service.Start_MainLoopMech();
            //
            service.Start_ActionsService();

            service.Start_Block_service(GameWindow.BlocksInfo);

            service.Start_day_cycle_service(GameWindow.DaYCycleInfo);

            service.Start_StatisticMech(GameWindow.StatisticGridInfo, this);

            service.Start_NotificationMech(GameWindow.NoMoneyNotificationBlock, this);

            service.Start_enemy_service(MainGameWindow.MainGrid);
        }


        private void CreateGameWindow()
        {
            GameWindow = new MainGameWindow();
        }


        private void StartGame()
        {
            MainFrame.Navigate(GameWindow);
        }

        private void ToolBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void Close_button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Rol_up_button_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void Max_window_button_button_Click(object sender, RoutedEventArgs e)
        {
            if (full_window)
            {
                full_window = false;
                this.WindowState = WindowState.Normal;
            }
            else
            {
                full_window = true;
                this.WindowState = WindowState.Maximized;
            }
        }
    }
}
