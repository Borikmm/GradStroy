using Gradostroy.Main_mechanics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ActionsService
{

    public static Action<Building> ABuildBuilded;
    public static Action<Building> ABuildDestroyed;


    public static Action ActionStartSpawn;
    public static Action ActionStopSpawn;

    public ActionsService()
    {
        ABuildBuilded += BuildBuilded;
        ABuildDestroyed += BuildDestroyed;
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

