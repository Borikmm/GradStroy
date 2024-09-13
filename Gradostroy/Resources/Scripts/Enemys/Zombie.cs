using Gradostroy;
using System.Windows.Controls;
using System;
using System.Windows.Media.Imaging;
using Gradostroy.Windows;
using System.Linq;
using Gradostroy.Main_mechanics;

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
            targetX = ((Building)target).Pos_X;
            targetY = ((Building)target).Pos_Y;

            // Вычисляем разницу между текущими координатами и координатами цели
            deltaX = targetX - Pos_X;
            deltaY = targetY - Pos_Y;

            // Вычисляем расстояние до цели
            var new_distance = Math.Sqrt(deltaX * deltaX + deltaY * deltaY);


            if (distance > new_distance)
            {
                distance = new_distance;
                nearest_obj = target;
            }
        }

        _target = nearest_obj;

        var dif_x = Math.Max(((Building)_target).Pos_X, Pos_X) - Math.Min(((Building)_target).Pos_X, Pos_X);
        var dif_y = Math.Max(((Building)_target).Pos_Y, Pos_Y) - Math.Min(((Building)_target).Pos_Y, Pos_Y);

        who_max = dif_x > dif_y ? true : false;

        var max_pos = Math.Max(dif_y, dif_x);
        var min_pos = Math.Min(dif_y, dif_x);

        step_for_min = (speed * min_pos) / max_pos;
    }


    public override Canvas Render(int x, int y)
    {

        Pos_X = x; Pos_Y = y;
        Canvas canvas = new Canvas();

        Image image = new Image
        {
            Width = 150,
            Height = 100,
            Source = new BitmapImage(new Uri(Service.Sprites[Service.Random.Next(1, 4)], UriKind.Relative)),
        };

        Image = image;

        Canvas.SetLeft(image, x);
        Canvas.SetTop(image, y);

        canvas.Children.Add(image);
        CanvasRendered = canvas;
        return canvas;

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
        double targetX = ((Building)_target).Pos_X;
        double targetY = ((Building)_target).Pos_Y;

        // Вычисляем разницу между текущими координатами и координатами цели
        double deltaX = targetX - Pos_X;
        double deltaY = targetY - Pos_Y;

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
            Pos_X = targetX;
            Pos_Y = targetY;
            Canvas.SetLeft(Image, Pos_X);
            Canvas.SetTop(Image, Pos_Y);
            return;
        }

        // Нормализуем вектор направления
        double normalizedX = deltaX / distance;
        double normalizedY = deltaY / distance;

        // Обновляем позицию зомби с учетом направления
        Pos_X += normalizedX * speed;
        Pos_Y += normalizedY * speed;

        // Устанавливаем новую позицию зомби
        Canvas.SetLeft(Image, Pos_X);
        Canvas.SetTop(Image, Pos_Y);
    }

    private void HandleCollision()
    {
        // Логика обработки столкновения (например, уменьшение здоровья башни или уничтожение зомби)
        Console.WriteLine("regrg");
        _target.GetDamage(_damage);
        if (!((Building)_target).Check_XP())
        {
            MainGameWindow.ADestroyBuilding?.Invoke(((Building)_target).CanvasRendered);
            _target = null;
        }
    }
}
