using Gradostroy.Main_mechanics;
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
    /// Class for control to main mechanics classes.
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

        Day_cycle_service _day_cycle_manager;
        Balance_service _balance_service;
        Blocks_service _blocks_service;
        Statistic_mech _statistic;
        Build_mechanic _build_Mechanic;
        Notification_mech _not_mech;


        public static Main_loop main_loop;
        public static Game_main_timers_service main_timers; // public and static for access from all programm space


        #endregion


        Service() 
        {
            Start_balance_service();
        }

        ~Service()
        {
            
        }

        private void Start_balance_service()
        {
            _balance_service = new Balance_service();
        }

        public void Start_statistic_mech(Grid textBlock, object win)
        {
            _statistic = new Statistic_mech(textBlock, win);
        }

        public void Start_Main_loop()
        {
            main_loop = new Main_loop();
            main_loop.Start_loop();
        }

        public void Start_Block_service(Dictionary<string, Main_block> blocks)
        {
            _blocks_service = new Blocks_service(blocks);
            _blocks_service.Start_setter();

            // Send balance to balance service from given blocks
            _balance_service.Set_balance(Convert.ToInt32(blocks["Balance_block"].Text));
        }

        public void Start_day_cycle_service(int cycle_time, int Update_on_hour, Tuple<int, int> day_night_relationship, Canvas Night_Overlay)
        {
            _day_cycle_manager = new Day_cycle_service(cycle_time, Update_on_hour, day_night_relationship, Night_Overlay);
            _day_cycle_manager.Start_day_cycle();
        }


        public void Start_all_timers_service()
        {
            main_timers = new Game_main_timers_service();
            main_timers.Start_timers();
        }

        public void Start_build_mechanic()
        {
            _build_Mechanic = new Build_mechanic();
        }

        public void Start_notification_mech(TextBlock block, object win)
        {
            _not_mech = new Notification_mech(block, win);
        }


    }

    /// <summary>
    /// Class for text blocks working. 
    /// </summary>
    public class Blocks_service
    {

        public static Dictionary<string, Main_block> blocks;

        public static Action<string> AUpdateStatisticBlock;


        public Blocks_service(Dictionary<string, Main_block> blocks_) 
        {
            Balance_service.AUpdate_balance += Update_balance_block;
            Day_cycle_service.ATimeChanged += Change_time_block;

            blocks = blocks_;

            AUpdateStatisticBlock += Update_statistic_block;
        }

        ~Blocks_service()
        {
            Balance_service.AUpdate_balance -= Update_balance_block;
            Day_cycle_service.ATimeChanged -= Change_time_block;
        }


        private void Change_time_block(int time)
        {
            blocks["Time_block"].link.Text = time.ToString() + " " + blocks["Time_block"].Spliter;
        }

        private void Update_balance_block(int balance)
        {
            blocks["Balance_block"].link.Text = blocks["Balance_block"].Spliter + " " + balance.ToString();
        }

        private void Update_statistic_block(string name) // increment
        {
            switch (name)
            {
                case "Destroy":
                    blocks["Col_buildings_block"].Text = (Convert.ToInt16(Blocks_service.blocks["Col_buildings_block"].Text) - 1).ToString();
                    blocks["Col_buildings_block"].link.Text = blocks["Col_buildings_block"].Spliter + blocks["Col_buildings_block"].Text;
                    break;
                case "House":
                    blocks["Col_buildings_block"].Text = (Convert.ToInt16(Blocks_service.blocks["Col_buildings_block"].Text) + 1).ToString();
                    blocks["Col_buildings_block"].link.Text = blocks["Col_buildings_block"].Spliter + blocks["Col_buildings_block"].Text;
                    break;
            }
        
        }


        public void Start_setter()
        {
            foreach (Main_block setting in blocks.Values)
            {
                if (setting.reverse_spliter)
                    setting.link.Text = setting.Text + " " + setting.Spliter;
                else
                    setting.link.Text = setting.Spliter + " " + setting.Text;
            }
        }

    }

    /// <summary>
    /// Class for all timers working. For working need write new MyTimer instance
    /// </summary>
    public class Game_main_timers_service
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

    public class Day_cycle_service
    {
        DispatcherTimer Day_cycle_timer;
        DispatcherTimer Hour_timer;

        Tuple<int, int> day_night_relationship; // 70 - day : 30 - night
        Canvas Night_Overlay;
        int now_time = 6; // 6 - 24
        double Update_color_FPS;
        byte transparencyLevel = 0;

        public static Action<int> ATimeChanged;

        public Day_cycle_service(double cycle_time, int Update_on_hour, Tuple<int, int> day_night_relationship, Canvas Night_Overlay)
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

    public class Balance_service
    {
        private int _balance;

        // For balance block update in Blocks_service
        public static Action<int> AUpdate_balance { get; set; }

        public Balance_service()
        {
            MainWindow.Abuild_or_destroy += Change_Balance;
            MainWindow.Acheck_balance += Check_Balance;
            MainWindow.AEarn_money += Change_Balance;
        }

        ~Balance_service()
        {
            MainWindow.Abuild_or_destroy -= Change_Balance;
            MainWindow.Acheck_balance -= Check_Balance;
            MainWindow.AEarn_money -= Change_Balance;
        }

        public void Set_balance(int balance)
        {
            _balance = balance;
        }

        private bool Check_Balance(int cost)
        {
            if (_balance < cost)
                return true;
            else return false;
        }

        private void Change_Balance(int number)
        {
            _balance -= number;
            AUpdate_balance?.Invoke(_balance);
        }

    }

}
