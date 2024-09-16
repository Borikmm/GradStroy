

using Gradostroy;
using Gradostroy.Main_mechanics;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

public class EnemyService
{
    private List<Enemy> _enemys;

    public List<Enemy> Enemys => _enemys;

    private Grid _grid;


    int _zombieHeight = 100;
    int _zombieWight = 150;



    public void RemoveEnemy(Enemy enemy)
    {
        if (_enemys.Contains(enemy))
        {
            _enemys.Remove(enemy);
        }
            
            
    }


    public EnemyService(Grid grid)
    {

        _enemys = new List<Enemy>();
        _grid = grid;

        ActionsService.ActionStartSpawn += Start;
        ActionsService.ActionStopSpawn += Stop;


        Service.main_timers.Spawn_enemy_timer.action += SpawnEnemy;

    }


    private void Start()
    {
        Service.main_timers.Spawn_enemy_timer.Start();
    }

    private void Stop()
    {
        Service.main_timers.Spawn_enemy_timer.Stop();
    }


    public void SpawnEnemy()
    {
        CreateEnemy(new Zombie());
    }


    private void CreateEnemy(Enemy enemy)
    {
        // rn
        int x;
        int y;

        var what = Service.Random.Next(0, 2);
        if (what == 0)
        {
            x = (int)_grid.ActualWidth - 100;
            y = Service.Random.Next(0, (int)_grid.ActualHeight);
        }
        else
        {
            x = Service.Random.Next(0, (int)_grid.ActualWidth);
            y = (int)_grid.ActualHeight - 100;
        }


        var renderEnemy = enemy.Render(x, y, _zombieWight, _zombieHeight, Service.Sprites[Service.Random.Next(1, 4)]);


        enemy.Name = enemy.BaseName + _enemys.Count.ToString();
        renderEnemy.Tag = enemy;


        _grid.Children.Add(renderEnemy);
        //renderEnemy.Background = new SolidColorBrush(Service.MyColorsList[MyColors.Red]);


        _enemys.Add(enemy);
    }




}

