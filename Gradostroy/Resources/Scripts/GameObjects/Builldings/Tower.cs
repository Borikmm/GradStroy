using Gradostroy;
using Gradostroy.Main_mechanics;
using Gradostroy.Windows;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

public class Tower : Building
{
    public Enemy ShoutTarget;


    public int ReloadTime = 2;
    private float _shoutingDistance = 300f;
    private int _timeRemaining = 0;
    private double _marginBulletX = 50;
    private double _marginBulletY = 50;


    private Grid _grid;

    public Tower(Grid grid)
    {

        _grid = grid;

        BaseName = "Tower";
        Cost = 10;
        Sell_Cost = -5;
        MainLoopMech.AFixedUpdate += FixedUpdate;
        Service.main_timers.OneSec_timer.action += Reload;
    }

    public override void Start_fixed_update()
    {

    }

    private void Shout()
    {
        Canvas bullet = new Bullet(ShoutTarget, position.x + _marginBulletX, position.y + _marginBulletY).CanvasRendered;

        _grid.Children.Add(bullet);
    }


    private void FindClosestEnemy()
    {

        if (ShoutTarget != null)
        {
            if (!ShoutTarget._isAlife)
            {

                ShoutTarget = null;
                return;
            }
        }

        if (Service._enemyService.Enemys.Count == 0)
        {

            ShoutTarget = null;
            return;
        }

        double targetX, targetY, deltaX, deltaY, distance = double.PositiveInfinity;
        Enemy nearest_obj = null;

        foreach (var target in Service._enemyService.Enemys)
        {
            // Получаем координаты цели
            targetX = (target).position.x;
            targetY = (target).position.y;

            // Вычисляем разницу между текущими координатами и координатами цели
            deltaX = targetX - position.x;
            deltaY = targetY - position.y;

            // Вычисляем расстояние до цели
            var new_distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);


            if (new_distance <= _shoutingDistance)
            {
                if (distance > new_distance)
                {
                    distance = new_distance;
                    nearest_obj = target;
                }
            }


        }
        if (nearest_obj != null)
        {
            ShoutTarget = nearest_obj;
        }
        
    }


    public override Canvas Render(int x, int y, int width, int height, string Image_path = null)
    {
        y -= 130;
        x -= 50;


        Image_path = "../../../../Resources/Sprites/Buildings/Tower_Blue.png";

        return base.Render(x, y, width, height, Image_path);
    }

    public override void FixedUpdate()
    {
        FindClosestEnemy();

        if (_timeRemaining <= 0)
        {
            if (ShoutTarget != null)
            {

                Shout();
            }
            _timeRemaining = ReloadTime;
        }

        
    }


    private void Reload()
    {
        _timeRemaining--;
    }

    public override void UnSub()
    {
        MainLoopMech.AFixedUpdate -= FixedUpdate;
        Service.main_timers.OneSec_timer.action -= Reload;
    }
}