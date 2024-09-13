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

            service.Start_enemy_service(GameWindow.MainGrid);
        }


        private void CreateGameWindow()
        {
            GameWindow = new MainGameWindow();
        }


        private void StartGame()
        {
            MainFrame.Navigate(GameWindow);
        }
    }
}
