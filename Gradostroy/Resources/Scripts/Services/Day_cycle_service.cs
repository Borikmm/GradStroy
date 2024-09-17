using Gradostroy;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

/// <summary>
/// Class for change day cycle
/// </summary>
public class Day_cycle_service
{
    DispatcherTimer Day_cycle_timer;
    DispatcherTimer Hour_timer;

    //Tuple<int, int> day_night_relationship; // 70 - day : 30 - night
    Canvas Night_Overlay;
    int now_time = 7; // 6 - 24
    double Update_color_FPS;
    byte transparencyLevel = 0;

    public static Action<int> ATimeChanged;


    public static int NumberNight = 0;


    public Day_cycle_service(double cycle_time, int Update_on_hour, Canvas Night_Overlay)
    {
        // Hour Timer setter
        Hour_timer = new DispatcherTimer();
        Hour_timer.Interval = TimeSpan.FromSeconds(cycle_time / 24); // division for hour
        Hour_timer.Tick += Hour_timer_actions;

        // Cycle FPS update
        Day_cycle_timer = new DispatcherTimer();
        Day_cycle_timer.Interval = TimeSpan.FromSeconds(cycle_time / 24 / Update_on_hour);
        Day_cycle_timer.Tick += Day_cycle_actions;

        this.Update_color_FPS = Update_on_hour;

        this.Night_Overlay = Night_Overlay;


        ATimeChanged?.Invoke(now_time);

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
            if (Convert.ToDouble(transparencyLevel) + find_sum_number < 210)
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


    // Need to create difficultService!!!!!!
    private void ChangeDifficult()
    {
        var difNow = Game_main_timers_service.timers_ticks["Spawn_enemy_timer"];


        if (difNow >= 0.3)
        {
            Game_main_timers_service.timers_ticks["Spawn_enemy_timer"] -= 0.2f;
        }
    }

    private void Hour_timer_actions(object sender, EventArgs e)
    {
        if (now_time == 24)
        {
            now_time = 0;
            NumberNight++;
            // Need to rename action to start night or day
            ActionsService.ActionStartSpawn?.Invoke();




        }

        if (now_time == 6)
        {
            ActionsService.ActionStopSpawn?.Invoke();
            ChangeDifficult();

            Service.main_timers.Spawn_enemy_timer.ChangeInterval(Game_main_timers_service.timers_ticks["Spawn_enemy_timer"]); // Need Create dificult service who changes value
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