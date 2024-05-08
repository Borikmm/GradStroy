using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;
using System.Windows;
using System.Windows.Controls;
using System.Diagnostics;

namespace Gradostroy.Main_mechanics
{
    public class Statistic_mech
    {
        Grid _grid_statistic { get; set; }
        object _main_window;

        public Statistic_mech(Grid text_block_link, object win) 
        {
            _grid_statistic = text_block_link;
            this._main_window = win;
            ((Window)this._main_window).StateChanged += MainWindow_StateChanged;
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

    }
}
