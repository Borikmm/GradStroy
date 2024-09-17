using Gradostroy.Main_mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


public class ActionsService
{

    public static Action<Building> ABuildBuilded;
    public static Action<Building> ABuildDestroyed;
    public static Action<Enemy> AZombieKilled;


    public static Action ActionStartSpawn;
    public static Action ActionStopSpawn;

    public ActionsService()
    {
        ABuildBuilded += BuildBuilded;
        ABuildDestroyed += BuildDestroyed;
        AZombieKilled += ZombieKilled;
    }

    private void ZombieKilled(Enemy enemy)
    {
        StatisticMech.Achange_statistic?.Invoke(enemy, "Killed"); // change statistic
    }

    private void BuildBuilded(Building building)
    {
        StatisticMech.Achange_statistic?.Invoke(building, "build"); // change statistic
    }

    private void BuildDestroyed(Building building)
    {
        StatisticMech.Achange_statistic?.Invoke(building, "Destroy"); // change statistic
    }
}

