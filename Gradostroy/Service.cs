using Gradostroy.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace Gradostroy
{
    /// <summary>
    /// Class for game management. Set start values to need blocks
    /// </summary>
    public class Service
    {
        #region Service Singleton
        private static Service instance;
        public static Service Instance
        {
            get
            {
                // Если экземпляр еще не создан, создаем его
                if (instance == null)
                {
                    instance = new Service();
                }
                return instance;
            }
        }
        #endregion

        #region Service Main_game_parametres

        public int Balance;
        Dictionary<string, Main_block> blocks;

        /// <summary>
        /// Main game settings, contains importain info for all classes
        /// </summary>
        public static readonly Dictionary<string, object> Game_Settings = new Dictionary<string, object>()
        {
            { "Cycle_time", 120}, // day - night cycle
            { "Update_on_hour", 30}, // fps update night overlay on one hour
            { "Main_loop_FPS", 30}, // main loop fps
        };

        #endregion

        #region Service Classes_for_game_management

        Day_cycle day_cycle_manager;
        public static Game_main_timers main_timers;

        #endregion


        Service()
        {
            MainWindow.Abuild_or_destroy += Change_Balance;
            MainWindow.Acheck_balance += Check_Balance;
            MainWindow.AEarn_money += Change_Balance;

            Day_cycle.ATimeChanged += Change_time;

        }

        ~Service()
        {
            MainWindow.Abuild_or_destroy -= Change_Balance;
            MainWindow.Acheck_balance -= Check_Balance;
            MainWindow.AEarn_money -= Change_Balance;

            Day_cycle.ATimeChanged -= Change_time;
        }


        private bool Check_Balance(int cost)
        {
            if (this.Balance < cost)
                return true;
            else return false;
        }

        private void Change_Balance(int number)
        { 
            this.Balance -= number;

            blocks["Balance_block"].link.Text = blocks["Balance_block"].Spliter + " " + this.Balance.ToString();
        }

        private void Change_time(int time)
        {
            blocks["Time_block"].link.Text = time.ToString() + " " + blocks["Time_block"].Spliter;
        }


        public void Start_setter(Dictionary<string, Main_block> blocks)
        {
            this.blocks = blocks;

            this.Balance = Convert.ToInt16(blocks["Balance_block"].Text);

            foreach (Main_block setting in blocks.Values)
            {
                if (setting.reverse_spliter)
                    setting.link.Text = setting.Text + " " + setting.Spliter;
                else
                    setting.link.Text = setting.Spliter + " " + setting.Text;
            }
        }

        public void Start_day_cycle(int cycle_time, int Update_on_hour, Tuple<int, int> day_night_relationship, Canvas Night_Overlay)
        {
            day_cycle_manager = new Day_cycle(cycle_time, Update_on_hour, day_night_relationship, Night_Overlay);
            day_cycle_manager.Start_day_cycle();
        }


        public void Start_all_timers()
        {
            main_timers = new Game_main_timers();
            main_timers.Start_timers();
        }
    }

    public class Game_main_timers
    {

        private static Dictionary<string, int> timers_ticks = new Dictionary<string, int>()
        {
            {"Mining_timer", 1},
        };


        // Mining
        public MyTimer Mining_timer = new MyTimer(tick: timers_ticks["Mining_timer"]);

        public void Start_timers()
        {
            Mining_timer.Start();
        }


    }

    public class Day_cycle
    {
        DispatcherTimer Day_cycle_timer;
        DispatcherTimer Hour_timer;

        Tuple<int, int> day_night_relationship; // 70 - day : 30 - night
        Canvas Night_Overlay;
        int now_time = 6; // 6 - 24
        double Update_color_FPS;
        byte transparencyLevel = 0;

        public static Action<int> ATimeChanged;

        public Day_cycle(double cycle_time, int Update_on_hour, Tuple<int, int> day_night_relationship, Canvas Night_Overlay)
        {
            // Hour Timer setter
            Hour_timer = new DispatcherTimer();
            Hour_timer.Interval = TimeSpan.FromSeconds(cycle_time/ 24); // division for hour
            Hour_timer.Tick += Hour_timer_actions;

            // Cycle FPS update
            Day_cycle_timer = new DispatcherTimer();
            Day_cycle_timer.Interval = TimeSpan.FromSeconds(cycle_time / 24 / Update_on_hour);
            Day_cycle_timer.Tick += Day_cycle_actions;

            this.Update_color_FPS = Update_on_hour;

            this.Night_Overlay = Night_Overlay;

            // this parametr dosnt working yet.....
            this.day_night_relationship = day_night_relationship;
        }

        /// <summary>
        /// Blackout and lightening overlay every tick Day_cycle_timer
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Day_cycle_actions(object sender, EventArgs e)
        {
            // 255 - 100
            //  x  - 2
            //------------/ hour switching
            // 100% / 12 * 4 = 2%
            // ------^ update fps

            // blackout overlay
            if (now_time >= 18 && now_time <= 22)
            {
                // find number, which one needs to add transparency to 
                double find_sum_number = (255 * (100 / (Update_color_FPS * 4))) / 100;

                // check for exceeding blackout
                if (Convert.ToDouble(transparencyLevel) + find_sum_number < 255)
                    // Addition transpanent
                    transparencyLevel = (byte)(Convert.ToDouble(transparencyLevel) + find_sum_number);


                // Set transpanent
                Color transparentColor = Color.FromArgb(transparencyLevel, 0, 0, 0);

                Night_Overlay.Background = new SolidColorBrush(transparentColor);
                
            }


            // lightening overlay
            if (now_time >= 6 && now_time <= 10)
            {
                double find_sum_number = 255 * (100 / (Update_color_FPS * 4)) / 100;

                // check for exceeding lightening
                if (Convert.ToDouble(transparencyLevel) - find_sum_number > 0)
                    transparencyLevel = (byte)(Convert.ToDouble(transparencyLevel) - find_sum_number);

                // Создание объекта Color с указанием цвета и уровня прозрачности
                Color transparentColor = Color.FromArgb(transparencyLevel, 0, 0, 0);

                Night_Overlay.Background = new SolidColorBrush(transparentColor);
            }
        }

        private void Hour_timer_actions(object sender, EventArgs e)
        {
            if (now_time == 24)
            {
                now_time = 0;
            }
            now_time++;
            ATimeChanged?.Invoke(now_time);
        }

        public void Start_day_cycle()
        {
            Hour_timer.Start();
            Day_cycle_timer.Start();
        }
    }
}
