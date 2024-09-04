using System;
using System.Windows;
using System.Windows.Controls;


namespace Gradostroy.Main_mechanics
{
    public class StatisticMech
    {

        public static Action<Building, string> Achange_statistic;

        Grid _grid_statistic { get; set; }
        object _main_window;

        public StatisticMech(Grid text_block_link, object win) 
        {
            _grid_statistic = text_block_link;
            this._main_window = win;
            ((Window)this._main_window).StateChanged += MainWindow_StateChanged;

            Achange_statistic += Change_statistic;
        }

        public void MainWindow_StateChanged(object sender, EventArgs e)
        {
            if (((Window)_main_window).WindowState == WindowState.Maximized)
            {
                Change_Visibility(true);
            }
            else if (((Window)_main_window).WindowState == WindowState.Normal)
            {
                Change_Visibility(false);
            }
        }

        private void Change_Visibility(bool state)
        {
            _grid_statistic.Visibility = state ? Visibility.Visible : Visibility.Hidden;
        }


        private void Change_statistic(Building building, string action)
        {
            Blocks_service.AUpdateStatisticBlock?.Invoke(building, action);
        }

    }
}
