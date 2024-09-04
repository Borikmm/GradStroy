

using Gradostroy.Main_mechanics;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;

public class EnemyService
{
    private List<Enemy> _enemys;

    private Grid _grid;

    public EnemyService(Grid grid)
    {
        _grid = grid;
        _enemys = new List<Enemy>();
    }


    private void CreateEnemy(Enemy enemy)
    {
        // rnd
        //var renderEnemy = enemy.Render(x, y);
        //_grid.Children.Add(Building);
        _enemys.Add(enemy);
    }




}

