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
            { "Cycle_time", 10}, // day - night cycle
            { "Update_on_hour", 30}, // fps update night overlay on one hour
            { "MainLoopMech_FPS", 60}, // main loop fps
        };


        public static Dictionary<int, string> Sprites = new Dictionary<int, string>()
        {
            { 1 , "../../../../Resources/Sprites/Zombies/dub.png" },
            { 2 , "../../../../Resources/Sprites/Zombies/git.png" },
            { 3 , "../../../../Resources/Sprites/Zombies/lestn.png" },
        };


        public static Dictionary<MyColors, Color> MyColorsList = new Dictionary<MyColors, Color>()
        {
            { MyColors.Red, Color.FromArgb(0, 255, 255, 255) },
            { MyColors.Yellow, Color.FromArgb(0, 0, 0, 0) },
            { MyColors.White, Color.FromArgb(0, 0, 0, 0) }
        };

        #endregion

        #region Service Classes_for_game_management

        Day_cycle_service _day_cycle_manager;
        Balance_service _balance_service;
        Blocks_service _blocks_service;
        StatisticMech _statistic;
        Build_mechanic _build_Mechanic;
        NotificationMech _not_mech;
        ActionsService _actionsService;


        public static EnemyService _enemyService;
        public static MainLoopMech MainLoopMech;
        public static Game_main_timers_service main_timers; // public and static for access from all programm space


        #endregion


        public static Random Random = new Random();

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

        public void Start_enemy_service(Grid grid)
        {
            _enemyService = new EnemyService(grid);
        }


        public void Start_ActionsService()
        {
            _actionsService = new ActionsService();
        }

        public void Start_StatisticMech(Grid textBlock, object win)
        {
            _statistic = new StatisticMech(textBlock, win);
        }

        public void Start_MainLoopMech()
        {
            MainLoopMech = new MainLoopMech();
            MainLoopMech.Start_loop();
        }

        public void Start_Block_service(Dictionary<string, Main_block> blocks)
        {
            _blocks_service = new Blocks_service(blocks);
            _blocks_service.Start_setter();

            // Send balance to balance service from given blocks
            _balance_service.Set_balance(Convert.ToInt32(blocks["Balance_block"].Text));
        }

        public void Start_day_cycle_service(Canvas DaYCycleInfo)
        {
            _day_cycle_manager = new Day_cycle_service((int)Game_Settings["Cycle_time"], (int)Game_Settings["Update_on_hour"], DaYCycleInfo);
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

        public void Start_NotificationMech(TextBlock block, object win)
        {
            _not_mech = new NotificationMech(block, win);
        }


    }

    

}
