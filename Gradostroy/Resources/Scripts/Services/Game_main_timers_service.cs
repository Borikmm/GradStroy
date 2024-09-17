using Gradostroy;
using System.Collections.Generic;

/// <summary>
/// Class for all timers working. For working need write new MyTimer instance
/// </summary>
public class Game_main_timers_service
{

    public static Dictionary<string, float> timers_ticks = new Dictionary<string, float>()
    {
        {"Mining_timer", 1},
        {"Spawn_enemy_timer", 2f },
    };

    public MyTimer Mining_timer = new MyTimer(tick: timers_ticks["Mining_timer"]);
    public MyTimer OneSec_timer = new MyTimer(tick: 1);
    public MyTimer Spawn_enemy_timer = new MyTimer(tick: timers_ticks["Spawn_enemy_timer"]);

    public void Start_timers()
    {
        Mining_timer.Start();
        OneSec_timer.Start();
    }


}
