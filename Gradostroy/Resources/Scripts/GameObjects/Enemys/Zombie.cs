using Gradostroy;
using System.Windows.Controls;
using System;
using System.Windows.Media.Imaging;
using Gradostroy.Windows;
using System.Linq;
using Gradostroy.Main_mechanics;
using System.Windows;

public class Zombie : Enemy
{
    private double step_for_min;
    private bool who_max;

    public Zombie(int XP = -1, int Damage = -1, Building Target = null)
    {
        _damage = Damage != -1 ? Damage : _damage;
        _XP = XP != -1 ? XP : _XP;
        _target = Target;
        speed = 2;
        MainLoopMech.AFixedUpdate += FixedUpdate;

        BaseName = "Zombie";


    }

    ~Zombie()
    {

    }


    private void FindClosestBuilding()
    {
        if (MainGameWindow.Buildings_list.Count == 0) return;

        double targetX, targetY, deltaX, deltaY, distance = double.PositiveInfinity;
        Building nearest_obj = null;


        foreach (var target in MainGameWindow.Buildings_list.Values)
        {
            // Получаем координаты цели
            targetX = ((Building)target).position.x;
            targetY = ((Building)target).position.y;

            // Вычисляем разницу между текущими координатами и координатами цели
            deltaX = targetX - position.x;
            deltaY = targetY - position.y;

            // Вычисляем расстояние до цели
            var new_distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);


            if (distance > new_distance)
            {
                distance = new_distance;
                nearest_obj = target;
            }
        }

        _target = nearest_obj;

        var dif_x = Math.Max(((Building)_target).position.x, position.x) - Math.Min(((Building)_target).position.x, position.x);
        var dif_y = Math.Max(((Building)_target).position.y, position.y) - Math.Min(((Building)_target).position.y, position.y);

        who_max = dif_x > dif_y ? true : false;

        var max_pos = Math.Max(dif_y, dif_x);
        var min_pos = Math.Min(dif_y, dif_x);

        step_for_min = (speed * min_pos) / max_pos;
    }



    public override void FixedUpdate()
    {
        FindClosestBuilding();
        GoToTarget();
    }

    private void GoToTarget()
    {
        if (_target == null)
        {
            return;
        } 
        // Получаем координаты цели
        double targetX = ((Building)_target).position.x;
        double targetY = ((Building)_target).position.y;

        // Вычисляем разницу между текущими координатами и координатами цели
        double deltaX = targetX - position.x;
        double deltaY = targetY - position.y;

        // Вычисляем расстояние до цели
        double distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);

        // Проверка на столкновение с башней
        if (distance < CollisionRadius)
        {
            HandleCollision(); // Метод для обработки столкновения
            return;
        }

        // Если зомби достиг цели, выходим из метода
        if (distance < speed)
        {
            position.x = targetX;
            position.y = targetY;
            Canvas.SetLeft(sprite, position.x);
            Canvas.SetTop(sprite, position.y);
            return;
        }

        // Нормализуем вектор направления
        double normalizedX = deltaX / distance;
        double normalizedY = deltaY / distance;

        // Обновляем позицию зомби с учетом направления
        position.x += normalizedX * speed;
        position.y += normalizedY * speed;

        UpdatePivot();

        // Устанавливаем новую позицию зомби
        Canvas.SetLeft(sprite, position.x);
        Canvas.SetTop(sprite, position.y);
    }

    public override void UnSub()
    {
        MainLoopMech.AFixedUpdate -= FixedUpdate;
    }

    private void HandleCollision()
    {
        // Логика обработки столкновения (например, уменьшение здоровья башни или уничтожение зомби)
        Console.WriteLine("regrg");
        _target.GetDamage(_damage);
        if (!_target.Check_XP())
        {
            _target = null;
        }
    }
}
